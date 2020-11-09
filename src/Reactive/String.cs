using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
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

        public int? MinLength
        {
            get => Read<int?>(nameof(MinLength));
            set => Write(nameof(MinLength), value);
        }

        public int? MaxLength
        {
            get => Read<int?>(nameof(MaxLength));
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

        private readonly static IDictionary<Format, Func<string, bool>> Validators = new Dictionary<Format, Func<string, bool>>
        {
            [Format.None] = value => true,

            [Format.URI] = value => Uri.TryCreate(value, UriKind.Absolute, out _),

            [Format.Guid] = value => Guid.TryParse(value, out _),

            [Format.Integer] = value => int.TryParse(value, out _),

            [Format.Float] = value => float.TryParse(value, out _),

            [Format.Double] = value => double.TryParse(value, out _),

            [Format.Decimal] = value => decimal.TryParse(value, out _),

            [Format.IPv6] = value => Uri.CheckHostName(value) == UriHostNameType.IPv6,

            [Format.Alpha] = value => Regex.IsMatch(value, "^[a-zA-Z]+$", RegexOptions.IgnoreCase),

            [Format.AlphaNumeric] = value => Regex.IsMatch(value, "^[a-zA-Z0-9]+$", RegexOptions.IgnoreCase),

            [Format.Hexadecimal] = value => Regex.IsMatch(value, "^[a-fA-F0-9]+$", RegexOptions.IgnoreCase),

            [Format.Identifier] = value => Regex.IsMatch(value, "^[-_a-zA-Z0-9]+$", RegexOptions.IgnoreCase),

            [Format.Numeric] = value => Regex.IsMatch(value, "^[0-9]+$", RegexOptions.IgnoreCase),

            [Format.Phone] = value => Regex.IsMatch(value, "^\\+(?:[0-9] ?){6,14}[0-9]$", RegexOptions.IgnoreCase),

            [Format.Style] = value => Regex.IsMatch(value, "\\s*(.+?):\\s*([^;]+);?", RegexOptions.IgnoreCase),

            [Format.Lowecase] = value => value == value.ToLower(),

            [Format.Uppercase] = value => value == value.ToUpper(),

            [Format.IPAddress] = value => Regex.IsMatch(value, "^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", RegexOptions.IgnoreCase),

            [Format.UUID] = value => Regex.IsMatch(value, "^(?:urn:uuid:)?[0-9a-f]{8}-(?:[0-9a-f]{4}-){3}[0-9a-f]{12}$", RegexOptions.IgnoreCase),

            [Format.Color] = value => Regex.IsMatch(value, "^(#?([0-9A-Fa-f]{3}){1,2}\\b|aqua|black|blue|fuchsia|gray|green|lime|maroon|navy|olive|orange|purple|red|silver|teal|white|yellow|(rgb\\(\\s*\\b([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])\\b\\s*,\\s*\\b([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])\\b\\s*,\\s*\\b([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])\\b\\s*\\))|(rgb\\(\\s*(\\d?\\d%|100%)+\\s*,\\s*(\\d?\\d%|100%)+\\s*,\\s*(\\d?\\d%|100%)+\\s*\\)))$", RegexOptions.IgnoreCase),

            [Format.IPv4] = value => Regex.IsMatch(value, @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?).){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", RegexOptions.IgnoreCase),

            [Format.HostName] = value => Regex.IsMatch(value, "^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\\-]*[a-zA-Z0-9])\\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\\-]*[A-Za-z0-9])$", RegexOptions.IgnoreCase),

            [Format.Email] = value => Regex.IsMatch(value, @"^\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z$", RegexOptions.IgnoreCase),

            [Format.Time] = value => DateTime.TryParseExact(value, "HH:mm:ss.FFFFFFFK", null, DateTimeStyles.None, out _),

            [Format.DateTime] = value => DateTimeOffset.TryParseExact(value, new string[]
            {
                "yyyy-MM-dd'T'HH:mm:ss.FFFFFFFK",
                "yyyy-MM-dd' 'HH:mm:ss.FFFFFFFK",
                "yyyy-MM-dd'T'HH:mm:ssK",
                "yyyy-MM-dd' 'HH:mm:ssK",
                "yyyy-MM-dd'T'HH:mm:ss",
                "yyyy-MM-dd' 'HH:mm:ss",
                "yyyy-MM-dd'T'HH:mm",
                "yyyy-MM-dd' 'HH:mm",
                "yyyy-MM-dd'T'HH",
                "yyyy-MM-dd' 'HH",
                //"yyyy-MM-dd",
                //"yyyy-MM-dd",
                //"yyyyMMdd",
                //"yyyy-MM",
                //"yyyy"
            }, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset dateTimeResult),

            [Format.Date] = value => DateTime.TryParseExact(value, "yyyy-MM-dd", null, DateTimeStyles.None, out DateTime dateTimeResult)
                    && dateTimeResult.Date == dateTimeResult,

            [Format.Base64] = value => (value.Length % 4 == 0) && Regex.IsMatch(value, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None)
        };

        public static IObjectType<IString, IStringObservable> CreateStringType()
            => CreateValueType<string, IString, IStringObservable>(
                typeof(string).Name,
                SchemaType.String,
                x => new StringProxy(x),
                () => new String(),
                () => Types.String)
                .Mutable(o => o.MinLength, Types.Maybe(Types.Int))
                .Mutable(o => o.MaxLength, Types.Maybe(Types.Int))
                .Mutable(o => o.Pattern, Types.Maybe(Types.String))
                .Mutable(o => o.Format, Types.Optional(FormatType, Format.None))
                .Action<string, IEnumerable<string>>(o => o.ValidateValue(default), (o, value) =>
                {
                    var errors = new List<string>();
                    if (o.MinLength.HasValue && value.Length < o.MinLength)
                    {
                        errors.Add($"should NOT be shorter than {o.MinLength} characters");
                    }
                    if (o.MaxLength.HasValue && value.Length > o.MaxLength)
                    {
                        errors.Add($"should NOT be longer than ${o.MaxLength} characters");
                    }
                    if (!string.IsNullOrWhiteSpace(o.Pattern) && !Regex.IsMatch(value, o.Pattern))
                    {
                        errors.Add($"should match pattern {o.Pattern}");
                    }
                    if (!Validators[o.Format].Invoke(value))
                    {
                        errors.Add($"should match format {o.Format}");
                    }
                    return errors;
                });

        public readonly static Lazy<IObjectType<IString, IStringObservable>> StringType = new Lazy<IObjectType<IString, IStringObservable>>(
                () => CreateStringType());
    }
}
