using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace CameraManage
{
    public partial class Form1 : Form
    {
        FilterInfoCollection videoDevices;
        VideoCaptureDevice videoSource;
        public int selectedDeviceIndex = 0;
        public Form1()
        {
            InitializeComponent();
            this.FormClosed += Form1_FormClosed;
        }

        #region one
        ////连接摄像头
        private void btnLink_Click(object sender, EventArgs e)
        {
            if (btnLink.Text == "连  接")
            {
                btnLink.Text = "断  开";
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                selectedDeviceIndex = 0;
                videoSource = new VideoCaptureDevice(videoDevices[selectedDeviceIndex].MonikerString); //连接摄像头。
                videoSource.VideoResolution = videoSource.VideoCapabilities[selectedDeviceIndex];
                videoSourcePlayer1.VideoSource = videoSource;
                videoSourcePlayer1.Start();
            }
            else
            {
                btnLink.Text = "连  接";
                videoSourcePlayer1.Stop();
            }
        }

        ////拍照
        private void btnTask_Click(object sender, EventArgs e)
        {
            if (videoSource == null)
                return;
            Bitmap bitmap = videoSourcePlayer1.GetCurrentVideoFrame();
            string fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ff") + ".jpg";
            string path = AppDomain.CurrentDomain.BaseDirectory;
            bitmap.Save(path + fileName, ImageFormat.Jpeg);
            bitmap.Dispose();
        }
        #endregion

        #region two
        public FilterInfoCollection USE_Webcams = null;
        public VideoCaptureDevice cam = null;
        //---按钮被单击事件
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnStart.Text == "开始")
                {
                    btnStart.Text = "停止";
                    ///---启动摄像头
                    cam.Start();
                }
                else
                {
                    btnStart.Text = "开始";
                    ///--停止摄像头捕获图像
                    cam.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                ///---实例化对象
                USE_Webcams = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                ///---摄像头数量大于0
                if (USE_Webcams.Count > 0)
                {
                    ///---禁用按钮
                    btnStart.Enabled = true;
                    ///---实例化对象
                    cam = new VideoCaptureDevice(USE_Webcams[0].MonikerString);
                    ///---绑定事件
                    cam.NewFrame += new NewFrameEventHandler(Cam_NewFrame);
                }
                else
                {
                    ///--没有摄像头
                    btnStart.Enabled = false;
                    ///---提示没有摄像头
                    MessageBox.Show("没有摄像头外设");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        ///------自定义函数
        private void Cam_NewFrame(object obj, NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }


        ///---窗口关闭事件
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (cam != null)
                {
                    ///---关闭摄像头
                    if (cam.IsRunning)
                    {
                        cam.Stop();
                    }
                }
                videoSourcePlayer1.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}
