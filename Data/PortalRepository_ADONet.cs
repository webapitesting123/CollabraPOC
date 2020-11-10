using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using FabrikamResidences_Activities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FabrikamResidences_Activities.Data
{

#if (NOT_MODULE3_DEMO1)
 #else                                                    // 1) uncomment line during Demo 1 of Module 3

    public class PortalRepository_ADONet : IPortalRepository
    {

        private SqlConnection _connection;               // 2) uncomment line 
        private ILogger _logger;

        private const string PortalActivityTableName = "PortalActivity";
        private const string AttendeeTableName = "Attendee";

        public PortalRepository_ADONet(IConfiguration config, ILogger<PortalRepository_ADONet> logger)
        {            
            // 4) Retrieve and Configure the connection string
            //  - Retrieve connection string from Azure SQL Database blade in the Azure Portal
            //  - Replace the User ID and Password values 
            //
            // 4a) For local development use dotnet's Secret Manager to store this value
            //  - Verify that the .csproj file contains a line for UserSecretId
            //  - run the following command  
            //       dotnet user-secrets set ConnectionStrings:AzureDB "<connection string with replaced values>"
            //
            // 4b) for deploying to Azure App Service
            //  - Add the connection string in the App Service's Application Settings / Connection strings
            //  - Naming the connection "AzureDB"

            // Note: Using the above approach allows the following GetConnectionString to work in either environment
            _connection = new SqlConnection(config.GetConnectionString("AzureDB")); //3) uncomment

            _logger = logger;
        }

        #region Manage Database Schema

        public void InitializeDatabase()
        {

             _connection.Open();                          // 5) uncomment all #5


// #if (DEBUG)                                              // 9) uncomment all #9
//             ClearDatabase();                             // 9) then review this method
// #endif                                                   // 9)

             if (!IsSchemaReady())                        // 7) uncomment all #7 lines and review this method 
             {                                            // 7)

                 CreateSchema();                          // 6) uncomment and review this method 

// #if (DEBUG)                                              // 8) uncomment all #8
//                 SeedActivity();                          // 8) then review this method
// #endif                                                   // 8)

             }                                            // 7) 


             _connection.Close();                         // 5) uncomment
        }
        
        private void CreateSchema()
        {
            ExecuteNonQuery($@"

                CREATE TABLE [dbo].[{PortalActivityTableName}] (
                    [Id]            INT IDENTITY(1,1) NOT NULL,
                    [Name]          VARCHAR(50) NOT NULL,
                    [Description]   VARCHAR(MAX) NOT NULL,
                    [Date]          DATETIME
                );

                CREATE TABLE [dbo].[{AttendeeTableName}] (
                    [Id]                INT IDENTITY(1,1) NOT NULL,
                    [PortalActivityId]  INT NOT NULL,
                    [FirstName]         VARCHAR(50) NULL,
                    [LastName]          VARCHAR(50) NULL,
                    [Email]             VARCHAR(50) NULL
                );
            ");

        }

        private bool IsSchemaReady()
        {
            try
            {
                var activitySchemaDT = GetDataTable($"SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{PortalActivityTableName}'");
                return activitySchemaDT.Rows.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        private void SeedActivity()
        {
            DateTime now = DateTime.Now;

            ExecuteNonQuery($@"
                INSERT INTO {PortalActivityTableName} (Name, Description, Date)
                VALUES
                ('Bingo',
                    'Come join us for an exciting game of Bingo with great prizes.',
                    '{now.AddDays(2).ToString("MM/dd/yyyy h:mm tt")}' ),
                ('Shuffleboard Competition',
                        'Meet us at the Shuffleboard court!',
                        '{now.AddDays(5).ToString("MM/dd/yyyy h:mm tt")}' );

                INSERT INTO {AttendeeTableName} (PortalActivityId, FirstName, LastName, Email)
                VALUES
                (1, 'Joe', 'Bingo', 'Joe@Addict.com' ),
                (1, 'john','Doe', 'jdoe@anonymous.com' ),
                (2, 'john', 'Doe', 'jdoe@anonymous.com' ),
                (2, 'Jill', 'Hill', 'champ@shuffleboard.com' );

            ");

        }        
        
        private void ClearDatabase()
        {
            ExecuteNonQuery($"DROP TABLE IF EXISTS {PortalActivityTableName};");
            ExecuteNonQuery($"DROP TABLE IF EXISTS {AttendeeTableName};");
        }

        #endregion

        #region Database Helpers

        private void ExecuteNonQuery(string sql)
        {
            bool closeConn = false;

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                closeConn = true;
                _connection.Open();
            }

            try
            {
                using (var command = new SqlCommand(sql, _connection))
                {
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error executing NonQuery: '{sql}'");
            }
            finally
            {
                if (closeConn)
                {
                    _connection.Close();
                }
            }

        }
        private DataTable GetDataTable(String sql)
        {

            bool closeConn = false;

            if (_connection.State != System.Data.ConnectionState.Open)
            {
                closeConn = true;
                _connection.Open();
            }

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, _connection);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetDataTable error for query: '{sql}'");
                return null;
            }
            finally
            {
                if (closeConn)
                {
                    _connection.Close();
                }
            }
        }

        #endregion

        #region IPortalRepository Members

        public PortalActivity GetActivity(int id)
        {
            DataTable dt = GetDataTable($@"Select Id, Name, Description, Date from {PortalActivityTableName}
                                            WHERE Id = {id}");

            PortalActivity activity = null;
            if (dt.Rows.Count > 0)
            {
                activity = PortalActivityFromDataRow(dt.Rows[0]);
                activity.Attendees = GetActivityAttendees(activity.Id).ToList();

            }

            return activity;

        }

        public IQueryable<PortalActivity> GetActivities()
        {
            List<PortalActivity> activities = new List<PortalActivity>();
            DataTable dt = GetDataTable($"Select Id, Name, Description, Date from {PortalActivityTableName}");

            foreach (DataRow row in dt.Rows)
            {
                activities.Add(PortalActivityFromDataRow(row));
            }

            return activities.AsQueryable();
        }

        private PortalActivity PortalActivityFromDataRow(DataRow row)
        {
            return new PortalActivity()
            {
                Id = (int)row["Id"],
                Name = row["Name"].ToString(),
                Description = row["Description"].ToString(),
                Date = DateTime.Parse(row["Date"].ToString())
            };
        }

        public void AddActivity(PortalActivity activity)
        {
            ExecuteNonQuery($@"
                INSERT INTO {PortalActivityTableName} (Name, Description, Date)
                VALUES
                ('{activity.Name}',
                    '{activity.Description}',
                    '{activity.Date.ToString("MM/dd/yyyy h:mm tt")}' )
            ");
        }

        public void DeleteActivity(int id)
        {
            ExecuteNonQuery($@"
                DELETE FROM {PortalActivityTableName}  WHERE Id = {id};
            ");
        }

        public void UpdateActivity(PortalActivity activity)
        {
            ExecuteNonQuery($@"
                UPDATE {PortalActivityTableName}
                SET Name = '{activity.Name}',
                    Description = '{activity.Description}',
                    Date = '{activity.Date.ToString("MM/dd/yyyy h:mm tt")}'
                WHERE Id = {activity.Id};
            ");
        }

        public void AddAttendee(Attendee attendee)
        {
            ExecuteNonQuery($@"
                INSERT INTO {AttendeeTableName} (PortalActivityId, FirstName, LastName, Email)
                VALUES
                ({attendee.PortalActivityId}, '{attendee.FirstName}', '{attendee.LastName}', '{attendee.Email}' );
            ");
        }

        public IQueryable<Attendee> GetActivityAttendees(int activityId)
        {
            List<Attendee> attendees = new List<Attendee>();

            try
            {
                DataTable dt = GetDataTable($@"SELECT Id, PortalActivityId, FirstName, LastName, Email from {AttendeeTableName}
                                                WHERE PortalActivityId = {activityId} ");

                foreach (DataRow row in dt.Rows)
                {
                    var attendee = new Attendee()
                    {
                        Id = (int)row["Id"],
                        PortalActivityId = (int)row["PortalActivityId"],
                        FirstName = row["FirstName"].ToString(),
                        LastName = row["LastName"].ToString(),
                        Email = row["Email"].ToString()
                    };

                    attendees.Add(attendee);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error GetActivityAttendees");
            }

            return attendees.AsQueryable();
        }

        #endregion
    }
#endif
}