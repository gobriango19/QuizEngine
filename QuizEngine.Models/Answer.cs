namespace QuizEngine.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Answer")]
    public class Answer : BaseModel
    {
        [Display(Name = "ID")]
        public long AnswerId { get; set; }

        [Column("Answer")]
        [Display(Name = "Answer")]
        [Required(AllowEmptyStrings=false, ErrorMessage="Answer text is required")]
        [StringLength(512, ErrorMessage="Answer text can only be 512 characters long")]
        public string AnswerText { get; set; }

        [Display(Name = "Image URL")]
        [StringLength(256, ErrorMessage="Image URL can only be 256 characters long")]
        public string ImageUrl { get; set; }

        [Display(Name = "Score")]
        [Required(ErrorMessage="Score is required")]
        public int Score { get; set; }

        [Display(Name = "Sequence")]
        [Required(ErrorMessage="Sequence is required")]
        public short Sequence { get; set; }

        [Required(ErrorMessage="A question ID for reference is required")]
        [Range(typeof(long), "1", "9223372036854775807", ErrorMessage = "A question ID for reference is required")]
        public long QuestionId { get; set; }

        [JsonIgnore]
        public virtual Question Question { get; set; }
    }
}
