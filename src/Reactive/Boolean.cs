using System;
using System.Collections.Generic;
using Skclusive.Mobx.Observable;
using Skclusive.Mobx.StateTree;

namespace Skclusive.Mobx.JsonSchema
{
    public interface IBooleanActions
    {
    }

    public interface IBooleanObservable : IValueObservable<bool>, IBooleanPrimitive, IBooleanActions
    {
    }

    internal class BooleanProxy : ValueProxy<bool, IBooleanObservable>, IBooleanObservable
    {
        public override IBooleanObservable Proxy => this;

        public BooleanProxy(IObservableObject<IBooleanObservable, INode> target) : base(target)
        {
        }
    }

    public partial class AppTypes
    {
        public static IObjectType<IBoolean, IBooleanObservable> CreateBooleanType()
            => CreateValueType<bool, IBoolean, IBooleanObservable>(
                SchemaType.Boolean,
                x => new BooleanProxy(x),
                () => new Boolean(),
                () => Types.Boolean);

        public readonly static Lazy<IObjectType<IBoolean, IBooleanObservable>> BooleanType = new Lazy<IObjectType<IBoolean, IBooleanObservable>>(
                () => CreateBooleanType());
    }
}
