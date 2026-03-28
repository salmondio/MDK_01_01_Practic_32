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

namespace WpfApp1.Pages.Supply
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        IEnumerable<Classes.Supply> AllSupplies = Classes.Supply.AllSupples();
        public Main()
        {
            InitializeComponent();

            foreach (Classes.Supply supply in AllSupplies)
                SupplyParent.Children.Add(new Elements.Supply(supply, this));
        }
    }
}
