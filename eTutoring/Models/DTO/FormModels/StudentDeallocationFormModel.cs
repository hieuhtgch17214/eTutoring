using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eTutoring.Models.DTO.FormModels
{
    public class StudentDeallocationFormModel
    {
        [Required]
        public string[] StudentIds { get; set; }
    }
}