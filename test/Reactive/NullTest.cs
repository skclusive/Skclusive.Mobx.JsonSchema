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

               Value = null,

               Title = "nothing?"
            });

            Assert.Equal(SchemaType.Null, nothing.Type);

            Assert.Null(nothing.Value);

            Assert.Equal("nothing?", nothing.Title);
        }
    }
}
