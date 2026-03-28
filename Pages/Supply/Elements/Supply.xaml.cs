using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1.Pages.Supply.Elements
{
    /// <summary>
    /// Логика взаимодействия для Supply.xaml
    /// </summary>
    public partial class Supply : UserControl
    {
        /// <summary> Обращаемся к классу поставщиков и получаем все данные </summary>
        IEnumerable<Classes.Manufacturer> AllManufacturers = Classes.Manufacturer.AllManufacturers();

        /// <summary> Обращаемся к классу пластинок и получаем все данные </summary>
        IEnumerable<Classes.Record> AllRecords = Classes.Record.AllRecords();

        /// <summary> Создаём переменную которая будет хранить данные о поставке для пос... </summary>
        Classes.Supply supply;

        /// <summary> Создаём переменную которая будет хранить ссылку на страницу Маин д... </summary>
        Pages.Supply.Main main;

        /// <summary> Конструктор который получает данные о поставке и страницу Маин </summary>
        public Supply(Classes.Supply supply, Pages.Supply.Main main)
        {
            // Инициализируем данные на элементе
            InitializeComponent();

            // Сохраняем данные о поставке
            this.supply = supply;

            // Сохраняем данные о странице Маин
            this.main = main;

            // В поле поставщика указываем поставщика, предварительно найдя его по уникальному коду
            tbManufacturer.Text = AllManufacturers.Where(x => x.Id == supply.IdManufacturer).First().Name;

            // В поле пластинки указываем пластинку, предварительно найдя её по уникальному коду
            tbRecord.Text = AllRecords.Where(x => x.Id == supply.IdRecord).First().Name;

            // В дату поставки указываем данные о дате поставки
            tbDateDelivery.Text = CorrectDate(supply.DateDelivery);

            // В поле количества указываем данные о количестве поставки
            tbCount.Text = supply.Count.ToString();
        }

        /// <summary> Корректировка даты </summary>
        private string CorrectDate(string Value)
        {
            // Обрезаем часы пришедшие с базы данных
            return Value.Split(' ')[0];
        }

        /// <summary> Метод изменения поставки </summary>
        private void EditSupply(object sender, RoutedEventArgs e)
        {
            // Обращаемся к главной форме и вызываем метод открытия страниц
            // Открываем страницу добавления, передавая в неё данные
            MainWindow.mainWindow.OpenPage(new Pages.Supply.Add(this.supply));
        }

        /// <summary> Метод удаления поставки </summary>
        private void DeleteSupply(object sender, RoutedEventArgs e)
        {
            // Запрашиваем у пользователя подтверждение на удаление
            if (MessageBox.Show($"Удалить поставку №{this.supply.Id}?", "Уведомление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // Если пользователь согласился, удаляем запись из БД
                this.supply.Delete();

                // Удаляем запись с интерфейса
                main.SupplyParent.Children.Remove(this);

                // Выводим сообщение о том что запись удалена
                MessageBox.Show($"Поставка №{this.supply.Id} успешно удалена.", "Уведомление");
            }
        }
    }
}
