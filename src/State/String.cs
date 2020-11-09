using System;
using System.Collections.Generic;

namespace Skclusive.Mobx.JsonSchema
{
    public enum Format
    {
        None,

        Date,

        DateTime,

        URL,

        URI,

        URIReference,

        URITemplate,

        Email,

        HostName,

        IPv4,

        IPv6,

        RegEx,

        Guid,

        UUID,

        Alpha,

        AlphaNumeric,

        Hexadecimal,

        Identifier,

        Numeric,

        Time,

        Color,

        Style,

        Phone,

        IPAddress,

        Lowecase,

        Uppercase,

        UTCMillisec,

        Base64,

        Integer,

        Float,

        Double,

        Decimal
    }

    public interface IStringPrimitive
    {
        int? MinLength { set; get; }

        int? MaxLength { set; get; }

        string Pattern { set; get; }

        Format Format { set; get; }
    }

    public interface IString : IValue<string>, IStringPrimitive
    {
    }

    public class String : ValueSnapshot<string>, IString
    {
        public String()
        {
            Type = SchemaType.String;
        }

        public int? MinLength { set; get; }

        public int? MaxLength { set; get; }

        public string Pattern { set; get; }

        public Format Format { set; get; }
    }
}
