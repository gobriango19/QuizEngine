namespace QuizEngine.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Question")]
    public class Question : BaseModel
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        [Display(Name = "ID")]
        public long QuestionId { get; set; }

        [Column("Question")]
        [Display(Name = "Question")]
        [Required(AllowEmptyStrings=false, ErrorMessage="Question text is required")]
        [StringLength(512, ErrorMessage="Questions text can only be 512 characters long")]
        public string QuestionText { get; set; }

        [Display(Name = "Image URL")]
        [DataType(DataType.ImageUrl)]
        [StringLength(256, ErrorMessage="Image URL can only be 256 characters long")]
        public string ImageUrl { get; set; }

        [Display(Name = "Sequence")]
        [Required(ErrorMessage="Sequence is required")]
        public short Sequence { get; set; }

        [Required(ErrorMessage="A quiz ID for reference is required")]
        [Range(typeof(long), "1", "9223372036854775807", ErrorMessage = "A quiz ID for reference is required")]
        public long QuizId { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        [JsonIgnore]
        public virtual Quiz Quiz { get; set; }
    }
}
