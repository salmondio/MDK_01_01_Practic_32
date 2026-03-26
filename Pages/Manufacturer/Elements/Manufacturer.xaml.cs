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
using WpfApp1.Classes;

namespace WpfApp1.Pages.Manufacturer.Elements
{
    /// <summary>
    /// Логика взаимодействия для Manufacturer.xaml
    /// </summary>
    public partial class Manufacturer : UserControl
    {
        // Обращаемся к классу Country, для того чтобы получить все страны
        IEnumerable<Classes.Country> Countries = Country.All();

        // Ссылка на страницу которая создала элемент
        Pages.Manufacturer.Main main;

        // Класс поставщика для получения данных
        Classes.Manufacturer manufacturer;

        // Конструктор который принимает данные поставщика и страницу Main для того чтобы обращаться к её элементам
        public Manufacturer(Classes.Manufacturer manufacturer, Pages.Manufacturer.Main main)
        {
            // Инициализируем компоненты чтобы с ними работать
            InitializeComponent();

            // В поле наименования присваиваем данные о поставщике
            tbName.Text = manufacturer.Name;

            // В поле страны присваиваем данные о стране
            // Получаем данные из массива стран по ID и выводим наименование
            tbCountry.Text = Countries.Where(x => x.Id == manufacturer.CountryCode).First().Name;

            // В поле номера телефона вставляем номер телефона
            tbPhone.Text = manufacturer.Phone.ToString();

            // В поле почты вставляем почту поставщика
            tbEmail.Text = manufacturer.Mail;

            // Запоминаем данные о поставщике для последующего взаимодействия
            this.manufacturer = manufacturer;

            // Запоминаем страницу Main, для последующего взаимодействия
            this.main = main;
        }

        /// <summary> Метод редактирования поставщика
        /// </summary>
        private void EditManufacturer(object sender, RoutedEventArgs e)
        {
            // Обращаемся к главному окну и вызываем метод открытия страницы
            // Вызываем страницу добавления, передавая в неё данные о поставщике
            // Тем самым страница добавления становится страницей редактирования
            MainWindow.mainWindow.OpenPage(new Pages.Manufacturer.Add(this.manufacturer));
        }

        /// <summary> Метод удаления поставщика
        /// </summary>
        private void DeleteManufacturer(object sender, RoutedEventArgs e)
        {
            // Выводим текстовое предупреждение о том что вы собираетесь удалить поставщика
            if (MessageBox.Show($"Удалить поставщика: {this.manufacturer.Name}?", "Уведомление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // Обращаемся к классу пластинок и ищем нет ли у нас пластинки с нашим поставщиком
                if (Classes.Record.AllRecords().Where(x => x.IdManufacturer == manufacturer.Id).Count() > 0)
                {
                    // Если такая пластинка существует, выводим уведомление о невозможности удаления
                    MessageBox.Show("Поставщика невозможно удалить. Для начала удалите зависимости.", "Уведомление");
                }
                else
                {
                    // Если пластинок не существует
                    // Обращаемся к классу поставщика и вызываем метод удаления
                    this.manufacturer.Delete();

                    // Обращаемся к странице, которая создала элемент и удаляем с неё самого себя
                    main.ManufacturerParent.Children.Remove(this);

                    // Выводим сообщение об удалении
                    MessageBox.Show($"Поставщик {this.manufacturer.Name} успешно удалён.", "Уведомление");
                }
            }
        }
    }
}
