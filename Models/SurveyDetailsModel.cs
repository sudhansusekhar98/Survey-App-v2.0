namespace SurveyApp.Models
{
    public class SurveyDetailsModel
    {
        public int TransID { get; set; }
        public Int64 SurveyID { get; set; }
        public int LocID { get; set; }
        public int ItemTypeID { get; set; }
        public int ItemID { get; set; }
        public int ItemQtyExist { get; set; }
        public int ItemQtyReq { get; set; }
        public string ImgPath { get; set; } = string.Empty;
        public string ImgID { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }
    }
}
