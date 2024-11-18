using repository.module.Models;

namespace app.module.Models
{
    public class EventDataDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public EventDataType Type { get; set; }
    }
}
