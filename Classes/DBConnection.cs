using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace WpfApp1.Classes
{
    public class DBConnection
    {
        public static DataTable Connection(string query)
        {
            DataTable dt = new DataTable("Datatable");
            SqlConnection sqlConnection = new SqlConnection("");
            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = query;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);

            return dt;
        }
    }
}
