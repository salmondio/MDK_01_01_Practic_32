using System.Data;
using System.Data.SqlClient;

namespace WpfApp1.Classes
{
    public class DBConnection
    {
        public static DataTable Connection(string query)
        {
            DataTable dt = new DataTable("Datatable");
            SqlConnection sqlConnection = new SqlConnection("server=10.0.201.112;Trusted_Connection=No;DataBase=base1_ISP_23_2_8;User=ISP_23_2_8;PWD=egW19je7D1_");
            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = query;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);

            return dt;
        }
    }
}
