using Skclusive.Core.Collection;
using Xunit;

namespace Skclusive.Mobx.JsonSchema.Tests
{
    public class ObjectTest
    {
        [Fact]
        public void TestCreate()
        {
            var nothing = AppTypes.ObjectType.Value.Create(new Object
            {
               Type = SchemaType.Object,

               Title = "nothing?",

               Properties = new Map<string, IAny>
               {
                   ["agree"] = new Boolean
                   {
                       Type = SchemaType.Boolean, 

                       Const = true,

                       Enum = new bool[] { true, false },

                       Value = true,

                       Title = "agree?"
                   },
                   ["textual"] = new String
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
                   ["sequence"] = new Number
                   {
                       Type = SchemaType.Number,

                       Const = 10,

                       Enum = new double[] { 1, 3, 5 },

                       Value = 6,

                       Title = "sequence?",

                       Minimum = 1,

                       Maximum = 8,

                       MultipleOf = 2
                   },
                   ["complex"] = new Object
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

            var value = nothing.Value;

            Assert.NotNull(value);

            Assert.Equal(SchemaType.Object, nothing.Type);

            Assert.Equal("nothing?", nothing.Title);

            Assert.Equal(4, nothing.Properties.Count);

            Assert.NotNull(nothing.Properties["agree"]);

            Assert.True(nothing.Properties["agree"] is IBooleanObservable);

            Assert.NotNull(nothing.Properties["textual"]);

            Assert.True(nothing.Properties["textual"] is IStringObservable);

            Assert.NotNull(nothing.Properties["sequence"]);

            Assert.True(nothing.Properties["sequence"] is INumberObservable);

            Assert.NotNull(nothing.Properties["complex"]);

            Assert.True(nothing.Properties["complex"] is IObjectObservable);

            Assert.Equal(1, (nothing.Properties["complex"] as IObjectObservable).Properties.Count);
        }
    }
}
