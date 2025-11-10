using AnalyticaDocs.Util;
using Microsoft.Data.SqlClient;
using SurveyApp.Models;
using System.Data;

namespace SurveyApp.Repo
{
    public class SurveyRepo : ISurvey
    {
        public bool AddSurvey(SurveyModel survey)
        {
            try
            {
                using var con = new SqlConnection(DBConnection.ConnectionString);
                using var cmd = new SqlCommand("dbo.SpSurvey", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // SpType = 1 -> Insert Survey 
                cmd.Parameters.AddWithValue("@SpType", 1);

                // The SP generates SurveyId internally. Passing SurveyId is optional
                // but we'll pass null to keep it explicit.
                cmd.Parameters.AddWithValue("@SurveyID", survey.SurveyId == 0 ? (object)DBNull.Value : survey.SurveyId);

                cmd.Parameters.AddWithValue("@SurveyName", survey.SurveyName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ImplementationType", survey.ImplementationType ?? (object)DBNull.Value);

                // Your SP expects SurveyDate as varchar(100) — keep same format or adjust SP to accept DATE.
                // If your model uses DateTime? convert to string (yyyy-MM-dd) or pass as DBNull if null.
                if (survey.SurveyDate == null)
                    cmd.Parameters.AddWithValue("@SurveyDate", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@SurveyDate", survey.SurveyDate);

                cmd.Parameters.AddWithValue("@SurveyTeamName", survey.SurveyTeamName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SurveyTeamContact", survey.SurveyTeamContact ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AgencyName", survey.AgencyName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@LocationSiteName", survey.LocationSiteName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CityDistrict", survey.CityDistrict ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ZoneSectorWardNumber", survey.ZoneSectorWardNumber ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ScopeOfWork", survey.ScopeOfWork ?? (object)DBNull.Value);

                cmd.Parameters.AddWithValue("@Latitude", survey.Latitude ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Longitude", survey.Longitude ?? (object)DBNull.Value);

                cmd.Parameters.AddWithValue("@MapMarking", survey.MapMarking ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SurveyStatus", survey.SurveyStatus ?? (object)DBNull.Value);

                
                if (survey.CreatedBy == 0)
                    cmd.Parameters.AddWithValue("@CreatedBy", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@CreatedBy", survey.CreatedBy);

                con.Open();

                int rowsAffected = cmd.ExecuteNonQuery();

                // NOTE: If you later modify the stored procedure to output the generated SurveyId
                // you can add an output parameter and read it here:
                // var newId = cmd.Parameters["@NewSurveyId"].Value;

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                // TODO: log ex.ToString()
                throw;
            }
        }

        public bool UpdateSurvey(SurveyModel survey)
        {
            try
            {
                using var con = new SqlConnection(DBConnection.ConnectionString);
                using var cmd = new SqlCommand(@"
                    UPDATE dbo.Survey 
                    SET SurveyName = @SurveyName,
                        ImplementationType = @ImplementationType,
                        SurveyDate = @SurveyDate,
                        SurveyTeamName = @SurveyTeamName,
                        SurveyTeamContact = @SurveyTeamContact,
                        AgencyName = @AgencyName,
                        LocationSiteName = @LocationSiteName,
                        CityDistrict = @CityDistrict,
                        ZoneSectorWardNumber = @ZoneSectorWardNumber,
                        ScopeOfWork = @ScopeOfWork,
                        Latitude = @Latitude,
                        Longitude = @Longitude,
                        MapMarking = @MapMarking,
                        SurveyStatus = @SurveyStatus,
                        CreatedBy = @CreatedBy
                    WHERE SurveyId = @SurveyId", con);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@SurveyId", survey.SurveyId);
                cmd.Parameters.AddWithValue("@SurveyName", survey.SurveyName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ImplementationType", survey.ImplementationType ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SurveyDate", survey.SurveyDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SurveyTeamName", survey.SurveyTeamName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SurveyTeamContact", survey.SurveyTeamContact ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AgencyName", survey.AgencyName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@LocationSiteName", survey.LocationSiteName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CityDistrict", survey.CityDistrict ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ZoneSectorWardNumber", survey.ZoneSectorWardNumber ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ScopeOfWork", survey.ScopeOfWork ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Latitude", survey.Latitude ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Longitude", survey.Longitude ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MapMarking", survey.MapMarking ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SurveyStatus", survey.SurveyStatus ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedBy", survey.CreatedBy);

                con.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                // log ex.ToString()
                throw;
            }
        }

        public List<SurveyModel> GetAllSurveys()
        {
            try
            {
                using var con = new SqlConnection(DBConnection.ConnectionString);
                using var cmd = new SqlCommand(@"
                    SELECT SurveyId, SurveyName, ImplementationType, SurveyDate,
                           SurveyTeamName, SurveyTeamContact, AgencyName, LocationSiteName,
                           CityDistrict, ZoneSectorWardNumber, ScopeOfWork, Latitude, Longitude,
                           MapMarking, SurveyStatus, CreatedBy
                    FROM dbo.Survey
                    ORDER BY SurveyDate DESC", con);

                con.Open();
                using var adapter = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                List<SurveyModel> records = SqlDbHelper.DataTableToList<SurveyModel>(dt);
                return records;
            }
            catch (Exception ex)
            {
                // log ex.ToString()
                throw;
            }
        }

        public SurveyModel? GetSurveyById(Int64 surveyId)
        {
            try
            {
                using var con = new SqlConnection(DBConnection.ConnectionString);
                using var cmd = new SqlCommand(@"
                    SELECT SurveyId, SurveyName, ImplementationType, SurveyDate,
                           SurveyTeamName, SurveyTeamContact, AgencyName, LocationSiteName,
                           CityDistrict, ZoneSectorWardNumber, ScopeOfWork, Latitude, Longitude,
                           MapMarking, SurveyStatus, CreatedBy
                    FROM dbo.Survey
                    WHERE SurveyId = @SurveyId", con);

                cmd.Parameters.AddWithValue("@SurveyId", surveyId);

                con.Open();
                using var adapter = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                List<SurveyModel> surveys = SqlDbHelper.DataTableToList<SurveyModel>(dt);
                return surveys.FirstOrDefault();
            }
            catch (Exception ex)
            {
                // log ex.ToString()
                throw;
            }
        }

        public bool DeleteSurvey(Int64 surveyId)
        {
            try
            {
                using var con = new SqlConnection(DBConnection.ConnectionString);
                using var cmd = new SqlCommand(@"
                    DELETE FROM dbo.Survey 
                    WHERE SurveyId = @SurveyId", con);

                cmd.Parameters.AddWithValue("@SurveyId", surveyId);

                con.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                // log ex.ToString()
                throw;
            }
        }
    }
}
