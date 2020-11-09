using System;
using System.Collections.Generic;
using System.Linq;
using Skclusive.Mobx.Observable;
using Skclusive.Mobx.StateTree;

namespace Skclusive.Mobx.JsonSchema
{
    public interface INullActions : IAnyActions
    {
    }

    public interface INullObservable : IAnyObservable, INullPrimitive, INullActions
    {
    }

    internal class NullProxy : AnyProxy<INullObservable>, INullObservable
    {
        public override INullObservable Proxy => this;

        public NullProxy(IObservableObject<INullObservable, INode> target) : base(target)
        {
        }

        public object Value
        {
            get => Read<object>(nameof(Value));
            set => Write(nameof(Value), value);
        }
    }

    public partial class AppTypes
    {
        public static IObjectType<INull, INullObservable> CreateNullType()
            => CreateAnyType<INull, INullObservable>(
                SchemaType.Null,
                x => new NullProxy(x),
                () => new Null())
                .Mutable(o => o.Value, Types.Null)
                .View(o => o.Modified, Types.Boolean, (o) => !object.Equals(o.Initial, o.Data))
                .View(o => o.Valid, Types.Boolean, (o) => o.Errors.Count == 0)
                .View(o => o.Data, Types.Frozen, (o) => o.Value)
                .Action(o => o.Reset(), (o) =>
                {
                    o.Value = null;
                    o.Errors.Clear();
                })
                .Action<object, IEnumerable<string>>(o => o.ValidateData(default), (o, data) =>
                {
                    var errors = new List<string>();

                    if (!(data is null))
                    {
                        errors.Add($"should be of null");
                    }

                    return errors;
                })
                .Action<object>(o => o.SetData(default), (o, data) =>
                {
                    if (data is null)
                    {
                        o.Value = null;
                    }
                });

        public readonly static Lazy<IObjectType<INull, INullObservable>> NullType = new Lazy<IObjectType<INull, INullObservable>>(
                () => CreateNullType());
    }
}
