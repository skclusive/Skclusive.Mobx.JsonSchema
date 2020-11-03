using Xunit;

namespace Skclusive.Mobx.JsonSchema.Tests
{
    public class NullTest
    {
        [Fact]
        public void TestCreate()
        {
            var nothing = AppTypes.NullType.Value.Create(new Null
            {
               Type = SchemaType.Null,

               Const = null,

               Value = null,

               Title = "nothing?"
            });

            Assert.Equal(SchemaType.Null, nothing.Type);

            Assert.Null(nothing.Const);

            Assert.Null(nothing.Value);

            Assert.Null(nothing.Enum);

            Assert.Equal("nothing?", nothing.Title);
        }
    }
}
