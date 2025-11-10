using AnalyticaDocs.Models;
using AnalyticaDocs.Util;
using Humanizer;
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
                using var cmd = new SqlCommand("dbo.SpSurvey", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", 8);
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
                using var cmd = new SqlCommand("dbo.SpSurvey", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", 2);

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
                using var cmd = new SqlCommand("dbo.SpSurvey", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", 7);
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

        public List<SurveyLocationModel> GetSurveyLocationById(Int64 surveyId)
        {
            var locations = new List<SurveyLocationModel>();
            using (var conn = new SqlConnection(DBConnection.ConnectionString))
            using (var cmd = new SqlCommand("dbo.SpSurvey", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", 9);
                cmd.Parameters.AddWithValue("@SurveyID", surveyId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    locations = SqlDbHelper.DataTableToList<SurveyLocationModel>(dt);
                }
            }
            return locations;
        }

        public SurveyLocationModel? GetSurveyLocationByLocId(int locId)
        {
            try
            {
                using var conn = new SqlConnection(DBConnection.ConnectionString);
                using var cmd = new SqlCommand("dbo.SpSurvey", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", 10); // Assuming SpType 10 for getting location by LocID
                cmd.Parameters.AddWithValue("@LocID", locId);
                
                conn.Open();
                using var reader = cmd.ExecuteReader();
                var dt = new DataTable();
                dt.Load(reader);
                var locations = SqlDbHelper.DataTableToList<SurveyLocationModel>(dt);
                return locations.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool AddSurveyLocation(SurveyLocationModel location)
        {
            try
            {
                using var conn = new SqlConnection(DBConnection.ConnectionString);
                using var cmd = new SqlCommand("dbo.SpSurvey", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", 5);
                cmd.Parameters.AddWithValue("@SurveyID", location.SurveyID);
                cmd.Parameters.AddWithValue("@LocName", location.LocName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@LocLat", location.LocLat ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@LocLog", location.LocLog ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedBy", location.CreateBy ?? (object)DBNull.Value);
                
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool UpdateSurveyLocation(SurveyLocationModel location)
        {
            try
            {
                using var conn = new SqlConnection(DBConnection.ConnectionString);
                using var cmd = new SqlCommand("dbo.SpSurvey", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", 12); 
                cmd.Parameters.AddWithValue("@LocID", location.LocID);
                cmd.Parameters.AddWithValue("@SurveyID", location.SurveyID);
                cmd.Parameters.AddWithValue("@LocName", location.LocName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@LocLat", location.LocLat ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@LocLog", location.LocLog ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Isactive", location.Isactive);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool DeleteSurveyLocation(int locId)
        {
            try
            {
                using var conn = new SqlConnection(DBConnection.ConnectionString);
                using var cmd = new SqlCommand("dbo.SpSurvey", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", 11); 
                cmd.Parameters.AddWithValue("@LocID", locId);
                
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool CreateLocationsBySurveyId(Int64 surveyId, List<SurveyLocationModel> locations, int createdBy)
        {
            using var conn = new SqlConnection(DBConnection.ConnectionString);
            conn.Open();
            foreach (var location in locations)
            {
                using var cmd = new SqlCommand("dbo.SpSurvey", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", 5);
                cmd.Parameters.AddWithValue("@SurveyID", surveyId);
                cmd.Parameters.AddWithValue("@LocName", location.LocName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@LocLat", location.LocLat ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@LocLog", location.LocLog ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                cmd.ExecuteNonQuery();
            }
            return true;
        }

    }
}
