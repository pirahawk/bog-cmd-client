using System;

namespace Bog.Cmd.Domain.Models
{
    public class BlobApiSettings
    {
        public string Host { get; set; }
        public string Scheme { get; set; }
        public Guid Api { get; set; }
    }
}