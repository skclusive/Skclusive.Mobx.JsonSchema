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

               Const = "const",

               Enum = new string[] { "one", "two", "three" },

               Value = "six",

               Title = "textual?",

               MinLength = 1,

               MaxLength = 8,

               Pattern = "pattern",

               Format = Format.DateTime
            });

            Assert.Equal(SchemaType.String, textual.Type);

            Assert.Equal("const", textual.Const);

            Assert.Equal("six", textual.Value);

            Assert.Equal(3, textual.Enum.Count);

            Assert.Equal("one", textual.Enum[0]);

            Assert.Equal("two", textual.Enum[1]);

            Assert.Equal("three", textual.Enum[2]);

            Assert.Equal(1, textual.MinLength);

            Assert.Equal(8, textual.MaxLength);

            Assert.Equal("pattern", textual.Pattern);

            Assert.Equal("sequence?", textual.Title);

            Assert.Equal(Format.DateTime, textual.Format);
        }
    }
}
