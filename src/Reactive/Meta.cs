using System;
using Skclusive.Mobx.Observable;
using Skclusive.Mobx.StateTree;

namespace Skclusive.Mobx.JsonSchema
{
    public interface IMetaActions
    {
    }

    public interface IMetaObservable : IMetaPrimitive, IMetaActions
    {
    }

    public abstract class MetaProxy<T> : ObservableProxy<T, INode>, IMetaObservable
    where T : IMetaObservable
    {

        public MetaProxy(IObservableObject<T, INode> target) : base(target)
        {
        }

        public string Error
        {
            get => Read<string>(nameof(Error));
            set => Write(nameof(Error), value);
        }
    }

    public class MetaProxy : MetaProxy<IMetaObservable>
    {
        public override IMetaObservable Proxy => this;

        public MetaProxy(IObservableObject<IMetaObservable, INode> target) : base(target)
        {
        }
    }

    public partial class AppTypes
    {
        public readonly static IObjectType<IMeta, IMetaObservable> MetaType = Types.
            Object<IMeta, IMetaObservable>("MetaType")
            .Proxy(x => new MetaProxy(x))
            .Snapshot(() => new Meta())
            .Mutable(o => o.Error, Types.Maybe(Types.String));
    }
}
