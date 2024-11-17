using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace app.module.Models
{
    public class PropertyDto
    {
        [Required]
        internal string Id { get; set; }
        internal string? ValueString { get; set; }
        internal IEnumerable<PropertyDto>? Properties { get; set; }
    }
}
