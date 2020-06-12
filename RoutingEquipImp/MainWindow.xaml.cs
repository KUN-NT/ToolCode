using Aspose.Cells;
using System.IO;
using System.Windows;

namespace RoutingEquipImp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            labFinish.Visibility = Visibility.Hidden;
            txtFilePath.Text = @"E:\JobCode\VS2012\Documents\GBS\IMP";
            txtIndex.Text = "0";
            txtCount.Text = "200";
            txtType.Text = "xls";
        }

        private async void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            labFinish.Visibility = Visibility.Hidden;
            i = 1;
            txtMessage.Text = "";
            btnRead.IsEnabled = false;
            int index = int.Parse(txtIndex.Text);
            int count = int.Parse(txtCount.Text);
            string dataFolder = txtFilePath.Text;
            foreach (var filePath in Directory.GetFiles(dataFolder, $"*.{txtType.Text}", SearchOption.TopDirectoryOnly))
            {
                if (filePath.Contains("$")) continue;
                using (var book = new Workbook(filePath))
                {
                    if (index == 0)
                    {
                        await ReadEquipData(book.Worksheets[index], count);
                    }
                    else if (index == 1)
                    {
                        await ReadRoutingData(book.Worksheets[index], count);
                    }
                    else
                    {
                        MessageBox.Show("404 Not Found");
                    }
                }
            }
            btnRead.IsEnabled = true;
            labFinish.Visibility = Visibility.Visible;
            MessageBox.Show($"处理完成{num}项");
        }

        private async void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(txtIndex.Text);
            int count = int.Parse(txtCount.Text);
            string dataFolder = txtFilePath.Text;
            btnCheck.IsEnabled = false;
            foreach (var filePath in Directory.GetFiles(dataFolder, $"*.{txtType.Text}", SearchOption.TopDirectoryOnly))
            {
                if (filePath.Contains("$")) continue;
                using (var book = new Workbook(filePath))
                {
                    await DataCheck(book.Worksheets);
                }
            }
            btnCheck.IsEnabled = true;
        }

        private static int i = 1;
        private static int num = 0;
        private void SetMessage(bool isSuccess,string msg)
        {
            if (isSuccess) num ++;
            txtMessage.Text += i.ToString() + "\t" + msg + "\r\n";
            i++;
        }
    }
}
