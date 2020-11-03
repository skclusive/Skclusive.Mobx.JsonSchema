using System;

namespace Skclusive.Mobx.JsonSchema
{
    public interface IMetaPrimitive
    {
        string Error { set; get; }
    }

    public interface IMeta : IMetaPrimitive
    {
    }

    public class Meta : IMeta
    {
        public string Error { set; get; }
    }
}
