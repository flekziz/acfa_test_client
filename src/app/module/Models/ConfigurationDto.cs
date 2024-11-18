using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace app.module.Models
{
    public class ConfigurationDto
    {
        [Required]
        public string Uid { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string InternalType { get; set; }
        public IEnumerable<PropertyDto> Properties { get; set; }
        public IEnumerable<ConfigurationDto> Configurations { get; set; }
    }
}
