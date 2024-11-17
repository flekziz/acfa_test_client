using System.ComponentModel.DataAnnotations;

namespace app.module.Models
{
    public class EventDto
    {
        [Required]
        internal string Uid { get; set; }

        [Required]
        internal string DisplayName { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Timestamp must be a non-negative number.")]
        internal int Timestamp { get; set; }

        [Required]
        internal EventDataDto Data { get; set; }
        internal string LocalizedString { get; set; }
    }
}
