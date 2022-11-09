using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace TKExercise
{
    //Provides methods for connecting to the SQL Server database and getting data
    internal class SQLServerConnection
    {
        string host, user, password; //Database connection details
        string connectionString; //Connection string
        string errorMessage; //Contains error message

        DataTable sqlResponse;

        public string ErrorMessage { get => errorMessage; }

        public DataTable SqlResponse { get => sqlResponse; }

        //Disable warning that warns about non nullable field that must contain non null variable when exiting constructor
        //These fields are initialized in separate method but warning still occurs so disable it for clarity
        #pragma warning disable CS8618

        //Default constructor that doesn't setup anything
        public SQLServerConnection()
        {
            SetConnectionDetails("", "", "");
        }

        //Constructor that setup connection details
        public SQLServerConnection(string host, string user, string password)
        {
            SetConnectionDetails(host, user, password);
        }

        //Values are set so restore warning for rest of the code
        #pragma warning restore CS8618

        //Setup connection details
        public void SetConnectionDetails(string host, string user, string password)
        {
            this.host = host;
            this.user = user;
            this.password = password;

            connectionString = "Data Source = " + host + ";Initial Catalog = DevData; User ID = " + user + "; Password = " + password + "";
        }

        //Try connecting to database and return if it was possible
        public bool TryConnect()
        {
            if (host == "" || user == "" || password == "")
            {
                //No connection details so exit with error
                return false;
            }

            SqlConnection connection = new SqlConnection(connectionString);

            try 
            { 
                connection.Open(); 
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            finally
            {
                connection.Close();
            }

            return true;
        }

        //Get details about int columns in tables
        public bool GetIntColumnsDetails()
        {
            //Check if connection is valid before attempting to load data
            if (!TryConnect())
            {
                return false;
            }

            string query = "SELECT COLUMN_NAME, TABLE_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE INFORMATION_SCHEMA.COLUMNS.DATA_TYPE = 'int'";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter data = new SqlDataAdapter(command);

            sqlResponse = new DataTable();

            try
            {
                connection.Open();
                data.Fill(sqlResponse); //Put response to the DataTable
            }
            catch (SqlException ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            finally
            {
                connection.Close();
                data.Dispose();
            }

            return true;
        }
    }
}
