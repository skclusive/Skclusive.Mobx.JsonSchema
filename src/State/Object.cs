using System;
using System.Collections.Generic;
using Skclusive.Core.Collection;

namespace Skclusive.Mobx.JsonSchema
{
    public interface IObjectPrimitive
    {
        int MinProperties { set; get; }

        int MaxProperties { set; get; }
    }

    public interface IObject : IAny, IObjectPrimitive
    {
        string[] Required { set; get; }

        IMap<string, IAny> Properties { set; get; }
    }

    public class Object : Any, IObject
    {
        public IMap<string, IAny> Properties { set; get; }

        public string[] Required { set; get; }

        public int MinProperties { set; get; }

        public int MaxProperties { set; get; }
    }
}
