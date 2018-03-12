using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using FaceDetection;

using AForge.Video.DirectShow;

namespace face
{
    public partial class Form1 : Form
    {
        //  private VideoSourcePlayer videoSourcePlayer = new AForge.Controls.VideoSourcePlayer();
        private FilterInfoCollection videoDevices;
        private string sourceImage = string.Empty;
        private string currentImage = string.Empty;
        FisherFaceRecognizer recognizer = new FisherFaceRecognizer(10, 10.0);
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();//Images
        List<string> Names_List = new List<string>(); //labels
        List<int> Names_List_ID = new List<int>();
        private string playpath = string.Empty;
        private string directory = string.Empty;
        VideoCapture grabber;
        Image<Bgr, Byte> currentFrame; //current image aquired from webcam for display
                                       //  Image<Gray, byte> result, TrainedFace = null; //used to store the result image and trained face
                                       //   Image<Gray, byte> gray_frame = null; //grayscale current image aquired from webcam for processing
        private int facenum = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void buttoncompare_Click(object sender, EventArgs e)
        {
            try
            {
                //UpdateStatus(string.Format("check :{0}", 111));
                //var filename = Path.GetTempFileName() + ".jpg";
                //pictureBoxcurrentimage.Image.Save(filename);
                //UpdateStatus(string.Format("check :{0}", 222));
                //var res = recognizer.Predict(new Image<Gray,Byte>(filename));
                //UpdateStatus(string.Format("{0},Distance={1},Label={2},{3}", Environment.NewLine, res.Distance, res.Label, res));
            }
            catch (Exception ex)
            {
                UpdateStatus(string.Format("exception :{0}", ex.Message));
            }
        }
        private void UpdateStatus(string status)
        {
            richTextBox1.AppendText(Environment.NewLine + string.Format("{0}--{1}", DateTime.Now, status));
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (videoDevices.Count == 0)
                {
                    UpdateStatus("没有摄像头，无法开始拍照，请连接摄像头！");
                    return;
                }
                grabber = new VideoCapture();
                grabber.QueryFrame();

                Application.Idle += new EventHandler(FrameGrabber);
            }
            catch (ApplicationException)
            {
                UpdateStatus("No local capture devices");
            }
        }

        void FrameGrabber(object sender, EventArgs e)
        {
            try
            {
                using (currentFrame = grabber.QueryFrame().ToImage<Bgr, Byte>())
                {
                    if (currentFrame != null)
                    {
                        pictureBoxsource.Image = currentFrame.Bitmap;

                        if (HaveFace(currentFrame))
                        {
                            var filename = Path.GetTempFileName() + "haveface.jpg";
                            currentFrame.Save(filename);
                            pictureBoxcurrentimage.Image = currentFrame.Bitmap;
                            UpdateStatus(string.Format("high quality face photo captured,{0}", ++facenum));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus(string.Format("in FrameGrabber,{0}", ex.Message));
            }
        }
        bool HaveFace(Image<Bgr, Byte> fname)
        {
            long detectionTime;
            List<Rectangle> faces = new List<Rectangle>();
            List<Rectangle> eyes = new List<Rectangle>();
            DetectFace.Detect(
              fname, "haarcascade_frontalface_default.xml", "haarcascade_eye.xml",
              faces, eyes,
              out detectionTime);
            if (faces.Count == 1 && eyes.Count == 2) return true;

            return false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Application.Idle -= new EventHandler(FrameGrabber);
                grabber.Dispose();
                recognizer.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void buttonstopcapture_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Idle -= new EventHandler(FrameGrabber);
                grabber.Dispose();
            }
            catch (Exception ex)
            {
                UpdateStatus(string.Format("maybe no camera:{0}", ex));
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg == 0x112)
        //    {
        //        switch ((int)m.WParam)
        //        {
        //            //禁止双击标题栏关闭窗体  
        //            case 0xF063:
        //            case 0xF093:
        //                m.WParam = IntPtr.Zero;
        //                break;
        //            //禁止拖拽标题栏还原窗体  
        //            case 0xF012:
        //            case 0xF010:
        //                m.WParam = IntPtr.Zero;
        //                break;
        //            //禁止双击标题栏  
        //            case 0xf122:
        //                m.WParam = IntPtr.Zero;
        //                break;
        //            //禁止关闭按钮  
        //            case 0xF060:
        //                m.WParam = IntPtr.Zero;
        //                break;
        //            //禁止最大化按钮  
        //            case 0xf020:
        //                m.WParam = IntPtr.Zero;
        //                break;
        //            //禁止最小化按钮  
        //            case 0xf030:
        //                m.WParam = IntPtr.Zero;
        //                break;
        //            //禁止还原按钮  
        //            case 0xf120:
        //                m.WParam = IntPtr.Zero;
        //                break;
        //        }
        //    }
        //    base.WndProc(ref m);
        //}

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonrestart_Click(object sender, EventArgs e)
        {
            grabber = new VideoCapture();
            grabber.QueryFrame();
            Application.Idle += new EventHandler(FrameGrabber);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBoxcurrentimage_Click(object sender, EventArgs e)
        {

        }
    }
}
