using System;
using System.Collections.Generic;
using Skclusive.Mobx.Observable;
using Skclusive.Mobx.StateTree;

namespace Skclusive.Mobx.JsonSchema
{
    public interface INumberActions
    {
    }

    public interface INumberObservable : IValueObservable<double>, INumberPrimitive, INumberActions
    {
    }

    internal class NumberProxy : ValueProxy<double, INumberObservable>, INumberObservable
    {
        public override INumberObservable Proxy => this;

        public NumberProxy(IObservableObject<INumberObservable, INode> target) : base(target)
        {
        }

        public int Minimum
        {
            get => Read<int>(nameof(Minimum));
            set => Write(nameof(Minimum), value);
        }

        public int Maximum
        {
            get => Read<int>(nameof(Maximum));
            set => Write(nameof(Maximum), value);
        }

        public int MultipleOf
        {
            get => Read<int>(nameof(MultipleOf));
            set => Write(nameof(MultipleOf), value);
        }
    }

    public partial class AppTypes
    {
        public static IObjectType<INumber, INumberObservable> CreateNumberType()
            => CreateValueType<double, INumber, INumberObservable>(
                SchemaType.Number,
                x => new NumberProxy(x),
                () => new Number(),
                () => Types.Double)
                .Mutable(o => o.Minimum, Types.Int)
                .Mutable(o => o.Maximum, Types.Int)
                .Mutable(o => o.MultipleOf, Types.Int);

        public readonly static Lazy<IObjectType<INumber, INumberObservable>> NumberType = new Lazy<IObjectType<INumber, INumberObservable>>(
                () => CreateNumberType());
    }
}
