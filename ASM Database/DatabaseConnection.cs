using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASM_Database
{
    internal class DatabaseConnection
    {
        private static string _connectionString = "Data Source=DESKTOP-JVT0KIP\\SQLEXPRESS;Initial Catalog=PHONE_SHOP;Integrated Security=True;";
        
        public static SqlConnection GetConnection()
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error while connecting to the database", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return connection;
        }
    }
}
