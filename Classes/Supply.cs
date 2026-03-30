using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Classes
{
    public class Supply
    {
        /// <summary> Код поставки </summary>
        public int Id { get; set; }

        /// <summary> Код поставщика </summary>
        public int IdManufacturer { get; set; }

        /// <summary> Код пластинки </summary>
        public int IdRecord { get; set; }

        /// <summary> Дата доставки </summary>
        public string DateDelivery { get; set; }

        /// <summary> Кол-во доставки </summary>
        public int Count { get; set; }

        /// <summary> Получение данных о всех поставках </summary>
        public static IEnumerable<Supply> AllSupples()
        {
            // Создаём список поставок
            List<Supply> supples = new List<Supply>();

            // Обращаемся к БД и получаем список
            DataTable recordQuery = Classes.DBConnection.Connection("SELECT * FROM [dbo].[Supple]");

            // Читаем строки
            foreach (DataRow row in recordQuery.Rows)
            {
                // Создаём дату
                DateTime dt = new DateTime();

                // Конвертируем дату из БД
                DateTime.TryParse(row[3].ToString(), out dt);

                // Записываем полученный результат в переменную
                string CorrectDate = dt.Year + "-" + dt.Month + "-" + dt.Day;

                // Заполняем список, указывая данные
                supples.Add(new Supply()
                {
                    Id = Convert.ToInt32(row[0]),
                    IdManufacturer = Convert.ToInt32(row[1]),
                    IdRecord = Convert.ToInt32(row[2]),
                    DateDelivery = CorrectDate,
                    Count = Convert.ToInt32(row[4])
                });
            }

            // Возвращаем список
            return supples;
        }

        /// <summary> Сохранение данных </summary>
        public void Save(bool Update = false)
        {
            // Если создание данных в БД
            if (Update == false)
            {
                // Вызываем SQL запрос, который создаёт данные в БД
                Classes.DBConnection.Connection(
                    "INSERT INTO [dbo].[Supple]([IdManufacturer], [IdRecord], [DateDelivery], [Count]) " +
                    $"VALUES ({this.IdManufacturer}, {this.IdRecord}, '{this.DateDelivery}', {this.Count});");

                // Получаем запись обратно
                // Получаем ID записи
                this.Id = Supply.AllSupples().Where(
                    x => x.IdManufacturer == this.IdManufacturer &&
                         x.IdRecord == this.IdRecord &&
                         x.DateDelivery == this.DateDelivery &&
                         x.Count == this.Count).First().Id;
            }
            else
            {
                // Если данные необходимо обновить
                // Вызываем SQL запрос на обновление данных
                Classes.DBConnection.Connection(
                    "UPDATE [dbo].[Supple] " +
                    "SET " +
                    $"[IdManufacturer] = {this.IdManufacturer}, " +
                    $"[IdRecord] = {this.IdRecord}, " +
                    $"[DateDelivery] = '{this.DateDelivery}', " +
                    $"[Count] = {this.Count} " +
                    $"WHERE [Id] = {this.Id};");
            }
        }

        /// <summary> Удаление записи о пластинке </summary>
        public void Delete()
        {
            // Вызываем SQL запрос на удаление данных
            Classes.DBConnection.Connection($"DELETE FROM [dbo].[Supple] WHERE [Id] = {this.Id};");
        }
    }
}
