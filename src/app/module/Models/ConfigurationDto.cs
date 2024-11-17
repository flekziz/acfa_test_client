using System.Collections.Generic;

namespace app.module.Models
{
    public class ConfigurationDto
    {
        internal string Uid { get; set; }
        internal string DisplayName { get; set; }
        internal string InternalType { get; set; }
        internal IEnumerable<PropertyDto> Properties { get; set; }
        internal IEnumerable<ConfigurationDto> Configurations { get; set; }
    }
}
