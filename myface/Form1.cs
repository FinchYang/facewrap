using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using AForge.Video.DirectShow;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Configuration;
using System.Net.Http;
using System.Threading;
using System.Reflection;
using face.model;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Text.RegularExpressions;

namespace face
{

    public partial class FormFace : Form
    {
        private delegate void UpdateStatusDelegate(string status);
        string FileNameId = string.Empty;
        string[] FileNameCapture = new string[] { string.Empty, string.Empty, string.Empty };
        string dllpath = @"idr210sdk";
        ComparedInfo upload = new ComparedInfo { address = "temp", operatingagency = "hehe" };
        int continuouscapture = 0;
        string version = string.Empty;

        //[DllImport(@"idr210sdk\sdtapi.dll")]
        //public extern static int InitComm(int iPort);
        //[DllImport(@"idr210sdk\sdtapi.dll")]
        //public extern static int CloseComm();
        //[DllImport(@"idr210sdk\sdtapi.dll")]
        //public extern static int Authenticate();

        //[DllImport(@"idr210sdk\sdtapi.dll")]
        //public extern static int ReadBaseMsg(byte[] pMsg, int len);
        //[DllImport(@"idr210sdk\sdtapi.dll")]
        //public extern static int ReadIINSNDN(string pMsg);
        //[DllImport(@"idr210sdk\sdtapi.dll")]
        //public extern static int GetSAMIDToStr(string pMsg);
        private FilterInfoCollection videoDevices;
        private string sourceImage = string.Empty;
        private string currentImage = string.Empty;
        //   FisherFaceRecognizer recognizer = new FisherFaceRecognizer(10, 10.0);
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();//Images
        List<string> Names_List = new List<string>(); //labels
        List<int> Names_List_ID = new List<int>();
        private string playpath = string.Empty;
        private string directory = string.Empty;
        string homepath = string.Empty;
        string host = string.Empty;
        string action = string.Empty;
        string idphotofile = string.Empty;
        string capturephotofile = string.Empty;
        bool capturing = true;
        private VideoCapture _capture = null;
      //  private bool _captureInProgress;
        private Mat _frame;
        public FormFace()
        {
            InitializeComponent();
            homepath = Environment.CurrentDirectory;
            host = ConfigurationManager.AppSettings["host"];
            action = ConfigurationManager.AppSettings["action"];
            CvInvoke.UseOpenCL = false;
            try
            {
                _capture = new VideoCapture(0);
                _capture.ImageGrabbed += ProcessFrame;
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
            _frame = new Mat();
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                _capture.Retrieve(_frame, 0);
                // picturecapture1.BackgroundImage = null;
                pictureBoxsource.BackgroundImage = _frame.Bitmap;
                try
                {
                    //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("请让客户摆正头部位置，自动抓拍并检测照片质量。。。") });
                  //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("{0},{1},{2}",_frame.Bitmap.Size,_frame.Bitmap.Height, _frame.Bitmap.Width) });
                    var ret = detectfacethreadex(_frame);
                        if (ret.ok)
                        //   if (HaveFace(currentFrame))
                        {
                        var a = Path.GetTempFileName() + "haveface.jpg";
                        // BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("{0},{1}", ret.face.Width, ret.face.Height) });

                        Bitmap img2 = new Bitmap(ret.face.Width, ret.face.Height, PixelFormat.Format24bppRgb);
                        // img2.SetResolution(ret.face.Height, 180.0F);
                        using (Graphics g = Graphics.FromImage(img2))
                        {
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.DrawImage(_frame.Bitmap, new Rectangle(0, 0, img2.Width, img2.Height), 0, 0, _frame.Bitmap.Width, _frame.Bitmap.Height, GraphicsUnit.Pixel);
                           // g.Dispose();

                            img2.Save(a, ImageFormat.Jpeg);
                        }

                        //_frame.Bitmap.SetResolution(320, 180);
                        //  _frame.Save(FileNameCapture[continuouscapture]);
                        if (compareone(a))
                        {

                        }
                    }
                    GC.Collect(111,GCCollectionMode.Forced);

                }
                catch (Exception ex)
                {
                    BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("in ProcessFrame,{0}", ex.Message) });
                }
            }
        }



   
        public class facer
        {
            public bool ok { get; set; }
            public Rectangle face { get; set; }
        }
        private facer detectfacethreadex(Mat frame)
        {
            var ret = new facer { ok = false };

            try
            {
                long detectionTime;
                List<Rectangle> faces = new List<Rectangle>();
                List<Rectangle> eyes = new List<Rectangle>();
                DetectFace.Detect(
                  frame, "haarcascade_frontalface_default.xml", "haarcascade_eye.xml",
                  faces, eyes,
                  out detectionTime);
                if (faces.Count == 1 && eyes.Count == 2)
                {
                    ret.ok = true;
                    ret.face = faces[0];
                    return ret;
                }

                return ret;
            }
            catch (Exception ex)
            {
                BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("in detectfacethread,{0}", ex.Message) });
            }
            return ret;
        }

        bool compareone(string capturefile)
        {
            var stop = new Stopwatch();
            stop.Start();
            var a = new System.Diagnostics.Process();

            a.StartInfo.UseShellExecute = false;
            a.StartInfo.RedirectStandardOutput = true;
            a.StartInfo.CreateNoWindow = true;

            a.StartInfo.WorkingDirectory = homepath;
            FileNameId = Path.Combine(homepath, "idr210sdk", "photo.bmp");
            a.StartInfo.Arguments = string.Format(" {0} {1}", FileNameId, capturefile);
            //   UpdateStatus(string.Format("files:{0}", a.StartInfo.Arguments));
            capturephotofile = capturefile;
            a.StartInfo.FileName = Path.Combine(homepath, "compare.exe");
            //  a.StartInfo.FileName = Path.Combine(homepath, "compare", "ccompare.exe");
            a.Start();
            var output = a.StandardOutput.ReadToEnd();

            a.WaitForExit();
            var ret = a.ExitCode;

            var reg = @"(?<=terminate)0\.[\d]{4,}";
            var m = Regex.Match(output, reg);
            stop.Stop();
            BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { stop.ElapsedMilliseconds + "-" + m.Success + "--" + m.Value + output });
            if (m.Success && double.Parse(m.Value) < 0.499)
            {
                MessageBox.Show(stop.ElapsedMilliseconds + "比对成功，是同一个人" + m.Value);
                //    MessageBox.Show( "比对成功，是同一个人" + m.Value);
                FileNameId = string.Empty;
                return true;
            }
            else return false;


        }

        bool compareone(string capturefile, int index)
        {
            var stop = new Stopwatch();
            stop.Start();
            var a = new System.Diagnostics.Process();

            a.StartInfo.UseShellExecute = false;
            a.StartInfo.RedirectStandardOutput = true;
            a.StartInfo.CreateNoWindow = true;

            a.StartInfo.WorkingDirectory = homepath;
            FileNameId = Path.Combine(homepath, "idr210sdk", "photo.bmp");
            a.StartInfo.Arguments = string.Format(" {0} {1}", FileNameId, capturefile);
            //   UpdateStatus(string.Format("files:{0}", a.StartInfo.Arguments));
            capturephotofile = capturefile;
            a.StartInfo.FileName = Path.Combine(homepath, "compare.exe");
            //  a.StartInfo.FileName = Path.Combine(homepath, "compare", "ccompare.exe");
            a.Start();
           var output= a.StandardOutput.ReadToEnd();
         
            a.WaitForExit();
            var ret = a.ExitCode;

            var reg = @"(?<=terminate)0\.[\d]{4,}";
            var m = Regex.Match(output, reg);
              stop.Stop();
            BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { stop.ElapsedMilliseconds +"-"+m.Success+ "--" + m.Value+output });
            if (m.Success&&double.Parse(m.Value)<0.499)
            {
                MessageBox.Show(stop.ElapsedMilliseconds+"比对成功，是同一个人" +m.Value);
            //    MessageBox.Show( "比对成功，是同一个人" + m.Value);
                FileNameId = string.Empty;
                return true;
            }
            else return false;

         
        }
      

      
     
        private void UpdateStatus(string status)
        {
            richTextBox1.AppendText(Environment.NewLine + string.Format("{0}--{1}", DateTime.Now, status));
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }
        private void ReleaseData()
        {
            if (_frame != null)
                _frame.Dispose();
            if (_capture != null)
                _capture.Dispose();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
         
            try
            {
                version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                labelversion.Text = " 版本 ： " + version;
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (videoDevices.Count == 0)
                {
                    UpdateStatus("没有摄像头，无法开始拍照，请连接摄像头！");
                    return;
                }
                //grabber = new VideoCapture();
                //grabber.QueryFrame();

                //Application.Idle += new EventHandler(FrameGrabber);
                _capture.Start();
              
            }
            catch (ApplicationException aex)
            {
                UpdateStatus("No local capture devices: " + aex.Message);
            }
            catch (Exception ex)
            {
                UpdateStatus("No local capture devices--" + ex.Message);
            }
        }

    

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        private void buttonstopcapture_Click(object sender, EventArgs e)
        {
            //   UpdateStatus(string.Format("stop click"));
            try
            {
                if (_capture != null)
                {
                    _capture.Pause();
                }
                capturing = false;
                //Application.Idle -= new EventHandler(FrameGrabber);
                //grabber.Dispose();
            }
            catch (Exception ex)
            {
                UpdateStatus(string.Format("stopcapture:{0}", ex));
            }
        }
        private void buttonrestart_Click(object sender, EventArgs e)
        {

            try
            {
                if (_capture != null)
                {
                    _capture.Start();
                }
                if (capturing) return;
                //  UpdateStatus(string.Format("restart click"));
                continuouscapture = 0;

           
                capturing = true;

            }
            catch (Exception ex)
            {
                UpdateStatus(string.Format("restart:{0}", ex));
            }
        }
      
      

     
        private void buttongetresult_Click(object sender, EventArgs e)
        {
            var url = string.Format("http://{0}/{1}?businessnumber={2}", host, "GetNoidResult", "demo");
            try
            {
                //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("GetNoidResult--11.{0},", url) });
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                using (var http = new HttpClient(handler))
                {
                    //var content = new StringContent(JsonConvert.SerializeObject(new NoidResultInput { businessnumber="demo"}));
                    //content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var hrm = http.GetAsync(url);
                    var response = hrm.Result;
                    string srcString = response.Content.ReadAsStringAsync().Result;
                    //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("GetNoidResult.{0},", srcString) });
                    BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("GetNoidResult.{0},", response.StatusCode) });
                    //   BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("GetNoidResult.{0},", hrm.Status) });
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //continuouscapture = 0; 0-no result,1-success,2-failure,3-uncertainty
                        var a = JsonConvert.DeserializeObject<List<NoidResult>>(srcString);
                        foreach (var b in a)
                        {
                            BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("id.{0},{1}", b.id, b.status) });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("GetNoidResult.{0},", ex.Message) });
            }
        }

    

       

        private void buttonclose_Click(object sender, EventArgs e)
        {
            try
            {
              //  CloseComm();
                _capture.Dispose();
                _frame.Dispose();
            }
            catch (Exception) { }
            Close();
        }

        private void buttonmin_Click(object sender, EventArgs e)
        {
            // this.Visible = false;
            //  ShowInTaskbar = true;
            //   Hide();
            WindowState = FormWindowState.Minimized;
        }

        private void pictureBoxcurrentimage_Paint(object sender, PaintEventArgs e)
        {
            PictureBox p = (PictureBox)sender;
          //  Color cc = new Color();
            Pen pp = new Pen(Color.FromArgb(216,216,216));
            e.Graphics.DrawRectangle(pp, e.ClipRectangle.X, e.ClipRectangle.Y,
 e.ClipRectangle.X + e.ClipRectangle.Width - 1,
e.ClipRectangle.Y + e.ClipRectangle.Height - 1);
        }

      

        private void picturecapture1_Paint(object sender, PaintEventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            Pen pp = new Pen(Color.FromArgb(216, 216, 216));
            e.Graphics.DrawRectangle(pp, e.ClipRectangle.X, e.ClipRectangle.Y,
 e.ClipRectangle.X + e.ClipRectangle.Width - 1,
e.ClipRectangle.Y + e.ClipRectangle.Height - 1);
        }

        private void picturecapture2_Paint(object sender, PaintEventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            Pen pp = new Pen(Color.FromArgb(216, 216, 216));
            e.Graphics.DrawRectangle(pp, e.ClipRectangle.X, e.ClipRectangle.Y,
 e.ClipRectangle.X + e.ClipRectangle.Width - 1,
e.ClipRectangle.Y + e.ClipRectangle.Height - 1);
        }

        private void buttonnoid_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormFace_Layout(object sender, LayoutEventArgs e)
        {

        }

        private void FormFace_Paint(object sender, PaintEventArgs e)
        {
//            var p = (Form)sender;
//            Pen pp = new Pen(Color.Red);
//            e.Graphics.DrawRectangle(pp, e.ClipRectangle.X+1, e.ClipRectangle.Y+1,
// e.ClipRectangle.X + e.ClipRectangle.Width - 4,
//e.ClipRectangle.Y + e.ClipRectangle.Height - 4);
        }
    }
}
