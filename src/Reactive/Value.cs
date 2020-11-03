using System;
using System.Collections.Generic;
using Skclusive.Mobx.Observable;
using Skclusive.Mobx.StateTree;

namespace Skclusive.Mobx.JsonSchema
{
    public interface IValueActions<V> : IAnyActions
    {
        void SetValue(V value);

        void Reset();

        void Validate();
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

        public void Reset()
        {
            (Target as dynamic).Reset();
        }

        public void Validate()
        {
            (Target as dynamic).Validate();
        }
    }

    public partial class AppTypes
    {
        public readonly static IType<SchemaType, SchemaType> SchemaMobxType = Types.Late("LateSchemaType", () => Types.Enumeration
        (
            Enum.GetValues<SchemaType>()
        ));

        public static IObjectType<VS, VO> CreateValueType<V, VS, VO>(
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
            .Action(o => o.Reset(), (o) =>
            {
                 o.Value = default;
            })
            .Action(o => o.Validate(), (o) =>
            {
            })
            .Action<V>(o => o.SetValue(default), (o, value) =>
            {
                o.Value = value;
            })
            .Action<object>(o => o.SetData(default), (o, data) =>
            {
                if (data is V value)
                {
                    o.SetValue(value);
                }
            })
            .Action<string>(o => o.SetTitle(default), (o, title) =>
            {
                o.Title = title;
            });
    }
}
