using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Classes
{
    public class State
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subname { get; set; }
        public string Description { get; set; }
        public static IEnumerable<State> AllState()
        {
            List<State> allState = new List<State>();
            DataTable requestStates = DBConnection.Connection("SELECT * FROM [dbo].[State]");
            foreach (DataRow state in requestStates.Rows)
                allState.Add(new State(){
                    Id = Convert.ToInt32(state[0]),
                    Name = state[1].ToString(),
                    Subname = state[2]. ToString(),
                    Description = state[3].ToString()
                });
            return allState;
        }
        public void Save(bool Update = false)
        {
            if (Update == false)
            {
                Classes.DBConnection.Connection(
                "INSERT INTO [dbo].[State]([Name], [Subname], [Description]) " +
                $"VALUES (N'{this.Name}', N'{this.Subname}', N'{this.Description}');");
                this.Id = AllState().Where(x => x.Name == this.Name && x.Subname == this.Subname && x.Description == this.Description).First().Id;
            }
            else
            {
                Classes.DBConnection.Connection(
                "UPDATE [dbo].[State] SET " +
                $"[Name] = N'{this.Name}', " +
                $"[Subname] = '{this.Subname}', " +
                $"[Description] = N'{this.Description}' " +
                $"WHERE[Id] = {this.Id};");
            }
        }
        public void Delete()
        {
            Classes.DBConnection.Connection($"DELETE FROM [dbo].[State] WHERE [Id] = {this.Id};");
        }
    }
}
