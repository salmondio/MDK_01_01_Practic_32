using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Classes
{
    public class Manufacturer
    {
        public int Id { get; set; }

        /// <summary> Наименование поставщика </summary>
        public string Name { get; set; }

        /// <summary> Код страны </summary>
        public int CountryCode { get; set; }

        /// <summary> Телефон </summary>
        public string Phone { get; set; }

        /// <summary> Почта </summary>
        public string Mail { get; set; }

        /// <summary> Список всех поставщиков </summary>
        public static IEnumerable<Manufacturer> AllManufacturers()
        {
            // Создаём список поставщиков
            List<Manufacturer> manufacturers = new List<Manufacturer>();

            // Выполняем запрос к базе данных, на получение всех данных
            DataTable recordQuery = Classes.DBConnection.Connection("SELECT * FROM [dbo].[Manufacturer]");

            // Перебираем строки в запросе
            foreach (DataRow row in recordQuery.Rows)
            {
                // Добавляем в список поставщика, присвоившая данные на свои места
                manufacturers.Add(new Manufacturer()
                {
                    Id = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    CountryCode = Convert.ToInt32(row[2]),
                    Phone = row[3].ToString(),
                    Mail = row[4].ToString()
                });
            }
            // Возвращаем список поставщиков
            return manufacturers;
        }

        /// <summary> Добавление или обновление записи </summary>
        public void Save(bool Update = false)
        {
            // Если добавление записи
            if (Update == false)
            {
                // Создаём запрос на добавление записи, выполняя SQL код
                Classes.DBConnection.Connection(
                    "INSERT INTO [dbo].[Manufacturer]([Name], [CountryCode], [Phone], [Mail])" +
                    "VALUES (" +
                    $"N'{this.Name}', " +
                    $"'{this.CountryCode}', " +
                    $"'{this.Phone}', " +
                    $"'{this.Mail}')");

                // Выставляем обратно ID, необходимо для изменения
                // Ищем среди всех поставщиков, поставившим у которого полностью совпадают данные и получаем ID
                this.Id = Manufacturer.AllManufacturers().Where(
                    x => x.Name == this.Name &&
                    x.CountryCode == this.CountryCode &&
                    x.Phone == this.Phone &&
                    x.Mail == this.Mail).First().Id;
            }
            else
            {
                // если у нас выполняется обновление данных о поставщике
                // Создаём запрос на обновление записи, выполняя SQL код
                Classes.DBConnection.Connection(
                    "UPDATE [dbo].[Manufacturer] SET " +
                    $"[Name] = N'{this.Name}', " +
                    $"[CountryCode] = '{this.CountryCode}', " +
                    $"[Phone] = '{this.Phone}', " +
                    $"[Mail] = '{this.Mail}' " +
                    $"WHERE [Id] = '{this.Id}';");
            }
        }

        /// <summary> Удаление записи о поставщике </summary>
        public void Delete() =>
            // Выполняем SQL код, который удаляет данные о поставщике по ID
            Classes.DBConnection.Connection($"DELETE FROM [dbo].[Manufacturer] WHERE [Id] = '{this.Id}';");
    }
}
