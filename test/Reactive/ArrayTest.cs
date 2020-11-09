using System.Collections.Generic;
using Skclusive.Core.Collection;
using Xunit;

namespace Skclusive.Mobx.JsonSchema.Tests
{
    public class ArrayTest
    {
        [Fact]
        public void TestCreate()
        {
            var array = AppTypes.ArrayType.Value.Create(new Array
            {
               Type = SchemaType.Array,

               Title = "array?",

               Items = new List<IAny>
               {
                   new Boolean
                   {
                       Type = SchemaType.Boolean, 

                       Const = true,

                       Enum = new bool[] { true, false },

                       Value = true,

                       Title = "agree?"
                   },
                   new String
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
                   },
                   new Number
                   {
                       Type = SchemaType.Number,

                       Const = 10,

                       Enum = new double?[] { 1, 3, 5 },

                       Value = 6,

                       Title = "sequence?",

                       Minimum = 1,

                       Maximum = 8,

                       MultipleOf = 2
                   },
                   new Object
                   {
                       Type = SchemaType.Object,

                       Title = "complex?",

                       Properties = new Map<string, IAny>
                       {
                           ["crazy"] = new Boolean
                           {
                               Type = SchemaType.Boolean,

                               Const = true,

                               Enum = new bool[] { true, false },

                               Value = true,

                               Title = "crazy?"
                           }
                       }
                   }
               }
            });

            var value = array.Value;

            Assert.NotNull(value);

            Assert.Equal(SchemaType.Array, array.Type);

            Assert.Equal("array?", array.Title);

            Assert.Equal(4, array.Items.Count);

            Assert.NotNull(array.Items[0]);

            Assert.True(array.Items[0] is IBooleanObservable);

            Assert.NotNull(array.Items[1]);

            Assert.True(array.Items[1] is IStringObservable);

            Assert.NotNull(array.Items[2]);

            Assert.True(array.Items[2] is INumberObservable);

            Assert.NotNull(array.Items[3]);

            Assert.True(array.Items[3] is IObjectObservable);

            Assert.Equal(1, (array.Items[3] as IObjectObservable).Properties.Count);
        }
    }
}
