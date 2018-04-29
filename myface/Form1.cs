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
                _capture = new VideoCapture();
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
                        FileNameCapture[continuouscapture] = Path.GetTempFileName() + "haveface.jpg";
                        // BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("{0},{1}", ret.face.Width, ret.face.Height) });

                        Bitmap img2 = new Bitmap(ret.face.Width, ret.face.Height, PixelFormat.Format24bppRgb);
                        // img2.SetResolution(ret.face.Height, 180.0F);
                        using (Graphics g = Graphics.FromImage(img2))
                        {
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.DrawImage(_frame.Bitmap, new Rectangle(0, 0, img2.Width, img2.Height), 0, 0, _frame.Bitmap.Width, _frame.Bitmap.Height, GraphicsUnit.Pixel);
                           // g.Dispose();

                            img2.Save(FileNameCapture[continuouscapture], ImageFormat.Jpeg);
                        }

                        //_frame.Bitmap.SetResolution(320, 180);
                      //  _frame.Save(FileNameCapture[continuouscapture]);
                        switch (continuouscapture)
                        {
                            case 0:
                                pictureBoxcurrentimage.BackgroundImage = _frame.Bitmap;
                                break;
                            case 1:
                                picturecapture1.BackgroundImage = _frame.Bitmap;
                                break;
                            default:
                                picturecapture2.BackgroundImage = _frame.Bitmap;
                                break;
                        }
                        BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("照片抓取成功,{0}", continuouscapture) });
                        // UpdateStatus(string.Format("照片抓取成功,{0}", continuouscapture));
                        continuouscapture++;
                        if (continuouscapture > 2)
                        {
                            capturing = false;
                            _capture.Pause();
                            BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("3张照片抓取完成") });
                            //UpdateStatus(string.Format("3张照片抓取完成"));
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



        private void buttoncompare_Click(object sender, EventArgs e)
        {
            try
            {
                labelscore.Text = string.Empty;
                //if (FileNameId == string.Empty)
                //{
                //    MessageBox.Show(string.Format("请先读取身份证信息！"));
                //    return;
                //}
                if (continuouscapture < 3)
                {
                    MessageBox.Show(string.Format("请先等待人脸照片抓取成功！-{0}", continuouscapture));
                    return;
                }
                labeltip.Text = "正在比对中。。。";
                for (int i = 0; i < 3; i++)
                {
                    if (compareone(FileNameCapture[i], i + 1))
                    {
                        break;
                    }
                }
                labeltip.Text = "比对完成 ！";
                continuouscapture = 0;
                pictureBoxcurrentimage.BackgroundImage = null;

                picturecapture1.BackgroundImage = null;

                picturecapture2.BackgroundImage = null;
                pictureid.BackgroundImage = null;
                GC.Collect(111, GCCollectionMode.Forced);
            }
            catch (Exception ex)
            {
                UpdateStatus(string.Format("exception :{0},{1},{2}", FileNameCapture, FileNameId, ex.Message));
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
     

        bool compareone1(string capturefile, int index)
        {
            var stop = new Stopwatch();
            stop.Start();
            var a = new System.Diagnostics.Process();

            //a.StartInfo.UseShellExecute = false;
            //a.StartInfo.RedirectStandardOutput = true;
            //a.StartInfo.CreateNoWindow = true;

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

            var reg = @"(?<=photobig,)0\.[\d]{6,}";
            var m = Regex.Match(output, reg);
            stop.Stop();
            if (m.Success)
            {
                //   MessageBox.Show(stop.ElapsedMilliseconds+"比对成功，是同一个人" +m.Value);
                MessageBox.Show("比对成功，是同一个人" + m.Value);
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
      
        private void uploadinfo(object obj)
        {
            var ui = (ComparedInfo)obj;
            ui.capturephoto = File.ReadAllBytes(capturephotofile);
            ui.idphoto = File.ReadAllBytes(idphotofile);

            //  var url = string.Format("http://{0}/{1}", "localhost:5000", "PostCompared");
            var url = string.Format("http://{0}/{1}", host, action);// "api/Trails");// 
            try
            {
                //   BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", url) });

                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", 222) });
                using (var http = new HttpClient(handler))
                {
                    var content = new StringContent(JsonConvert.SerializeObject(ui));
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var hrm = http.PostAsync(url, content);
                    var response = hrm.Result;
                    // BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("capturefile.{0},{1}", capturephotofile, ui.capturephoto) });
                    string srcString = response.Content.ReadAsStringAsync().Result;
                    //   BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", srcString) });
                    BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", response.StatusCode) });
                    //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", hrm.Status) });
                }
            }
            catch (Exception ex)
            {
                BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", ex.Message) });
            }
        }

        private void UpdateIdInfo(string status)
        {
            richTextBox1.AppendText(status);
            richTextBox1.AppendText(Environment.NewLine );
        }
        private void UpdateIdInfoNoNewLine(string status)
        {
            richTextBox1.AppendText(status);
        }
     
        private void UpdateStatus(string status)
        {
        //    richTextBox1.AppendText(Environment.NewLine + string.Format("{0}--{1}", DateTime.Now, status));
        //    richTextBox1.SelectionStart = richTextBox1.Text.Length;
        //    richTextBox1.ScrollToCaret();
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
            textBoxname.Visible = false;
            textBoxid.Visible = false;
            buttongetresult.Visible = false;
            buttoncloudcompare.Visible = false;
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

                pictureBoxcurrentimage.BackgroundImage = null;

                picturecapture1.BackgroundImage = null;

                picturecapture2.BackgroundImage = null;



                //grabber = new VideoCapture();
                //grabber.QueryFrame();
                //Application.Idle += new EventHandler(FrameGrabber);
                capturing = true;

            }
            catch (Exception ex)
            {
                UpdateStatus(string.Format("restart:{0}", ex));
            }
        }
        private void buttonreadid_Click(object sender, EventArgs e)
        {
            //int ret;
            //int iPort = 1;
            //try
            //{
            //    // var nm=new NativeMethods();
            //    ret = InitComm(iPort);
            //    if (ret != 0)
            //    {
            //        var ok = true;
            //        do
            //        {
            //            ret = Authenticate();
            //            if (ret != 0)
            //            {
            //                ok = false;

            //                var Msg = new byte[200];
            //              //  var aa = 0;
            //                ret = ReadBaseMsg(Msg,   0);
            //                if (ret > 0)
            //                {
            //                    richTextBox1.Clear();
            //                    upload.name = System.Text.Encoding.Default.GetString(Msg.Take(31).ToArray());
            //                    upload.gender = System.Text.Encoding.Default.GetString(Msg.Skip(31).Take(3).ToArray());
            //                    upload.nation = System.Text.Encoding.Default.GetString(Msg.Skip(34).Take(10).ToArray());
            //                    upload.birthday = System.Text.Encoding.Default.GetString(Msg.Skip(44).Take(9).ToArray());
            //                    upload.idaddress = Encoding.Default.GetString(Msg.Skip(53).Take(71).ToArray());
            //                    upload.id = System.Text.Encoding.Default.GetString(Msg.Skip(124).Take(19).ToArray());
            //                    upload.issuer = System.Text.Encoding.Default.GetString(Msg.Skip(143).Take(31).ToArray());
            //                    upload.startdate = System.Text.Encoding.Default.GetString(Msg.Skip(174).Take(9).ToArray());
            //                    upload.enddate = Encoding.Default.GetString(Msg.Skip(183).Take(9).ToArray());
            //                  //  var aaa = Encoding.Default.GetString(Msg.Skip(174).Take(18).ToArray());
            //                    UpdateIdInfo(string.Format("姓名："+upload.name));                               
            //                    UpdateIdInfo(string.Format("性别："+upload.gender));                               
            //                    UpdateIdInfo(string.Format("民族："+upload.nation));                                
            //                    UpdateIdInfo(string.Format("住址：" + upload.idaddress));                               
            //                    UpdateIdInfo(string.Format("出生日期：{0}",upload.birthday));                               
            //                    UpdateIdInfo(string.Format("证件所属：" + upload.issuer));                                
            //                    UpdateIdInfo(string.Format("公民身份号码："+upload.id));

            //                    UpdateIdInfoNoNewLine("身份证有效期：");
            //                    UpdateIdInfoNoNewLine(string.Format(upload.startdate));
            //                    UpdateIdInfoNoNewLine("-");
            //                    UpdateIdInfo(string.Format(upload.enddate));

            //                    var FileNameIdtmp = Path.Combine(homepath, dllpath, "photo.bmp");
            //                    FileNameId = Path.Combine(homepath, dllpath, "photobig.jpg");
            //                    idphotofile = FileNameId;
            //                    using (var jpg = new Bitmap(FileNameIdtmp))
            //                    {
            //                        jpg.Save(FileNameId, ImageFormat.Jpeg);
            //                    }
            //                    //  Image.FromFile(FileNameIdtmp).Save(FileNameId, ImageFormat.Jpeg);
            //                    //using (var img = image.fromfile(filenameid))
            //                    //{
            //                    //    pictureid.backgroundimage = img;
            //                    //}
            //                    pictureid.BackgroundImage = Image.FromFile(FileNameId);
            //                    UpdateStatus(string.Format("身份证信息读取成功"));
            //                }
            //            }
            //            else
            //            {
            //                UpdateStatus(string.Format("请将身份证放在读卡器上 !"));
            //                System.Threading.Thread.Sleep(500);
            //                continue;
            //            }
            //        } while (ok);
            //    }

            //    ret = CloseComm();
            //}
            //catch (Exception ex)
            //{
            //    UpdateStatus(string.Format("身份证读卡器操作--{0} !", ex.Message));
            //}
        }

      

        private void button1_Click(object sender, EventArgs e)//无证件上传云端比对
        {
            if (textBoxid.Text.Length < 15)
            {
                MessageBox.Show(string.Format("请输入正确的身份证号！"));
                return;
            }
            if (continuouscapture < 3)
            {
                MessageBox.Show(string.Format("请先等待人脸照片抓取成功！-{0}", continuouscapture));
                return;
            }
            var ui = new NoidInput();
            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0:
                        ui.pic1 = File.ReadAllBytes(FileNameCapture[i]);
                        break;
                    case 1:
                        ui.pic2 = File.ReadAllBytes(FileNameCapture[i]);
                        break;
                    default:
                        ui.pic3 = File.ReadAllBytes(FileNameCapture[i]);
                        break;
                }
            }
            ui.id = textBoxid.Text;
            ui.name = textBoxname.Text;

            var url = string.Format("http://{0}/{1}", host, "NoidUpload");
            try
            {
                // 

                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                //      BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", 222) });
                using (var http = new HttpClient(handler))
                {
                    //    BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", 333) });
                    var content = new StringContent(JsonConvert.SerializeObject(ui));
                    //   BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", 444) });
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var hrm = http.PostAsync(url, content);
                    var response = hrm.Result;
                    string srcString = response.Content.ReadAsStringAsync().Result;
                    //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("NoidUpload.{0},", srcString) });
                    BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("NoidUpload.{0},", response.StatusCode) });
                    //   BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("NoidUpload.{0},", hrm.Status) });
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        continuouscapture = 0;
                        pictureBoxcurrentimage.BackgroundImage = null;

                        picturecapture1.BackgroundImage = null;

                        picturecapture2.BackgroundImage = null;
                    }
                }
                MessageBox.Show(string.Format("成功！"));
            }
            catch (Exception ex)
            {
                BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("NoidUpload.{0},", ex.Message) });
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

        private void buttonnoid_Click(object sender, EventArgs e)
        {
            try
            {
               buttonnoid. BackgroundImage = Image.FromFile("image/rlsb_wsfz_you.png");
                buttonhaveid.BackgroundImage = Image.FromFile("image/rlsb_ysfz.png");
                textBoxname.Text = string.Empty;
                textBoxid.Text = string.Empty;
                labeltip.Text = string.Empty;
                labelscore.Text = string.Empty;
                textBoxname.Visible = true;
                textBoxid.Visible = true;
                buttongetresult.Visible = true;
                buttoncloudcompare.Visible = true;


                pictureid.Visible = false;
                buttoncompare.Visible = false;
                buttonreadid.Visible = false;
                BackgroundImage = Image.FromFile("image/rlsb_wsf.png");
                richTextBox1.Clear();

            }
            catch (Exception ex)
            {
                UpdateStatus(string.Format("buttonnoid_Click:{0}", ex.Message));
            }
        }

        private void buttonhaveid_Click(object sender, EventArgs e)
        {
            try
            {
                buttonnoid.BackgroundImage = Image.FromFile("image/rlsb_wsfz.png");
                buttonhaveid.BackgroundImage = Image.FromFile("image/rlsb_ysfz_you.png");

                labeltip.Text = string.Empty;
                labelscore.Text = string.Empty;
                pictureid.Visible = true;
                buttoncompare.Visible = true;
                buttonreadid.Visible = true;


                textBoxname.Visible = false;
                textBoxid.Visible = false;
                buttongetresult.Visible = false;
                buttoncloudcompare.Visible = false;
              BackgroundImage = Image.FromFile("image/rlsb_ysf.png");
            }
            catch (Exception ex)
            {
                UpdateStatus(string.Format("haveid:{0}", ex.Message));
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
