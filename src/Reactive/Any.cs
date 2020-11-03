using System;
using System.Collections.Generic;
using Skclusive.Mobx.Observable;
using Skclusive.Mobx.StateTree;

namespace Skclusive.Mobx.JsonSchema
{
    public interface IAnyActions
    {
        void SetTitle(string title);

        void SetData(object data);
    }

    public interface IAnyObservable : IMetaObservable, IAnyActions
    {
        SchemaType Type { set; get; }

        string Title { set; get; }

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

        public void SetTitle(string title)
        {
            (Target as dynamic).SetTitle(title);
        }

        public object Data => Read<object>(nameof(Data));

        public void SetData(object data)
        {
            (Target as dynamic).SetData(data);
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
           .Mutable(o => o.Type, Types.Literal(schemaType))
           .Mutable(o => o.Title, Types.String)
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
