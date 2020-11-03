using System;
using System.Collections.Generic;
using Skclusive.Core.Collection;

namespace Skclusive.Mobx.JsonSchema
{
    public interface IArrayPrimitive
    {
        int MinItems { set; get; }

        int MaxItems { set; get; }

        bool UniqueItems { set; get; }

        bool AdditionalItems { set; get; }
    }

    public interface IArray : IAny, IArrayPrimitive
    {
        IList<IAny> Items { set; get; }
    }

    public class Array : Any, IArray
    {
        public IList<IAny> Items { set; get; }

        public int MinItems { set; get; }

        public int MaxItems { set; get; }

        public bool UniqueItems { set; get; }

        public bool AdditionalItems { set; get; }
    }
}
