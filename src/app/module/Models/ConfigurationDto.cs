using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace app.module.Models
{
    public class ConfigurationDto
    {
        [Required]
        internal string Uid { get; set; }
        [Required]
        internal string DisplayName { get; set; }
        [Required]
        internal string InternalType { get; set; }
        internal IEnumerable<PropertyDto> Properties { get; set; }
        internal IEnumerable<ConfigurationDto> Configurations { get; set; }
    }
}
