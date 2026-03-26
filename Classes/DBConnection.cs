using System.Data;
using System.Data.SqlClient;

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
