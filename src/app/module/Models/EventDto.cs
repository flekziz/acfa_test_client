using System.ComponentModel.DataAnnotations;

namespace app.module.Models
{
    public class EventDto
    {
        [Required]
        public string Uid { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Timestamp must be a non-negative number.")]
        public int Timestamp { get; set; }

        [Required]
        public EventDataDto Data { get; set; }
        public string LocalizedString { get; set; }
    }
}
