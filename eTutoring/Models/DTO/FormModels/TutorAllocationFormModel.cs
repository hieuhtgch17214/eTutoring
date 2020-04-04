using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eTutoring.Models.DTO.FormModels
{
    public class TutorAllocationFormModel
    {
        [Required]
        public string TutorId { get; set; }

        [Required]
        public string[] StudentIds { get; set; }
    }
}