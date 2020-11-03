using Xunit;

namespace Skclusive.Mobx.JsonSchema.Tests
{
    public class BooleanTest
    {
        [Fact]
        public void TestCreate()
        {
            var agree = AppTypes.BooleanType.Value.Create(new Boolean
            {
               Type = SchemaType.Boolean,

               Const = true,

               Enum = new bool[] { true, false },

               Value = true,

               Title = "agree?"
            });

            Assert.Equal(SchemaType.Boolean, agree.Type);

            Assert.True(agree.Const);

            Assert.True(agree.Value);

            Assert.Equal(2, agree.Enum.Count);

            Assert.True(agree.Enum[0]);

            Assert.False(agree.Enum[1]);

            Assert.Equal("agree?", agree.Title);
        }
    }
}
