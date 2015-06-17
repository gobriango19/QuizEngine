using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace QuizEngine.Models
{
    public abstract class BaseModel
    {
        //[JsonIgnore]
        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:F}")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Last Updated Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:F}")]
        public DateTime? UpdateDate { get; set; }
    }
}
