using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WpfApp1.Classes
{
    public class Record
    {
        /// <summary> Код пластинки </summary>
        public int Id { get; set; }

        /// <summary> Наименование пластинки </summary>
        public string Name { get; set; }

        /// <summary> Год выпуска пластинки </summary>
        public int Year { get; set; }

        /// <summary> Формат записи пластинки (0 — МОНО, 1 — СТЕРЕО) </summary>
        public int Format { get; set; }

        /// <summary> Размер пластинки (0 — 7 дюймов, 1 — 10 дюймов, 2 — 12 дюймов, 3 — иное) </summary>
        public int Size { get; set; }

        /// <summary> Код производителя </summary>
        public int IdManufacturer { get; set; }

        /// <summary> Стоимость </summary>
        public float Price { get; set; }

        /// <summary> Состояние (0 — SS, 1 — EX, 2 — M, 3 — MM, 4 — VG, 5 — G, 6 — F, 7 — P, 8 — B) </summary>
        public int IdState { get; set; }

        /// <summary> Заметки </summary>
        public string Description { get; set; }

        /// <summary> Получение всех записей из БД </summary>
        public static IEnumerable<Record> AllRecords()
        {
            // Создаём список с записями о пластинке
            List<Record> records = new List<Record>();

            // Получаем список из базы данных
            DataTable recordQuery = Classes.DBConnection.Connection("SELECT * FROM [dbo].[Record]");

            // Читаем строки, заполняя данные в классе
            foreach (DataRow row in recordQuery.Rows)
            {
                records.Add(new Record()
                {
                    Id = Convert.ToInt32(row),
                    Name = row.ToString(),
                    Year = Convert.ToInt32(row),
                    Format = Convert.ToInt32(row),
                    Size = Convert.ToInt32(row),
                    IdManufacturer = Convert.ToInt32(row),
                    Price = float.Parse(row.ToString()),
                    IdState = Convert.ToInt32(row),
                    Description = row.ToString()
                });
            }

            // Возвращаем список
            return records;
        }

        /// <summary> Добавление или обновление записи </summary>
        public void Save(bool Update = false)
        {
            // Корректируем цену, заменяя запятую на точку
            string CorrectPrice = this.Price.ToString().Replace(",", ".");

            // Если данные необходимо внести (добавление)
            if (Update == false)
            {
                // Выполняем SQL-запрос на добавление записи
                Classes.DBConnection.Connection(
                    "INSERT INTO [dbo].[Record] (" +
                    "[Name], " +
                    "[Year], " +
                    "[Format], " +
                    "[Size], " +
                    "[IdManufacturer], " +
                    "[Price], " +
                    "[IdState], " +
                    "[Description]) " +
                    "VALUES (" +
                    $"N'{this.Name}', " +
                    $"{this.Year}, " +
                    $"{this.Format}, " +
                    $"{this.Size}, " +
                    $"{this.IdManufacturer}, " +
                    $"{CorrectPrice}, " +
                    $"{this.IdState}, " +
                    $"N'{this.Description}')");

                // Получаем ID записи
                this.Id = Record.AllRecords()
                    .Where(x => x.Name == this.Name &&
                               x.Year == this.Year &&
                               x.Format == this.Format &&
                               x.Size == this.Size &&
                               x.IdManufacturer == this.IdManufacturer &&
                               x.IdState == this.IdState &&
                               x.Description == this.Description)
                    .First().Id;
            }
            else
            {
                // Если данные необходимо изменить (обновление)
                // Выполняем SQL-запрос на обновление данных в БД
                Classes.DBConnection.Connection(
                    "UPDATE [dbo].[Record] " +
                    $"SET [Name] = N'{this.Name}', " +
                    $"[Year] = {this.Year}, " +
                    $"[Format] = {this.Format}, " +
                    $"[Size] = {this.Size}, " +
                    $"[IdManufacturer] = {this.IdManufacturer}, " +
                    $"[Price] = {CorrectPrice}, " +
                    $"[IdState] = {this.IdState}, " +
                    $"[Description] = N'{this.Description}' " +
                    $"WHERE [Id] = {this.Id}");
            }
        }

        

        public static void Export(string fileName, IEnumerable<Record> records)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                // Создаем рабочий лист
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Пластинки");

                // Заголовки столбцов
                worksheet.Cells["A1"].Value = "ID";
                worksheet.Cells["B1"].Value = "Наименование";
                worksheet.Cells["C1"].Value = "Год выпуска";
                worksheet.Cells["D1"].Value = "Формат записи";
                worksheet.Cells["E1"].Value = "Размер пластинки";
                worksheet.Cells["F1"].Value = "Код производителя";
                worksheet.Cells["G1"].Value = "Стоимость";
                worksheet.Cells["H1"].Value = "Состояние";
                worksheet.Cells["I1"].Value = "Заметки";

                // Заполняем данными
                int rowNum = 2;
                foreach (var record in records)
                {
                    worksheet.Cells[$"A{rowNum}"].Value = record.Id;
                    worksheet.Cells[$"B{rowNum}"].Value = record.Name;
                    worksheet.Cells[$"C{rowNum}"].Value = record.Year;
                    worksheet.Cells[$"D{rowNum}"].Value = record.Format;
                    worksheet.Cells[$"E{rowNum}"].Value = record.Size;
                    worksheet.Cells[$"F{rowNum}"].Value = record.IdManufacturer;
                    worksheet.Cells[$"G{rowNum}"].Value = record.Price;
                    worksheet.Cells[$"H{rowNum}"].Value = record.IdState;
                    worksheet.Cells[$"I{rowNum}"].Value = record.Description;

                    rowNum++;
                }

                // Сохраняем файл
                File.WriteAllBytes(fileName, package.GetAsByteArray());
            }
        }

    /// <summary> Удаление записи о пластинке </summary>
    public void Delete()
        {
            // Выполняем SQL-запрос на удаление данных в БД
            Classes.DBConnection.Connection($"DELETE FROM [dbo].[Record] WHERE [Id] = {this.Id};");
        }
    }
}
