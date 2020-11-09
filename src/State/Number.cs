using System;
using System.Collections.Generic;

namespace Skclusive.Mobx.JsonSchema
{
    public interface INumberPrimitive
    {
        int? Minimum { set; get; }

        int? Maximum { set; get; }

        int? MultipleOf { set; get; }
    }

    public interface INumber : IValue<double?>, INumberPrimitive
    {
    }

    public class Number : ValueSnapshot<double?>, INumber
    {
        public Number()
        {
            Type = SchemaType.Number;
        }

        public int? Minimum { set; get; }

        public int? Maximum { set; get; }

        public int? MultipleOf { set; get; }
    }
}
