using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Classes
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static IEnumerable<Country> All()
        {
            List<Country> all = new List<Country>();
            DataTable countyTable = DBConnection.Connection("SELECT * FROM [dbo].[Country]");

            foreach(DataRow row in countyTable.Rows)
            {
                all.Add(new Country() {
                    Id = Convert.ToInt32(row[0]),
                    Name = row[1].ToString()
                });
            }

            return all;
        }
    }
}
