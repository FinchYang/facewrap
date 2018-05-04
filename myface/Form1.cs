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
      

        [DllImport("face_recognition.dll")]
        private static extern string compare(string file1, string file2);
        [DllImport("face_recognition.dll")]//, BestFitMapping = true, CallingConvention = CallingConvention.ThisCall)]
        extern static int Addfunc(int a, int b);

       

        private delegate void UpdateStatusDelegate(string status);
        string FileNameId = string.Empty;
        string FileNameCapture = string.Empty;
        string dllpath = @"idr210sdk";
        ComparedInfo upload = new ComparedInfo { address = "temp", operatingagency = "hehe" };
        int continuouscapture = 0;
        string version = string.Empty;

        

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
        private Memid memid = new Memid { memoid = string.Empty, checkok = false };
        public class Memid
        {
            public string memoid { get; set; }
            public bool checkok { get; set; }
        }
        private void button1_Click111(object sender, EventArgs e)
        {

          

          
            //idInfo = comm.ParaseIDBuff(txtBuff);

          
           
        }
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

           
        

            //ThreadPool.QueueUserWorkItem(ar =>
            //{
            //    Thread.Sleep(1000);
            //    for (; ; )
            //{
            //    handleB.WaitOne();
            //   // BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("B:我是B,我已经顶替A开始运行了。") });
            //    Thread.Sleep(5000);
            //  //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("B:我的事情已经做完了，该让A搞搞了，休息一会。") });
            //    EventWaitHandle.SignalAndWait(handleA, handleB);
            // //   BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("B:hi,A我搞定了，下班了。") });
            //    handleA.Set();
            //    }
            //});
        }
        //EventWaitHandle handleA = new AutoResetEvent(false);
        //EventWaitHandle handleB = new AutoResetEvent(false);
        //Semaphore sempore = new Semaphore(0, 1);
        private object lockObj = new object();
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
                        
                      //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("捕捉一张脸。。。{0}", DateTime.Now) });
                        lock (lockObj)
                        {
                            FileNameCapture = Path.GetTempFileName() + "haveface.jpg";
                            _frame.Save(FileNameCapture);
                            pictureBoxcapture.BackgroundImage = _frame.Bitmap;
                        }

                        // BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("{0},{1}", ret.face.Width, ret.face.Height) });

                        //Bitmap img2 = new Bitmap(ret.face.Width, ret.face.Height, PixelFormat.Format24bppRgb);
                        //// img2.SetResolution(ret.face.Height, 180.0F);
                        //using (Graphics g = Graphics.FromImage(img2))
                        //{
                        //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        //    g.DrawImage(_frame.Bitmap, new Rectangle(0, 0, img2.Width, img2.Height), 0, 0, _frame.Bitmap.Width, _frame.Bitmap.Height, GraphicsUnit.Pixel);
                        //   // g.Dispose();

                        //    img2.Save(a, ImageFormat.Jpeg);
                        //}

                        //_frame.Bitmap.SetResolution(320, 180);
                        //  _frame.Save(FileNameCapture[continuouscapture]);
                       
                       
                        //stop.Restart();
                        //var ad = Addfunc(23423, 234234);
                        //stop.Stop();
                        //BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("addfunc--{0}-{1}-", ad, stop.ElapsedMilliseconds) });
                       // compareone(a);
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
                   // foreach (Rectangle face in faces)
                        CvInvoke.Rectangle(frame, faces[0], new Bgr(Color.Red).MCvScalar, 2);
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
        private void UpdateCheckok(string ok)
        {
            memid.checkok = bool.Parse(ok);
            UpdateStatus(ok);
        }
        private void UpdateResult(string ok)
        {
            textBoxresult.Text="%"+ok+"相似度";
            UpdateStatus(ok);
        }
        private void UpdateId(string ok)
        {
            textBoxid.Text = ok;
            UpdateStatus(ok);
        }
        private void UpdateNation(string ok)
        {
            textBoxnation.Text = ok;
            UpdateStatus(ok);
        }
        private void UpdateGender(string ok)
        {
            textBoxgender.Text = ok;
            UpdateStatus(ok);
        }
        private void UpdateName(string ok)
        {
            textBoxname.Text = ok;
            UpdateStatus(ok);
        }
        private void UpdateMemId(string id)
        {
            memid.memoid = id;
            UpdateStatus(id);
        }
        private void UpdateCaputureFile(string id)
        {
            FileNameCapture = string.Empty;
        }
        private void UpdateIdPhoto(string status)
        {
            pictureid.BackgroundImage=Image.FromFile(status);
            UpdateStatus(status);
        }
        private void UpdateCapturePhoto(string status)
        {
            FileNameCapture=string.Empty;
            UpdateStatus(status);
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
            //SetCallbackFunc(callback);
            //ctx.Form = this;
            //IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(ctx));
            //Marshal.StructureToPtr(ctx, ptr, false);
            //SetCallbackContext(ptr);

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
                ThreadPool.QueueUserWorkItem(ar =>
                {
                   
                    Thread.Sleep(1000);
                    

                    for (; ; )
                    {
                        Thread.Sleep(100);
                        // if (memid.memoid !=string.Empty && memid.checkok == true) continue;
                        var comm = new common();
                        int port = 0;  //自动搜索端口          
                        bool ret = comm.connectDev(port);
                        if (!ret)
                        {
                            comm.closeDev();
                            MessageBox.Show("连接设备失败", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }
                        ret = comm.findIDCard();
                        if (!ret)
                        {
                            comm.closeDev();
                            // BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("请把二代证放在读卡区 ")});
                            continue;
                        }

                        byte[] txtBuff = new byte[256];

                        ret = comm.readIDInfo(out txtBuff);//默认生成zp.bmp照片
                        if (!ret)
                        {
                            comm.closeDev();
                            BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("读取二代证信息失败  ") });
                            continue;
                        }
                       
                        var idInfo = comm.ParaseIdInfo();
                        comm.closeDev();

                        var name = idInfo.getName();
                        var id = idInfo.getId();
                        var gender = idInfo.getSex();
                        var folk = idInfo.getFolk();
                        BeginInvoke(new UpdateStatusDelegate(UpdateId), new object[] { id });
                        BeginInvoke(new UpdateStatusDelegate(UpdateNation), new object[] { folk });
                        BeginInvoke(new UpdateStatusDelegate(UpdateGender), new object[] { gender });
                        BeginInvoke(new UpdateStatusDelegate(UpdateName), new object[] { name });

                      
                        BeginInvoke(new UpdateStatusDelegate(UpdateIdPhoto), new object[] { "zp.bmp" });
                        while (FileNameCapture == string.Empty)
                        {
                            Thread.Sleep(100);
                        }


                        //lock (lockObj)
                        //{
                        //    var stop = new Stopwatch();
                        //    stop.Start();

                        //    var rrr = compare(FileNameCapture, "zp.bmp");
                        //    var ok = true;

                        //    BeginInvoke(new UpdateStatusDelegate(UpdateCapturePhoto), new object[] { "清空照片" });
                        //    BeginInvoke(new UpdateStatusDelegate(UpdateMemId), new object[] { id });
                        //    var reg = @"[\d]+";
                        //    var m = Regex.Match(rrr, reg);
                        //    stop.Stop();
                        //    var per = double.Parse(m.Value) / 100000;
                        //    BeginInvoke(new UpdateStatusDelegate(UpdateResult), new object[] { (per * 100).ToString() });

                        //    if (m.Success && per > 0.7)
                        //    {
                        //        BeginInvoke(new UpdateStatusDelegate(UpdateCaputureFile), new object[] { string.Empty });
                        //        BeginInvoke(new UpdateStatusDelegate(UpdateMemId), new object[] { string.Empty });
                        //        MessageBox.Show(stop.ElapsedMilliseconds + "比对成功，是同一个人" + m.Value);
                        //        ok = true;
                        //    }
                        //    else ok = false;
                        //    BeginInvoke(new UpdateStatusDelegate(UpdateCheckok), new object[] { ok.ToString() });

                        //    BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("similarity--{0}-{1}-", rrr, stop.ElapsedMilliseconds) });


                        //}
                       
                    }
                   
                });
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
                Process.GetCurrentProcess().Kill();//终止当前正在运行的线程
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
       
        private void button1_Click(object sender, EventArgs e)
        {
          //  Loop();
            UpdateStatus(string.Format("{0}", Addfunc(10, 33)));
            UpdateStatus(string.Format("UCommand1,{0}-", ClassComminterface.UCommand1(0x41, 0, "", "")));
        }
    }
}
