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

namespace PrintDemoSingle
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.gridView.ItemsSource = GetData();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            string errorMsg = "";
            string templateName = "test.docx";
            var printData = this.gridView.ItemsSource as List<Data>;
            Print.Print_Task<Data>(printData, templateName, ref errorMsg);
        }

        private List<Data> GetData()
        {
            List<Data> datas = new List<Data>()
            {
                new Data()
                {
                    userId="1",
                    userName="user1",
                    userPwd="123",
                    userTest="test"
                },
                new Data()
                {
                    userId="2",
                    userName="user2",
                    userPwd="456",
                    userTest="test"
                },
                new Data()
                {
                    userId="3",
                    userName="user3",
                    userPwd="789",
                    userTest="test"
                },
                new Data()
                {
                    userId="11",
                    userName="user1",
                    userPwd="123",
                    userTest="test"
                },
                new Data()
                {
                    userId="22",
                    userName="user2",
                    userPwd="456",
                    userTest="test"
                },
                new Data()
                {
                    userId="33",
                    userName="user3",
                    userPwd="789",
                    userTest="test"
                }
            };
            return datas;
        }
    }

    public class Data
    {
        public string userTest { get; set; }
        public string userId { get; set; }
        public string userName { get; set; }
        public string userPwd { get; set; }
    }
}
