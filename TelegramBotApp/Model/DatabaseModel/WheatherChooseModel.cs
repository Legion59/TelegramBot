using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelegramBotApp.Model.DatabaseModel
{
    public class WheatherChooseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long UserId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MessageId { get; set; }

        [Required]
        [MaxLength(256)]
        [MinLength(1)]
        public string ChooseMessageText { get; set; }

        public DateTime MessageTime { get; set; }
    }
}
