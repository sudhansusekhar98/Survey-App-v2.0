using System.ComponentModel.DataAnnotations;

namespace SurveyApp.Models
{
    public class ItemTypeMasterModel
    {
        public int id { get; set; }
        
        [Required]
        [Display(Name = "Module Name")]
        public string TypeName { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Description")]
        public string TypeDesc { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public char IsActive { get; set; } = 'Y';
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
