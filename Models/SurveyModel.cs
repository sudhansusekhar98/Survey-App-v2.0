using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SurveyApp.Models
{
    public class SurveyModel
    {
        public Int64 SurveyId { get; set; }

        [Required]
        [Display(Name = "Survey Name")]
        public string SurveyName { get; set; } = string.Empty;

        [Display(Name = "Implementation Type")]
        public string? ImplementationType { get; set; }

        [Display(Name = "Survey Date")]
        public DateTime? SurveyDate { get; set; }

        [Display(Name = "Survey Team Name")]
        public string? SurveyTeamName { get; set; }

        [Display(Name = "Survey Team Contact")]
        public string? SurveyTeamContact { get; set; }

        [Display(Name = "Agency Name")]
        public string? AgencyName { get; set; }

        [Display(Name = "Location Site Name")]
        public string? LocationSiteName { get; set; }

        [Display(Name = "City/District")]
        public string? CityDistrict { get; set; }

        [Display(Name = "Zone/Sector/Ward Number")]
        public string? ZoneSectorWardNumber { get; set; }

        [Display(Name = "Scope of Work")]
        public string? ScopeOfWork { get; set; }

        [Display(Name = "Latitude")]
        public decimal? Latitude { get; set; }

        [Display(Name = "Longitude")]
        public decimal? Longitude { get; set; }

        [Display(Name = "Map Marking")]
        public string? MapMarking { get; set; }

        [Display(Name = "Survey Status")]
        public string? SurveyStatus { get; set; }

        public int CreatedBy { get; set; }

        // Options for dropdowns
        public List<SelectListItem> StatusOptions { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Text = "Pending", Value = "Pending" },
            new SelectListItem { Text = "In Progress", Value = "In Progress" },
            new SelectListItem { Text = "Completed", Value = "Completed" },
            new SelectListItem { Text = "On Hold", Value = "On Hold" }
        };

        public List<SelectListItem> ImplementationTypeOptions { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Text = "New Installation", Value = "New Installation" },
            new SelectListItem { Text = "Upgrade", Value = "Upgrade" },
            new SelectListItem { Text = "City Survillance", Value = "City Survillance" },
            new SelectListItem { Text = "Replacement", Value = "Replacement"}
        };
    } 
}
