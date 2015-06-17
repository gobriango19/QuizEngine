namespace QuizEngine.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Result")]
    public class Result : BaseModel
    {
        [Display(Name = "ID")]
        public long ResultId { get; set; }

        [Display(Name = "Name")]
        [Required(AllowEmptyStrings=false, ErrorMessage="Name is required")]
        [StringLength(128, ErrorMessage="Name can only be 128 characters long")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [StringLength(512, ErrorMessage="Description can only be 512 characters long")]
        public string Description { get; set; }

        [Display(Name = "Image URL")]
        [StringLength(256, ErrorMessage="Image URL can only be 256 characters long")]
        public string ImageUrl { get; set; }

        [Display(Name = "Min Score")]
        [Required(ErrorMessage="Minimum score is required")]
        public int MinScore { get; set; }

        [Display(Name = "Max Score")]
        [Required(ErrorMessage="Maximum score is required")]
        public int MaxScore { get; set; }

        [Required(ErrorMessage="A quiz ID for reference is required")]
        [Range(typeof(long), "1", "9223372036854775807", ErrorMessage = "A quiz ID for reference is required")]
        public long QuizId { get; set; }

        [JsonIgnore]
        public virtual Quiz Quiz { get; set; }
    }
}
