using SurveyApp.Models;
using System.Security.Cryptography.Xml;

namespace SurveyApp.Repo
{
    public interface ISurvey
    {
        List<SurveyModel> GetAllSurveys();
        SurveyModel? GetSurveyById(Int64 surveyId);
        bool AddSurvey(SurveyModel survey);
        bool UpdateSurvey(SurveyModel survey);
        bool DeleteSurvey(Int64 surveyId);
        List<SurveyLocationModel> GetSurveyLocationById(Int64 surveyId);
        SurveyLocationModel? GetSurveyLocationByLocId(int locId);
        bool AddSurveyLocation(SurveyLocationModel location);
        bool UpdateSurveyLocation(SurveyLocationModel location);
        bool DeleteSurveyLocation(int locId);
        bool CreateLocationsBySurveyId(Int64 surveyId, List<SurveyLocationModel> locations, int createdBy);
        List<ItemTypeMasterModel> GetItemTypeMaster(int locId);
        bool SaveItemTypesForLocation(Int64 surveyId, string surveyName, int locId, List<int> itemTypeIds);
    }
}

