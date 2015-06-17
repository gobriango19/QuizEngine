using Newtonsoft.Json;

namespace QuizEngine.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Quiz")]
    public class Quiz : BaseModel
    {
        public Quiz()
        {
            IsActive = false;

            //Questions = new HashSet<Question>();
            //Results = new HashSet<Result>();
        }

        [Display(Name = "ID")]
        public long QuizId { get; set; }

        [Display(Name = "Name")]
        [Required(AllowEmptyStrings=false, ErrorMessage="Name is required")]
        [StringLength(128, ErrorMessage="Name can only be 128 characters long")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [StringLength(512, ErrorMessage="Description can only be 512 characters long")]
        public string Description { get; set; }

        [Display(Name = "Image URL")]
        [DataType(DataType.ImageUrl)]
        [StringLength(256, ErrorMessage="Image URL can only be 256 characters long")]
        public string ImageUrl { get; set; }

        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

        [NotMapped]
        [JsonIgnore]
        public List<Question> Questions { get; set; }

        [NotMapped]
        [JsonIgnore]
        public List<Result> Results { get; set; } 

        /*
        public virtual ICollection<Question> Questions { get; set; }

        public virtual ICollection<Result> Results { get; set; }
        */ 
    }
}
