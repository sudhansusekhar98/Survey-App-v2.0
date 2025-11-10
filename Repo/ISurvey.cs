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
    }
}
