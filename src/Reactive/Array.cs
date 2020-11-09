using System;
using System.Collections.Generic;
using System.Linq;
using Skclusive.Core.Collection;
using Skclusive.Mobx.Observable;
using Skclusive.Mobx.StateTree;

namespace Skclusive.Mobx.JsonSchema
{
    public interface IArrayActions : IAnyActions
    {
        void SetValue(IList<object> value);
    }

    public interface IArrayObservable : IAnyObservable, IArrayPrimitive, IArrayActions
    {
        IList<object> Value { set; get; }

        IList<IAnyObservable> Items { set; get; }
    }

    internal class ArrayProxy : AnyProxy<IArrayObservable>, IArrayObservable
    {
        public override IArrayObservable Proxy => this;

        public ArrayProxy(IObservableObject<IArrayObservable, INode> target) : base(target)
        {
        }

        public IList<IAnyObservable> Items
        {
            get => Read<IList<IAnyObservable>>(nameof(Items));
            set => Write(nameof(Items), value);
        }

        public int? MinItems
        {
            get => Read<int?>(nameof(MinItems));
            set => Write(nameof(MinItems), value);
        }

        public int? MaxItems
        {
            get => Read<int?>(nameof(MaxItems));
            set => Write(nameof(MaxItems), value);
        }


        public bool? UniqueItems
        {
            get => Read<bool?>(nameof(UniqueItems));
            set => Write(nameof(UniqueItems), value);
        }

        public bool? AdditionalItems
        {
            get => Read<bool?>(nameof(AdditionalItems));
            set => Write(nameof(AdditionalItems), value);
        }

        public IList<object> Value
        {
            get => Read<IList<object>>(nameof(Value));
            set => Write(nameof(Value), value);
        }

        public void SetValue(IList<object> value)
        {
            (Target as dynamic).SetValue(value);
        }
    }

    public partial class AppTypes
    {
        public static IObjectType<IArray, IArrayObservable> CreateArrayType()
            => CreateAnyType<IArray, IArrayObservable>(
                SchemaType.Array,
                x => new ArrayProxy(x),
                () => new Array())
                .Mutable(o => o.MinItems, Types.Maybe(Types.Int))
                .Mutable(o => o.MaxItems, Types.Maybe(Types.Int))
                .Mutable(o => o.UniqueItems, Types.Maybe(Types.Boolean))
                .Mutable(o => o.AdditionalItems, Types.Maybe(Types.Boolean))
                .Mutable(o => o.Items, Types.Late("LateItemsType", () => Types.List(AnyType.Value)))
                .View(o => o.Modified, Types.Boolean, (o) => o.Items.Any(item => item.Modified))
                .View(o => o.Valid, Types.Boolean, (o) => o.Errors.Count == 0 && o.Items.All(item => item.Valid))
                .View(o => o.Value, Types.List(Types.Frozen), (o) =>
                {
                    return o.Items.Select(item => item?.Data).ToList();
                })
                .View(o => o.Data, Types.Frozen, (o) => o.Value)
                .Action(o => o.Reset(), (o) =>
                {
                    foreach (var item in o.Items)
                    {
                         item.Reset();
                    }
                    o.Errors.Clear();
                })
                .Action<object, IEnumerable<string>>(o => o.ValidateData(default), (o, data) =>
                {
                    var errors = new List<string>();

                    if (data is IList<object> value)
                    {
                        if (o.MinItems.HasValue && value.Count < o.MinItems)
                        {
                            errors.Add($"should NOT have less than ${o.MinItems} items");
                        }

                        if (o.MaxItems.HasValue && value.Count > o.MaxItems)
                        {
                            errors.Add($"should NOT have more than ${o.MaxItems} items");
                        }
                    }
                    else
                    {
                        errors.Add($"should be of type {typeof(IMap<string, object>).Name}");
                    }

                    foreach (var item in o.Items)
                    {
                        item?.Validate();
                    }

                    return errors;
                })
                .Action<object>(o => o.SetData(default), (o, data) =>
                {
                    if (data is IList<object> value)
                    {
                        for (var i = 0; i < value.Count; i++)
                        {
                            var item = o.Items[i];

                            item?.SetData(value[i]);
                        }
                    }
                });

        public readonly static Lazy<IObjectType<IArray, IArrayObservable>> ArrayType = new Lazy<IObjectType<IArray, IArrayObservable>>(
                () => CreateArrayType());
    }
}
