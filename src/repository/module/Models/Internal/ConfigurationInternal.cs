using System.Collections.Generic;

namespace repository.module.Models.Internal
{
    internal class ConfigurationInternal
    {
        public string Uid { get; set; }
        public string DisplayName { get; set; }
        public string InternalType { get; set; }
        public ICollection<ConfigurationInternal> Configurations { get; set; }
        public ICollection<PropertyInternal> Properties { get; set; }

        public string? ParentUid { get; set; }
        public ConfigurationInternal Parent { get; set; }
    }
}
