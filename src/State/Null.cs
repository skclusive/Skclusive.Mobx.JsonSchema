using System;
using System.Collections.Generic;

namespace Skclusive.Mobx.JsonSchema
{
    public interface INullPrimitive : IAnyPrimitive
    {
        object Value { set; get; }
    }

    public interface INull : IAny, INullPrimitive
    {
    }

    public class Null : Any, INull
    {
        public Null()
        {
            Type = SchemaType.Null;
        }

        public object Value { set; get; }
    }
}
