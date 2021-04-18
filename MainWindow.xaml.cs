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
using System.Data;
using System.Reflection;
using System.IO;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Win32;
using System.Net;

namespace LABA2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int numberOfRecPerPage = 15;
        static Paging PagedTable = new Paging();

        private bool isUpdated = false;

        IEnumerable<Threat> threats = null;
        IEnumerable<Threat> threatsNew = null;

        List<Threat> threatsBefore = new List<Threat>();
        List<Threat> threatsAfter = new List<Threat>();
        public MainWindow()
        {

            InitializeComponent();
            if (!File.Exists("thrlist.xlsx"))
            {
                DownloadWindow dw = new DownloadWindow();
                dw.Show();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            ThreatsDataGrid.Visibility = Visibility.Visible;
            threats = XLSXImport.EnumerateThreats("thrlist.xlsx").ToList();
            PagedTable.PageIndex = 0;
            DataTable firstTable = PagedTable.SetPaging(threats, numberOfRecPerPage);
            ThreatsDataGrid.ItemsSource = firstTable.DefaultView;
            PageInfoLabel.Content = PageNumberDisplay();
            PrevPageButton.Visibility = Visibility.Visible;
            NextPageButton.Visibility = Visibility.Visible;
            UpdateButton.IsEnabled = true;
            SaveButton.IsEnabled = true;
        }
        public string PageNumberDisplay()
        {
            int PagedNumber = numberOfRecPerPage * (PagedTable.PageIndex + 1);
            if (PagedNumber > threats.Count())
            {
                PagedNumber = threats.Count();
            }
            return PagedNumber + " из " + threats.Count();
        }

        private void PrevPageButton_Click(object sender, RoutedEventArgs e)
        {
            ThreatsDataGrid.ItemsSource = PagedTable.Previous(threats, numberOfRecPerPage).DefaultView;
            PageInfoLabel.Content = PageNumberDisplay();
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            ThreatsDataGrid.ItemsSource = PagedTable.Next(threats, numberOfRecPerPage).DefaultView;
            PageInfoLabel.Content = PageNumberDisplay();

        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            threats = XLSXImport.EnumerateThreats("thrlist.xlsx").ToList();
            WebClient webClient = new WebClient();
            webClient.DownloadFile("https://bdu.fstec.ru/files/documents/thrlist.xlsx", "thrlist.xlsx");
            threatsNew = XLSXImport.EnumerateThreats("thrlist.xlsx").ToList();

            List<Threat> threatsList = threats.ToList();
            List<Threat> threatsNewList = threatsNew.ToList();

            int count = 0;
            for (int i = 0; i < threatsList.Count; i++)
            {
                if (!threatsList[i].Equals(threatsNewList[i]))
                {
                    isUpdated = true;
                    threatsBefore.Add(threatsList[i]);
                    threatsAfter.Add(threatsNewList[i]);
                    count++;
                }
            }
            if (count == 0)
            {
                UpdateFail uf = new UpdateFail();
                uf.Show();
            }
            else
            {
                UpdateSuccess us = new UpdateSuccess();
                us.UpdateText.Text = $"Обновлено следующее количество записей: {count}";
                us.Show();
            }
            if (threatsList.Count < threatsNewList.Count)
            {
                for (int i = threatsList.Count; i < threatsNewList.Count; i++)
                {
                    threatsAfter.Add(threatsNewList[i]);
                }
            }

            threats = XLSXImport.EnumerateThreats("thrlist.xlsx").ToList();
            PagedTable.PageIndex = 0;
            DataTable firstTable = PagedTable.SetPaging(threats, numberOfRecPerPage);
            ThreatsDataGrid.ItemsSource = firstTable.DefaultView;
            PageInfoLabel.Content = PageNumberDisplay();


        }

        private MemoryStream GetStream(XLWorkbook excelWorkbook)
        {
            MemoryStream fs = new MemoryStream();
            excelWorkbook.SaveAs(fs);
            fs.Position = 0;
            return fs;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var wb = new XLWorkbook();
            DataTable dt = PagedTable.SetPaging(threats, threats.Count());
            var ws = wb.Worksheets.Add(dt, "Threats");
            //wb.SaveAs("test2.xlsx");
            ws.RowHeight = 14;
            ws.ColumnWidth = 80;
            ws.Column(1).Width = 20;
            ws.Column(6).Width = 10;
            ws.Column(7).Width = 10;
            ws.Column(8).Width = 10;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "savedList.xlsx";
            sfd.Filter = "Excel Documents (*.xlsx)|*.xlsx";
            sfd.DefaultExt = ".xlsx";
            var sel = sfd.ShowDialog();
            if (sel == true)
            {
                wb.SaveAs(sfd.FileName);
                
            }
        }

        
        
    }
}
