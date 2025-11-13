using System.ComponentModel.DataAnnotations;

namespace SurveyApp.Models
{
    /// <summary>
    /// ViewModel for displaying selected ItemTypes for a location in accordion format
    /// </summary>
    public class LocationItemTypeViewModel
    {
        public int LocId { get; set; }
        public Int64 SurveyId { get; set; }
        public string SurveyName { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public List<ItemTypeMasterModel> SelectedItemTypes { get; set; } = new List<ItemTypeMasterModel>();
    }
}
