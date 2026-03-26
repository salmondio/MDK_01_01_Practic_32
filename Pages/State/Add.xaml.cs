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

namespace WpfApp1.Pages.State
{
    /// <summary>
    /// Логика взаимодействия для Add.xaml
    /// </summary>
    public partial class Add : Page
    {
        private Classes.State changeState;

        public Add(Classes.State state = null)
        {
            InitializeComponent();
            // Если у нас присутствует объект для изменения состояния
            if (state != null)
            {
                // Запоминаем его в переменную
                this.changeState = state;
                // В поля для данных, подставляем данные
                this.tbName.Text = state.Name;
                this.tbSubname.Text = state.Subname;
                this.tbDescription.Text = state.Description;
                // Изменяем надпись на кнопке
                addBtn.Content = "Изменить";
            }
        }

        /// <summary> Добавление или изменение состояния
        /// </summary>
        private void AddState(object sender, RoutedEventArgs e)
        {
            // Если поле с наименованием не пустое
            if (!string.IsNullOrEmpty(tbName.Text))
            {
                // Если поле с скор. наименованием не пустое
                if (!string.IsNullOrEmpty(tbSubname.Text))
                {
                    // Если не существует состояния для изменения
                    if (this.changeState == null)
                    {
                        // Создаём новый объект состояния
                        Classes.State newState = new Classes.State()
                        {
                            Name = tbName.Text,
                            Subname = tbSubname.Text,
                            Description = tbDescription.Text
                        };
                        // Сохраняем состояние в базу данных
                        newState.Save();
                        // Выводим сообщение о том что состояние добавлено
                        MessageBox.Show($"Состояние {newState.Name} успешно добавлено.", "Уведомление");
                        // Открываем страницу для изменения добавленного состояния
                        MainWindow.mainWindow.OpenPage(new Pages.State.Add(newState));
                    }
                    else
                    {
                        // Изменяем данные состояния
                        changeState.Name = tbName.Text;
                        changeState.Subname = tbSubname.Text;
                        changeState.Description = tbDescription.Text;
                        // Обновляем состояние в базе данных
                        changeState.Save(true);
                        // Выводим сообщение о том что состояние изменено
                        MessageBox.Show($"Состояние {changeState.Name} успешно изменено.", "Уведомление");
                    }
                }
                else
                {
                    // Выводим сообщение об ошибке
                    MessageBox.Show("Пожалуйста, укажите сокращённое наименование состояния.", "Предупреждение");
                }
            }
            else
            {
                // Выводим сообщение об ошибке
                MessageBox.Show("Пожалуйста, укажите наименование состояния.", "Предупреждение");
            }
        }
    }
}
