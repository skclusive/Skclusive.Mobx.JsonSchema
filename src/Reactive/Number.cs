using System;
using System.Collections.Generic;
using System.Linq;
using Skclusive.Mobx.Observable;
using Skclusive.Mobx.StateTree;

namespace Skclusive.Mobx.JsonSchema
{
    public interface INumberActions
    {
    }

    public interface INumberObservable : IValueObservable<double?>, INumberPrimitive, INumberActions
    {
    }

    internal class NumberProxy : ValueProxy<double?, INumberObservable>, INumberObservable
    {
        public override INumberObservable Proxy => this;

        public NumberProxy(IObservableObject<INumberObservable, INode> target) : base(target)
        {
        }

        public int? Minimum
        {
            get => Read<int?>(nameof(Minimum));
            set => Write(nameof(Minimum), value);
        }

        public int? Maximum
        {
            get => Read<int?>(nameof(Maximum));
            set => Write(nameof(Maximum), value);
        }

        public int? MultipleOf
        {
            get => Read<int?>(nameof(MultipleOf));
            set => Write(nameof(MultipleOf), value);
        }
    }

    public partial class AppTypes
    {
        public static IObjectType<INumber, INumberObservable> CreateNumberType()
            => CreateValueType<double?, INumber, INumberObservable>(
                typeof(double).Name,
                SchemaType.Number,
                x => new NumberProxy(x),
                () => new Number(),
                () => Types.Maybe<double>(Types.Double))
                .Mutable(o => o.Minimum, Types.Maybe<int>(Types.Int))
                .Mutable(o => o.Maximum, Types.Maybe<int>(Types.Int))
                .Mutable(o => o.MultipleOf, Types.Maybe<int>(Types.Int))
                .Action<double, IEnumerable<string>>(o => o.ValidateValue(default), (o, value) =>
                {
                    var errors = new List<string>();
                    if (o.Minimum.HasValue && value < o.Minimum)
                    {
                        errors.Add($"should NOT be lesser than {o.Minimum}");
                    }
                    if (o.Maximum.HasValue && value > o.Maximum)
                    {
                        errors.Add($"should NOT be greater than {o.Maximum}");
                    }
                    if (o.MultipleOf.HasValue && (value % o.MultipleOf.Value != 0))
                    {
                        errors.Add($"should be multiple of {o.MultipleOf}");
                    }
                    return errors;
                });

        public readonly static Lazy<IObjectType<INumber, INumberObservable>> NumberType = new Lazy<IObjectType<INumber, INumberObservable>>(
                () => CreateNumberType());
    }
}
