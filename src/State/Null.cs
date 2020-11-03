using System;
using System.Collections.Generic;

namespace Skclusive.Mobx.JsonSchema
{
    public interface INullPrimitive
    {
    }

    public interface INull : IValue<object>, INullPrimitive
    {
    }

    public class Null : ValueSnapshot<object>, INull
    {
    }
}
