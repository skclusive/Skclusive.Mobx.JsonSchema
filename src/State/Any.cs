using System;

namespace Skclusive.Mobx.JsonSchema
{
    public interface IAnyPrimitive : IMetaPrimitive
    {
        SchemaType Type { set; get; }

        string Title { set; get; }

        bool Validating { set; get; }

        bool Syncing { set; get; }
    }

    public interface IAny : IAnyPrimitive, IMeta
    {
        string[] Errors { set; get; }
    }

    public class Any : Meta, IAny
    {
        public SchemaType Type { set; get; }

        public string Title { set; get; }

        public bool Validating { set; get; }

        public bool Syncing { set; get; }

        public string[] Errors { set; get; }
    }
}
