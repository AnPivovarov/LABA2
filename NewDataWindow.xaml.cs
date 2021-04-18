using System;
using System.Collections.Generic;
using System.Data;
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

namespace LABA2
{
    /// <summary>
    /// Логика взаимодействия для NewDataWindow.xaml
    /// </summary>
    public partial class NewDataWindow : Window
    {
        static Paging PagedTable = new Paging();
        public NewDataWindow()
        {
            InitializeComponent();
            DataTable firstTable = PagedTable.SetPaging(MainWindow.threatsBefore, MainWindow.threatsBefore.Count);
            ThreatsBeforeDataGrid.ItemsSource = firstTable.DefaultView;
            DataTable secondTable = PagedTable.SetPaging(MainWindow.threatsAfter, MainWindow.threatsAfter.Count);
            ThreatsAfterDataGrid.ItemsSource = secondTable.DefaultView;
        }

        private void NDWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
