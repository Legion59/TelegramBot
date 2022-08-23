using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelegramBotApp.Model
{
    public class LocationMessageModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MessageId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ChatId { get; set; }

        [Required]
        [MaxLength(256)]
        [MinLength(1)]
        public string MessageText { get; set; }

        public DateTime MessageTime { get; set; }
    }
}
