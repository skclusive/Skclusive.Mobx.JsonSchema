using System;
using System.Collections.Generic;
using System.Linq;
using Skclusive.Mobx.Observable;
using Skclusive.Mobx.StateTree;

namespace Skclusive.Mobx.JsonSchema
{
    public interface IAnyActions
    {
        void SetTitle(string title);

        void SetData(object data);

        void Reset();

        void Validate();

        void SyncData(object data);

        IEnumerable<string> ValidateData(object data);
    }

    public interface IAnyVolatile
    {
        object Initial { set; get; }
    }

    public interface IAnyObservable : IAnyPrimitive, IMetaObservable, IAnyActions, IAnyVolatile
    {
        bool Modified { get; }

        bool Valid { get; }

        IList<string> Errors { set; get; }

        object Data { get; }
    }

    public abstract class AnyProxy<A> : MetaProxy<A>, IAnyObservable
       where A : IAnyObservable
    {
        public AnyProxy(IObservableObject<A, INode> target) : base(target)
        {
        }

        public SchemaType Type
        {
            get => Read<SchemaType>(nameof(Type));
            set => Write(nameof(Type), value);
        }

        public string Title
        {
            get => Read<string>(nameof(Title));
            set => Write(nameof(Title), value);
        }


        public bool Validating
        {
            get => Read<bool>(nameof(Validating));
            set => Write(nameof(Validating), value);
        }

        public bool Syncing
        {
            get => Read<bool>(nameof(Syncing));
            set => Write(nameof(Syncing), value);
        }

        public IList<string> Errors
        {
            get => Read<IList<string>>(nameof(Errors));
            set => Write(nameof(Errors), value);
        }

        public object Data => Read<object>(nameof(Data));

        public bool Modified => Read<bool>(nameof(Modified));

        public bool Valid => Read<bool>(nameof(Valid));

        object IAnyVolatile.Initial
        {
            get => Read<object>(nameof(IAnyVolatile.Initial));
            set => Write(nameof(IAnyVolatile.Initial), value);
        }

        public void SetTitle(string title)
        {
            (Target as dynamic).SetTitle(title);
        }

        public void SetData(object data)
        {
            (Target as dynamic).SetData(data);
        }

        public void SyncData(object data)
        {
            (Target as dynamic).SyncData(data);
        }

        public void Reset()
        {
            (Target as dynamic).Reset();
        }

        public void Validate()
        {
            (Target as dynamic).Validate();
        }

        public IEnumerable<string> ValidateData(object data)
        {
            return (Target as dynamic).ValidateData(data);
        }
    }

    public partial class AppTypes
    {
        public static IObjectType<S, O> CreateAnyType<S, O>(
           SchemaType schemaType,
           Func<IObservableObject<O, INode>, O> proxify,
           Func<S> snpashoty)
           where S : IAny
           where O : IAnyObservable
            => Types.Late($"Late{schemaType}AnyType", () => Types
           .Compose<S, O, IMeta, IMetaObservable>($"{schemaType}AnyType", MetaType)
           .Proxy(proxify)
           .Snapshot(snpashoty)
           .Volatile(o => o.Initial)
           .Hook(Hook.AfterCreate, (o) => o.Initial = o.Data)
           .Mutable(o => o.Type, Types.Literal(schemaType))
           .Mutable(o => o.Title, Types.String)
           .Mutable(o => o.Validating, Types.Boolean)
           .Mutable(o => o.Syncing, Types.Boolean)
           .Mutable(o => o.Errors, Types.Optional(Types.List(Types.String), System.Array.Empty<string>()))
           .Action(o => o.Validate(), (o) =>
           {
                if (!o.Validating && !o.Syncing)
                {
                    o.Errors.Clear();

                    o.Validating = true;

                    var errors = o.ValidateData(o.Data);

                    foreach (var error in errors)
                    {
                        o.Errors.Add(error);
                    }

                    o.Validating = false;
                }
            })
            .Action<object>(o => o.SyncData(default), (o, data) =>
            {
                if (!o.Syncing)
                {
                    o.Syncing = true;

                    o.SetData(data);

                    o.Syncing = false;

                    o.Validate();
                }
            })
        );

        public readonly static Lazy<IType<IAny, IAnyObservable>> AnyType = new Lazy<IType<IAny, IAnyObservable>>
        (
            () => Types.Union<IAny, IAnyObservable>
            (
                options: new UnionOptions
                {
                    Dispatcher = (object value) =>
                    {
                        if (value is IAny any)
                        {
                            switch (any.Type)
                            {
                                case SchemaType.Boolean: return BooleanType.Value;

                                case SchemaType.Null: return NullType.Value;

                                case SchemaType.Number: return NumberType.Value;

                                case SchemaType.String: return StringType.Value;

                                case SchemaType.Object: return ObjectType.Value;

                                case SchemaType.Array: return ArrayType.Value;
                            }
                        }

                        return NullType.Value;
                    }
                },

                BooleanType.Value,

                NullType.Value,

                StringType.Value,

                NumberType.Value
            )
        );
    }
}
