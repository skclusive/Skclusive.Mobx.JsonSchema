using System.Linq;
using Xunit;

namespace Skclusive.Mobx.JsonSchema.Tests
{
    public class StringTest
    {
        [Fact]
        public void TestCreate()
        {
            var textual = AppTypes.StringType.Value.Create(new String
            {
               Type = SchemaType.String,

               Const = null,

               Enum = new string[] { "one", "two", "three" },

               Value = "two",

               Title = "textual?",

               MinLength = null,

               MaxLength = 8,

               //Pattern = "pattern",

               Format = Format.DateTime
            });

            Assert.Equal(SchemaType.String, textual.Type);

            Assert.Null(textual.Const);

            Assert.Equal("two", textual.Value);

            Assert.Equal(3, textual.Enum.Count);

            Assert.Equal("one", textual.Enum[0]);

            Assert.Equal("two", textual.Enum[1]);

            Assert.Equal("three", textual.Enum[2]);

            Assert.Null(textual.MinLength);

            Assert.Equal(8, textual.MaxLength);

            Assert.Null(textual.Pattern);

            Assert.Equal("textual?", textual.Title);

            Assert.Equal(Format.DateTime, textual.Format);

            textual.Validate();

            Assert.False(textual.Valid);

            Assert.Single(textual.Errors);

            Assert.Equal("should match format DateTime", textual.Errors[0]);
        }

        [Fact]
        public void TestDateTime()
        {
            var datetime = AppTypes.StringType.Value.Create(new String
            {
                Type = SchemaType.String,

                Value = "2012-07-08T16:41:41.532Z",

                Title = "datetime",

                MinLength = 8,

                MaxLength = 28,

                Format = Format.DateTime
            });

            Assert.Equal(SchemaType.String, datetime.Type);

            Assert.Null(datetime.Const);

            Assert.Equal("2012-07-08T16:41:41.532Z", datetime.Value);

            Assert.Equal(8, datetime.MinLength);

            Assert.Equal(28, datetime.MaxLength);

            Assert.Null(datetime.Pattern);

            Assert.Equal("datetime", datetime.Title);

            Assert.Equal(Format.DateTime, datetime.Format);

            datetime.Validate();

            Assert.True(datetime.Valid);

            Assert.Empty(datetime.Errors);
        }

        [Theory]
        [InlineData(Format.DateTime, "2012-07-08T16:41:41.532Z", true)] // should validate a valid date-time
        [InlineData(Format.DateTime, "2012-07-08T16:41:41Z", true)] // should validate a valid date-time without milliseconds
        [InlineData(Format.DateTime, "2012-07-08T16:41:41.532+00:00", true)] // should validate a date-time with a timezone offset instead of Z
        //[InlineData(Format.DateTime, "2012-07-08T16:41:41.532z", true)] // should validate a date-time with a z instead of a Z
        [InlineData(Format.DateTime, "2012-07-08 16:41:41.532Z", true)] // should validate a date-time with a space instead of a T
        //[InlineData(Format.DateTime, "2012-07-08t16:41:41.532Z", true)] // should validate a date-time with a t instead of a T
        [InlineData(Format.DateTime, "2012-07-08", false)] // should not validate a date-time with the time missing
        [InlineData(Format.DateTime, "TEST2012-07-08T16:41:41.532Z", false)] // should not validate an invalid date-time
        [InlineData(Format.DateTime, "2012-07-08T16:41:41.532+00:00Z", false)] // should not validate a date-time with a timezone offset AND a Z
        [InlineData(Format.DateTime, "2012-07-08T16:41:41.532+Z00:00", false)] // should not validate a date-time with a timezone offset AND a Z
        [InlineData(Format.Date, "2012-07-08", true)] // should validate a valid date
        [InlineData(Format.Date, "TEST2012-07-08", false)] // should not validate an invalid date
        [InlineData(Format.Time, "16:41:41", true)] // should validate a valid time
        [InlineData(Format.Time, "d16:41:41.532Z", false)] // should not validate an invalid time
        [InlineData(Format.Color, "red", true)] // should validate the color red
        [InlineData(Format.Color, "#f00", true)] // should validate the color #f00
        [InlineData(Format.Color, "#ff0000", true)] // should validate the color #ff0000
        [InlineData(Format.Color, "rgb(255,0,0)", true)] // should validate the color rgb(255,0,0)
        [InlineData(Format.Color, "json", false)] // should not validate an invalid color (json)
        [InlineData(Format.Style, "color: red;", true)] // should validate a valid style
        [InlineData(Format.Style, "color: red; position: absolute; background-color: rgb(204, 204, 204); max-width: 150px;", true)] // should validate a valid complex style
        [InlineData(Format.Color, "0", false)] // should not validate an invalid style
        [InlineData(Format.Phone, "+31 42 123 4567", true)] // should validate a valid phone-number
        [InlineData(Format.Phone, "31 42 123 4567", false)] // should not validate an invalid phone-number
        [InlineData(Format.URI, "http://www.bing.com/", true)] // should validate http://www.bing.com/
        [InlineData(Format.URI, "http://www.bing.com/search", true)] // should validate http://www.bing.com/search
        [InlineData(Format.URI, "search", false)] // should not validate relative URIs
        [InlineData(Format.URI, "The dog jumped", false)] // should not validate with whitespace
        [InlineData(Format.Email, "obama@whitehouse.gov", true)] // should validate obama@whitehouse.gov
        [InlineData(Format.Email, "barack+obama@whitehouse.gov", true)] // should validate barack+obama@whitehouse.gov
        [InlineData(Format.Email, "obama@", false)] // should not validate obama@
        [InlineData(Format.IPAddress, "192.168.0.1", true)] // should validate 192.168.0.1
        [InlineData(Format.IPAddress, "127.0.0.1", true)] // should validate 127.0.0.1
        [InlineData(Format.IPAddress, "192.168.0", false)] // should not validate 192.168.0
        [InlineData(Format.IPv6, "fe80::1%lo0", true)] // should validate fe80::1%lo0
        [InlineData(Format.IPv6, "::1", true)] // should validate ::1
        [InlineData(Format.IPv6, "127.0.0.1", false)] // should not validate 127.0.0.1
        [InlineData(Format.IPv6, "localhost", false)] // should not validate localhost
        [InlineData(Format.HostName, "localhost", true)] // should validate localhost
        [InlineData(Format.HostName, "www.bing.com", true)] // should validate www.bing.com
        [InlineData(Format.HostName, "www.-hi-.com", false)] // should not validate www.-hi-.com
        [InlineData(Format.Alpha, "alpha", true)] // should validate alpha
        [InlineData(Format.Alpha, "abracadabra", true)] // should validate abracadabra
        [InlineData(Format.Alpha, "1test", false)] // should validate 1test
        [InlineData(Format.AlphaNumeric, "alpha", true)] // should validate alphanumeric
        [InlineData(Format.AlphaNumeric, "123", true)] // should validate 123
        [InlineData(Format.AlphaNumeric, "abracadabra123", true)] // should validate abracadabra123
        [InlineData(Format.AlphaNumeric, "1test!", false)] // should not validate 1test!
        public void TestFormat(Format format, string value, bool valid)
        {
            var datetime = AppTypes.StringType.Value.Create(new String
            {
                Type = SchemaType.String,

                Value = value,

                Title = "datetime",

                Format = format
            });

            Assert.Equal(SchemaType.String, datetime.Type);

            Assert.Null(datetime.Const);

            Assert.Equal(value, datetime.Value);

            Assert.Null(datetime.MinLength);

            Assert.Null(datetime.MaxLength);

            Assert.Null(datetime.Pattern);

            Assert.Equal("datetime", datetime.Title);

            Assert.Equal(format, datetime.Format);

            datetime.Validate();

            Assert.Equal(valid, datetime.Valid);

            Assert.Equal(datetime.Errors, valid ? Enumerable.Empty<string>() : new string[] { $"should match format {format}" });
        }
    }
}
