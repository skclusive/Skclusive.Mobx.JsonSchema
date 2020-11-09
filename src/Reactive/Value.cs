using System;
using System.Collections.Generic;
using System.Linq;
using Skclusive.Mobx.Observable;
using Skclusive.Mobx.StateTree;

namespace Skclusive.Mobx.JsonSchema
{
    public interface IValueActions<V> : IAnyActions
    {
        void SetValue(V value);

        IEnumerable<string> ValidateValue(V value);
    }

    public interface IValueObservable<V> : IAnyObservable, IValuePrimitive<V>, IValueActions<V>
    {
        IList<V> Enum { set; get; }
    }

    public abstract class ValueProxy<V, VO> : AnyProxy<VO>, IValueObservable<V>
        where VO : IValueObservable<V>
    {
        public ValueProxy(IObservableObject<VO, INode> target) : base(target)
        {
        }

        public IList<V> Enum
        {
            get => Read<IList<V>>(nameof(Enum));
            set => Write(nameof(Enum), value);
        }


        public V Const
        {
            get => Read<V>(nameof(Const));
            set => Write(nameof(Const), value);
        }

        public V Value
        {
            get => Read<V>(nameof(Value));
            set => Write(nameof(Value), value);
        }

        public void SetValue(V value)
        {
            (Target as dynamic).SetValue(value);
        }

        public IEnumerable<string> ValidateValue(V value)
        {
            return (Target as dynamic).ValidateValue(value);
        }
    }

    public partial class AppTypes
    {
        public readonly static IType<SchemaType, SchemaType> SchemaMobxType = Types.Late("LateSchemaType", () => Types.Enumeration
        (
            Enum.GetValues<SchemaType>()
        ));

        public static IObjectType<VS, VO> CreateValueType<V, VS, VO>(
            string name,
            SchemaType schemaType,
            Func<IObservableObject<VO, INode>, VO> proxify,
            Func<VS> snpashoty,
            Func<IType<V, V>> valueType)
            where VS : IValue<V>
            where VO : IValueObservable<V>
             => CreateAnyType(schemaType, proxify, snpashoty)
            .Mutable(o => o.Const, Types.Maybe(valueType()))
            .Mutable(o => o.Enum, Types.Maybe(Types.List(valueType())))
            .Mutable(o => o.Value, Types.Maybe(valueType()))
            .View(o => o.Data, Types.Frozen, (o) => o.Value)
            .View(o => o.Modified, Types.Boolean, (o) => !object.Equals(o.Initial, o.Data))
            .View(o => o.Valid, Types.Boolean, (o) => o.Errors.Count == 0)
            .Action(o => o.Reset(), (o) =>
            {
                o.SetData(o.Initial);
                o.Errors.Clear();
            })
            .Action<object, IEnumerable<string>>(o => o.ValidateData(default), (o, data) =>
            {
                var errors = new List<string>();

                if (data is V value)
                {
                    var typeErrors = valueType().Validate(value, System.Array.Empty<IContextEntry>());

                    errors.AddRange(typeErrors.Select(typeError => typeError.Message));

                    if (!object.Equals(o.Const, default) && !object.Equals(o.Const, value))
                    {
                        errors.Add($"should be equal to {o.Const}");
                    }

                    if (!object.Equals(o.Enum, default) && o.Enum.Count > 0 && !o.Enum.Contains(value))
                    {
                        errors.Add($"should be equal to one of the allowed values[{string.Join(" ,", o.Enum.Select(e => e.ToString()))}]");
                    }

                    var valueErrors = o.ValidateValue(value);

                    errors.AddRange(valueErrors);
                }
                else
                {
                    errors.Add($"should be of type {name}");
                }

                return errors;
            })
            .Action<V>(o => o.SetValue(default), (o, value) =>
            {
                o.Value = value;
            })
            .Action<object>(o => o.SetData(default), (o, data) =>
            {
                if (data is null || data is V)
                {
                    o.Value = (V)data;
                }
            })
            .Action<string>(o => o.SetTitle(default), (o, title) =>
            {
                o.Title = title;
            });
    }
}
