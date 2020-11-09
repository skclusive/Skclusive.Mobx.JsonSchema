using System;
using System.Collections.Generic;
using System.Linq;
using Skclusive.Core.Collection;
using Skclusive.Mobx.Observable;
using Skclusive.Mobx.StateTree;

namespace Skclusive.Mobx.JsonSchema
{
    public interface IObjectActions : IAnyActions
    {
        void SetValue(IMap<string, object> value);
    }

    public interface IObjectObservable : IAnyObservable, IObjectPrimitive, IObjectActions
    {
        IMap<string, object> Value { set; get; }

        IList<string> Required { set; get; }

        IMap<string, IAnyObservable> Properties { set; get; }

        IList<IAnyObservable> Fields { get; }
    }

    internal class ObjectProxy : AnyProxy<IObjectObservable>, IObjectObservable
    {
        public override IObjectObservable Proxy => this;

        public ObjectProxy(IObservableObject<IObjectObservable, INode> target) : base(target)
        {
        }

        public IList<string> Required
        {
            get => Read<IList<string>>(nameof(Required));
            set => Write(nameof(Required), value);
        }

        public IMap<string, IAnyObservable> Properties
        {
            get => Read<IMap<string, IAnyObservable>>(nameof(Properties));
            set => Write(nameof(Properties), value);
        }

        public int? MinProperties
        {
            get => Read<int?>(nameof(MinProperties));
            set => Write(nameof(MinProperties), value);
        }

        public int? MaxProperties
        {
            get => Read<int?>(nameof(MaxProperties));
            set => Write(nameof(MaxProperties), value);
        }

        public IList<IAnyObservable> Fields => Read<IList<IAnyObservable>>(nameof(Fields));

        public IMap<string, object> Value
        {
            get => Read<IMap<string, object>>(nameof(Value));
            set => Write(nameof(Value), value);
        }

        public void SetValue(IMap<string, object> value)
        {
            (Target as dynamic).SetValue(value);
        }
    }

    public partial class AppTypes
    {
        public static IObjectType<IObject, IObjectObservable> CreateObjectType()
            => CreateAnyType<IObject, IObjectObservable>(
                SchemaType.Object,
                x => new ObjectProxy(x),
                () => new Object())
                .Mutable(o => o.Required, Types.Optional(Types.List(Types.String), System.Array.Empty<string>()))
                .Mutable(o => o.MinProperties, Types.Maybe(Types.Int))
                .Mutable(o => o.MaxProperties, Types.Maybe(Types.Int))
                .Mutable(o => o.Properties, Types.Late("LatePropertiesType", () => Types.Map(AnyType.Value)))
                .View(o => o.Fields, Types.Late("LateFieldsType", () => AnyType.Value), (o) => o.Properties.Values)
                .View(o => o.Modified, Types.Boolean, (o) => o.Properties.Any(prop => prop.Value.Modified))
                .View(o => o.Valid, Types.Boolean, (o) => o.Errors.Count == 0 && o.Properties.Values.All(prop => prop.Valid))
                .View(o => o.Value, Types.Map(Types.Frozen), (o) =>
                {
                    var value = new Map<string, object>();
                    foreach (var property in o.Properties)
                    {
                        value[property.Key] = property.Value?.Data;
                    }
                    return value;
                })
                .View(o => o.Data, Types.Frozen, (o) => o.Value)
                .Action(o => o.Reset(), (o) =>
                {
                    foreach (var property in o.Properties.Values)
                    {
                        property.Reset();
                    }
                    o.Errors.Clear();
                })
                .Action<object, IEnumerable<string>>(o => o.ValidateData(default), (o, data) =>
                {
                    var errors = new List<string>();

                    if (data is IMap<string, object> value)
                    {
                        if (o.MinProperties.HasValue && value.Count < o.MinProperties)
                        {
                            errors.Add($"should NOT have less than ${o.MinProperties} properties");
                        }

                        if (o.MaxProperties.HasValue && value.Count > o.MaxProperties)
                        {
                            errors.Add($"should NOT have more than ${o.MaxProperties} properties");
                        }
                        //foreach (var key in value.Keys)
                        //{
                        //    var property = o.Properties[key];

                        //    errors.AddRange(property?.ValidateData(value[key]));
                        //}
                    }
                    else
                    {
                        errors.Add($"should be of type {typeof(IMap<string, object>).Name}");
                    }

                    // additionalProperty validation need to be added

                    foreach (var property in o.Properties.Values)
                    {
                        property?.Validate();
                    }

                    return errors;
                 })
                .Action<object>(o => o.SetData(default), (o, data) =>
                {
                    if (data is IMap<string, object> value)
                    {
                        foreach (var key in value.Keys)
                        {
                            var property = o.Properties[key];

                            property?.SetData(value[key]);
                        }
                    }
                });

        public readonly static Lazy<IObjectType<IObject, IObjectObservable>> ObjectType = new Lazy<IObjectType<IObject, IObjectObservable>>(
                () => CreateObjectType());
    }
}
