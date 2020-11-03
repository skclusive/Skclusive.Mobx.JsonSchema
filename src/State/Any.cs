using System;

namespace Skclusive.Mobx.JsonSchema
{
    public interface IAny : IMeta
    {
        SchemaType Type { set; get; }

        string Title { set; get; }
    }

    public class Any : Meta, IAny
    {
        public SchemaType Type { set; get; }

        public string Title { set; get; }
    }
}
