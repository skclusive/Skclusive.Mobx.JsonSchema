using System;
using System.Collections.Generic;

namespace Skclusive.Mobx.JsonSchema
{
    public interface IBooleanPrimitive
    {
    }

    public interface IBoolean : IValue<bool>, IBooleanPrimitive
    {
    }

    public class Boolean : ValueSnapshot<bool>, IBoolean
    {
        public Boolean()
        {
            Type = SchemaType.Boolean;
        }
    }
}
