using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace RequestImg
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            string txtPath = ConfigurationManager.AppSettings["TxtFile"];
            if (!Directory.Exists(txtPath))
            {
                Directory.CreateDirectory(txtPath);
            }
            fileAllName = txtPath + @"\key.txt";
            RequestHelp.SetUrl += SetLabShowContent;
        }

        private string oauthId = "";
        private string fileAllName = "";
        Dictionary<string, string> myparam;
        private async void Oauth_Click(object sender, RoutedEventArgs e)
        {
            await GetKey(fileAllName);
        }

        List<string> resultData;
        private async void Mpart_Click(object sender, RoutedEventArgs e)
        {
            listResult.Items.Clear();
            resultData = new List<string>();
            myparam = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(txtKey.Text)) myparam["key"] = txtKey.Text.Trim();
            if (!string.IsNullOrEmpty(txtStart.Text)) myparam["start"] = txtStart.Text.Trim();
            if (!string.IsNullOrEmpty(txtSize.Text)) myparam["size"] = txtSize.Text.Trim();
            ResultList<Mpart> result = await RequestHelp.TryGetAsync<ResultList<Mpart>>($"api/{oauthId}/search/{txtMTable.Text.Trim()}", myparam);
            if (result == null)
            {
                txtMessage.Text = "未获取授权";
            }
            if (result.errcode == 0)
            {
                foreach (var mpart in result.list)
                {
                    resultData.Add(mpart.objId);
                }
                txtMessage.Text = "Success";
                resultData.ForEach(p => listResult.Items.Add(p));
            }
            else if (result.errcode == 502)
            {
                txtMessage.Text = "授权过期，请重新获取";
            }
            else
            {
                MessageBox.Show(result.errmsg);
            }
        }

        private async void BtnGuan_Click(object sender, RoutedEventArgs e)
        {
            // http://localhost:8080/sipmweb/api/b2c33e12ebabc498ccd9ec39712a1d5c/relation/MPART /01_B864B6D8CC78460E852592BABC4E8DCB/data?re=MPART_DESF&extra=&item=DESF
            //http://192.168.0.197:7777/sipmweb/api/4e5d28459b5951d03c88eb4cc790c8e2/relation/MPART/01_8C570358FDED4186B39A7DDBFABD34DB/data?re=MPART_DESF&extra=&item=DESF
            listFResult.Items.Clear();
            resultData = new List<string>();
            myparam = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(txtGuanTable.Text)) myparam["re"] = txtGuanTable.Text.Trim();
            myparam["extra"] = txtExtra.Text.Trim();
            if (!string.IsNullOrEmpty(txtFileTable.Text)) myparam["item"] = txtFileTable.Text.Trim();
            var partNo = "";
            if (listResult.SelectedValue!=null)
            {
                partNo = listResult.SelectedValue.ToString();
            }
            else
            {
                if (!string.IsNullOrEmpty(txtPartNo.Text))
                {
                    partNo = txtPartNo.Text.Trim();
                }
                else
                {
                    MessageBox.Show("请选择要查询的零件");
                    return;
                }
            }
            ResultList<Guan> result = await RequestHelp.TryGetAsync<ResultList<Guan>>($"api/{oauthId}/relation/{txtFTable.Text.Trim()}/{partNo}/data", myparam);
            if (result == null)
            {
                txtFMessage.Text = "未获取授权";
                return;
            }
            if (result.errcode == 0)
            {
                txtMessage.Text = "Success";
                foreach (var mpart in result.list)
                {
                    resultData.Add(mpart.objId + ":" + mpart.fname);// + "." + mpart.suffix);
                }
                resultData.ForEach(p => listFResult.Items.Add(p));
            }
            else if (result.errcode == 30002)
            {
                txtFMessage.Text = "未找到零件文件信息";
            }
            else
            {
                MessageBox.Show(result.errmsg);
            }
        }

        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            if (listFResult.SelectedValue==null)
            {
                MessageBox.Show("请选择一条数据");
                return;
            }
            string uri = $"{RequestHelp.BaseUri}/web/search/detail?rid={txtOuath.Text.Trim()}&id={listFResult.SelectedValue.ToString().Split(':')[0]}&t={txtShowTable.Text.Trim()}";
            //System.Diagnostics.Process.Start(uri);
            System.Diagnostics.Process.Start("iexplore.exe", uri);
        }

        private async Task GetKey(string fileAllName)
        {
            myparam = new Dictionary<string, string>();
            myparam["uname"] = "mes";
            myparam["passwd"] = "25d55ad283aa400af464c76d713c07ad";
            myparam["f"] = "false";
            Oauth result = await RequestHelp.TryGetAsync<Oauth>("api/oauth", myparam);
            if (result.errcode == 0)
            {
                oauthId = result.errmsg;

                File.WriteAllText(fileAllName, oauthId);
            }
            else if (result.errcode == 504)
            {
                if (File.Exists(fileAllName))
                {
                    oauthId = File.ReadAllText(fileAllName);
                }
            }
            else
            {
                MessageBox.Show(result.errmsg);
            }
            txtOuath.Text = oauthId;
        }

        private void SetLabShowContent(string value)
        {
            this.labShow.Content = value;
        }
    }
}
