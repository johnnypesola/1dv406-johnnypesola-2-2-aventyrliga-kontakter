using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace _2_2_aventyrliga_kontakter.Model
{
    public abstract class DALBase
    {
        static private string _connectionString;
        protected const string DAL_ERROR_MSG = "An error occured in the data access layer.";

        protected enum DALConnectOptions { open, closed };

        static protected SqlConnection connection { get; set; }

        // Constructor
        static DALBase()
        {
            /*
             * Den statiska konstruktorn initierar det statiska fältet genom att hämta anslutningssträngen från filen Web.config.
             */

            _connectionString = WebConfigurationManager.ConnectionStrings["ContactConnectionString"].ConnectionString;
        }

        protected SqlConnection CreateConnection()
        {
            /*
             * Metoden CreateConnection är ”protected” och skapar och returnerar en referens till ett anslutningsobjekt.
             */
            connection = new SqlConnection(_connectionString);

            return connection;
        }

        protected SqlCommand Connect(string commandName, DALConnectOptions options = DALConnectOptions.open)
        {
            SqlCommand cmd;

            // Create Sql command object
            cmd = new SqlCommand(commandName, connection);

            // Set Type to StoreProcedure, which we will be executing.
            cmd.CommandType = CommandType.StoredProcedure;

            // Open connection to database if opted for.
            if (DALConnectOptions.open == options)
            {
                connection.Open();
            }

            return cmd;
        }
    }
}