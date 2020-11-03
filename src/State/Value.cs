using System;

namespace Skclusive.Mobx.JsonSchema
{
    public interface IValuePrimitive<V>
    {
        V Const { set; get; }

        V Value { set; get; }
    }

    public interface IValue<V> : IAny, IValuePrimitive<V>
    {
        V[] Enum { set; get; }
    }

    public class ValueSnapshot<V> : Any, IValue<V>
    {
        public V[] Enum { set; get; }

        public V Const { set; get; }

        public V Value { set; get; }
    }
}
