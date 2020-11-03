using Xunit;

namespace Skclusive.Mobx.JsonSchema.Tests
{
    public class NumberTest
    {
        [Fact]
        public void TestCreate()
        {
            var sequence = AppTypes.NumberType.Value.Create(new Number
            {
               Type = SchemaType.Number,

               Const = 10,

               Enum = new double[] { 1, 3, 5 },

               Value = 6,

               Title = "sequence?",

               Minimum = 1,

               Maximum = 8,

               MultipleOf = 2
            });

            Assert.Equal(SchemaType.Number, sequence.Type);

            Assert.Equal(10, sequence.Const);

            Assert.Equal(6, sequence.Value);

            Assert.Equal(3, sequence.Enum.Count);

            Assert.Equal(1, sequence.Enum[0]);

            Assert.Equal(3, sequence.Enum[1]);

            Assert.Equal(5, sequence.Enum[2]);

            Assert.Equal(1, sequence.Minimum);

            Assert.Equal(8, sequence.Maximum);

            Assert.Equal(2, sequence.MultipleOf);

            Assert.Equal("sequence?", sequence.Title);
        }
    }
}
