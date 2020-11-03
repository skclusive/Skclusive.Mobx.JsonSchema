using System;
using System.Collections.Generic;
using Skclusive.Mobx.Observable;
using Skclusive.Mobx.StateTree;

namespace Skclusive.Mobx.JsonSchema
{
    public interface INullActions
    {
    }

    public interface INullObservable : IValueObservable<object>, INullPrimitive, INullActions
    {
    }

    internal class NullProxy : ValueProxy<object, INullObservable>, INullObservable
    {
        public override INullObservable Proxy => this;

        public NullProxy(IObservableObject<INullObservable, INode> target) : base(target)
        {
        }
    }

    public partial class AppTypes
    {
        public static IObjectType<INull, INullObservable> CreateNullType()
            => CreateValueType<object, INull, INullObservable>(
                SchemaType.Null,
                x => new NullProxy(x),
                () => new Null(),
                () => Types.Null);

        public readonly static Lazy<IObjectType<INull, INullObservable>> NullType = new Lazy<IObjectType<INull, INullObservable>>(
                () => CreateNullType());
    }
}
