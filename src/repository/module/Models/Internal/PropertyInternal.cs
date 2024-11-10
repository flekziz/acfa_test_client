using System.Collections.Generic;

namespace repository.module.Models.Internal
{
    internal class PropertyInternal
    {
        public string Name { get; set; }
        public string? ValueString { get; set; }
        public ICollection<PropertyInternal>? Properties { get; set; }
    }
}
