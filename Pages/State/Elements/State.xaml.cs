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

namespace WpfApp1.Pages.State.Elements
{
    /// <summary>
    /// Логика взаимодействия для State.xaml
    /// </summary>
    public partial class State : UserControl
    {
        /// <summary> Объект State, к которому привязывается интерфейс </summary>
        Classes.State state;
        /// <summary> Ссылка на страницу Main </summary>
        Pages.State.Main main;

        public State(Classes.State state, Pages.State.Main main)
        {
            InitializeComponent();

            this.state = state;
            this.main = main;
            tbName.Text = this.state.Name;
            tbSubname.Text = this.state.Subname;
            tbDescription.Text = this.state.Description;
        }

        private void EditState(object sender, RoutedEventArgs e) =>
            // Обращаемся к главному окну, и вызываем метод открытия страниц
            // Открываем страницу добавления состояний и передаём состояние которое будем изменять
            MainWindow.mainWindow.OpenPage(new Pages.State.Add(state));

        private void DeleteState(object sender, RoutedEventArgs e)
        {
            // Выводим текстовое предупреждение о том что вы собираетесь удалить состояние
            if (MessageBox.Show($"Удалить состояние: {this.state.Name}?", "Уведомление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // Получаем все записи пластинок
                IEnumerable<Classes.Record> AllRecord = Classes.Record.AllRecords();
                // Обращаемся к пластинкам и ищем нет ли у нас пластинки с нашим состоянием
                if (AllRecord.Where(x => x.IdState == state.Id).Count() > 0)
                {
                    // Если такая пластинка существует, выводим уведомление о невозможности удалить
                    MessageBox.Show($"Состояние {this.state.Name} невозможно удалить. Для начала удалите зависимости.", "Уведомление");
                }
                else
                {
                    // Если пластинок не существует
                    // Обращаемся к классу состояния и вызываем метод удаления
                    this.state.Delete();
                    // Обращаемся к странице, которая создала элемент и удаляем с ней самого себя
                    main.StateParent.Children.Remove(this);
                    // Выводим сообщение об удалении
                    MessageBox.Show($"Состояние {this.state.Name} успешно удалена.", "Уведомление");
                }
            }
        }
    }
}
