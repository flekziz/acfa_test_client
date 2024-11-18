using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace app.module.Models
{
    public class PropertyDto
    {
        [Required]
        public string Id { get; set; }
        public string? ValueString { get; set; }
        public IEnumerable<PropertyDto>? Properties { get; set; }
    }
}
