using System;
using System.Collections.Generic;
using Skclusive.Mobx.Observable;
using Skclusive.Mobx.StateTree;

namespace Skclusive.Mobx.JsonSchema
{
    public interface IStringActions
    {
    }

    public interface IStringObservable : IValueObservable<string>, IStringPrimitive, IStringActions
    {
    }

    internal class StringProxy : ValueProxy<string, IStringObservable>, IStringObservable
    {
        public override IStringObservable Proxy => this;

        public StringProxy(IObservableObject<IStringObservable, INode> target) : base(target)
        {
        }

        public int MinLength
        {
            get => Read<int>(nameof(MinLength));
            set => Write(nameof(MinLength), value);
        }

        public int MaxLength
        {
            get => Read<int>(nameof(MaxLength));
            set => Write(nameof(MaxLength), value);
        }

        public string Pattern
        {
            get => Read<string>(nameof(Pattern));
            set => Write(nameof(Pattern), value);
        }

        public Format Format
        {
            get => Read<Format>(nameof(Format));
            set => Write(nameof(Format), value);
        }
    }

    public partial class AppTypes
    {
        public readonly static IType<Format, Format> FormatType = Types.Late("LateFormat", () => Types.Enumeration
        (
            Enum.GetValues<Format>()
        ));

        public static IObjectType<IString, IStringObservable> CreateStringType()
            => CreateValueType<string, IString, IStringObservable>(
                SchemaType.String,
                x => new StringProxy(x),
                () => new String(),
                () => Types.String)
                .Mutable(o => o.MinLength, Types.Int)
                .Mutable(o => o.MaxLength, Types.Int)
                .Mutable(o => o.Pattern, Types.String)
                .Mutable(o => o.Format, FormatType);

        public readonly static Lazy<IObjectType<IString, IStringObservable>> StringType = new Lazy<IObjectType<IString, IStringObservable>>(
                () => CreateStringType());
    }
}
