using System.Collections.Generic;

namespace app.module.Models
{
    public class PropertDto
    {
        internal string Id { get; set; }
        internal string? ValueString { get; set; }
        internal IEnumerable<PropertDto>? Properties { get; set; }
    }
}
