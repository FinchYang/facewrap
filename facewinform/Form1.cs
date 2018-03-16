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
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Configuration;

namespace face
{
    public partial class Form1 : Form
    {
        private delegate void UpdateStatusDelegate(string status);
        string FileNameId = string.Empty;
        string[] FileNameCapture =new string[] { string.Empty, string.Empty, string.Empty };
        string dllpath = @"idr210sdk";
        ComparedInfo upload = new ComparedInfo { address="temp",operatingagency="hehe"};
        int continuouscapture = 0;
        [DllImport(@"idr210sdk\sdtapi.dll")]
        public extern static int InitComm(int iPort);
        [DllImport(@"idr210sdk\sdtapi.dll")]
        public extern static int CloseComm();
        [DllImport(@"idr210sdk\sdtapi.dll")]
        public extern static int Routon_IC_HL_ReadCardSN(StringBuilder SN);
        [DllImport(@"idr210sdk\sdtapi.dll")]
        public extern static int Routon_IC_HL_ReadCard(int SID, int BID, int KeyType, byte[] Key, byte[] data);

        [DllImport(@"idr210sdk\sdtapi.dll")]
        public extern static int Authenticate();

        [DllImport(@"idr210sdk\sdtapi.dll")]
        public extern static int ReadBaseMsg(byte[] pMsg, int len);

        [DllImport(@"idr210sdk\sdtapi.dll")]
        public extern static int ReadNewAppMsg(byte[] pMsg, out int len);  

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
        string homepath = string.Empty;
        string host = string.Empty;
        string action = string.Empty;
         string idphotofile = string.Empty;
        string capturephotofile = string.Empty;
        public Form1()
        {
            InitializeComponent();
            homepath = Environment.CurrentDirectory;
            host = ConfigurationManager.AppSettings["host"];
            action = ConfigurationManager.AppSettings["action"];
        }
        private void buttoncompare_Click(object sender, EventArgs e)
        {
            try
            {
                if(FileNameId == string.Empty)
                {
                    UpdateStatus(string.Format("请先读取身份证信息！"));
                    return;
                }
               if(continuouscapture<3)
                {
                    UpdateStatus(string.Format("请先等待人脸照片抓取成功！-{0}",continuouscapture));
                    return;
                }
               for(int i = 0; i < 3; i++)
                {
                    if (compareone(FileNameCapture[i],i+1))
                    {
                        break;
                    }
                }
                continuouscapture = 0;
            }
            catch (Exception ex)
            {
                UpdateStatus(string.Format("exception :{0},{1},{2}", FileNameCapture, FileNameId, ex.Message));
            }
        }
        bool compareone(string capturefile,int index)
        {
            var stop = new Stopwatch();
            stop.Start();
            var a = new System.Diagnostics.Process();
            a.StartInfo.UseShellExecute = false;
            a.StartInfo.WorkingDirectory = Path.Combine(homepath, "compare");
            a.StartInfo.CreateNoWindow = true;
            a.StartInfo.Arguments = string.Format(" {0} {1}", capturefile, FileNameId);
            capturephotofile = capturefile;
            a.StartInfo.FileName = Path.Combine(homepath, "compare", "FaceCompareCon.exe");
            a.Start();
            a.WaitForExit();
            stop.Stop();
            UpdateStatus(string.Format("time elapsed:{0}", stop.ElapsedMilliseconds));
            var result = JsonConvert.DeserializeObject<result>(File.ReadAllText(Path.Combine(homepath, "compare", "compareresult.txt")));
            UpdateStatus(string.Format("result score:{0}", result.score));

            if (result.ok)
            {
                switch (result.status)
                {
                    case CompareStatus.unkown:
                        break;
                    case CompareStatus.success:
                        UpdateStatus(string.Format("第{1}张照片对比是同一个人，WARNING_VALUE={0}", 73,index));
                        var th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(uploadinfo));
                        th.Start(upload);
                        
                        MessageBox.Show("比对成功，是同一个人");
                        FileNameId = string.Empty;
                        break;
                    case CompareStatus.failure:
                        UpdateStatus(string.Format("第{1}张照片对比不是同一个人，WARNING_VALUE={0}", 73, index));
                        break;
                    case CompareStatus.uncertainty:
                        UpdateStatus(string.Format("第{1}张照片不确定是否同一人，WARNING_VALUE={0}", 73, index));
                        break;
                    default:
                        break;
                }
            }
            else
            {
                UpdateStatus(string.Format("第{1}张照片比对出错，{0}", result.errcode, index));
            }

            if (result.ok && result.status == CompareStatus.success) return true;
            else return false;            
        }

        private void uploadinfo(object obj)
        {
            var ui = (ComparedInfo)obj;
            ui.capturephoto = File.ReadAllBytes(capturephotofile);
            ui.idphoto = File.ReadAllBytes(idphotofile);
            
            var url = string.Format("http://{0}/{1}", host, action);
            try
            {
              //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", 111) });

                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
             //   BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", 222) });
                using (var http = new HttpClient(handler))
                {
                  //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", 333) });
                    var content = new StringContent(JsonConvert.SerializeObject(ui));
                  //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", 444) });
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var hrm = http.PostAsync(url, content);
                    var response = hrm.Result;
                    BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("capturefile.{0},{1}", capturephotofile, ui.capturephoto) });
                    string srcString = response.Content.ReadAsStringAsync().Result;
                    BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", srcString) });
                    BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", response.StatusCode) });
                    BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", hrm.Status) });
                }
            }
            catch (Exception ex)
            {
                BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", ex.Message) });
            }
        }

        public struct FaceFile
        {
            public byte[] fcontent;
            public int flen;
        }
        static FaceFile freadAll(string fname)
        {
            FaceFile ret = new FaceFile();
            ret.flen = 0;
            if (!File.Exists(fname))
            {
                Console.WriteLine("file %s not exist.\n", fname);
                return ret;
            }

            ret.fcontent = File.ReadAllBytes(fname);
            ret.flen = ret.fcontent.Length;
            return ret;
        }
        static int ShowReturnCode(int code)
        {
            switch (code)
            {
                case -1:
                    Console.WriteLine("interal error");
                    break;
                case -2:
                    Console.WriteLine("apply for memory error");
                    break;
                case -3:
                    Console.WriteLine("picture format error");
                    break;
                case -4:
                    Console.WriteLine("parameter error");
                    break;
                case -5:
                    Console.WriteLine("resolution ratio >1080P");
                    break;
                case -6:
                    Console.WriteLine("no face");
                    break;
                case -7:
                    Console.WriteLine("multiple face \r\n");
                    break;
                default:
                    break;
            }
            return code;
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
                            FileNameCapture[continuouscapture] = Path.GetTempFileName() + "haveface.jpg";
                            currentFrame.Save(FileNameCapture[continuouscapture]);
                            switch (continuouscapture)
                            {
                                case 0:
                                    pictureBoxcurrentimage.Image = currentFrame.Bitmap;
                                    break;
                                case 1:
                                    picturecapture1.Image = currentFrame.Bitmap;
                                    break;
                                default:
                                    picturecapture2.Image = currentFrame.Bitmap;
                                    break;
                            }
                            
                            UpdateStatus(string.Format("照片抓取成功,{0}", continuouscapture));
                            continuouscapture++;
                            if (continuouscapture > 2) {
                                Application.Idle -= new EventHandler(FrameGrabber);
                                grabber.Dispose();
                                UpdateStatus(string.Format("3张照片抓取完成"));
                            }                           
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

        private void buttonreadid_Click(object sender, EventArgs e)
        {
            int ret;
            int iPort = 1;

            ret = InitComm(iPort);
            if (ret != 0)
            {
                var ok = true;
                do
                {
                    ret = Authenticate();
                    if (ret != 0)
                    {
                        ok = false;

                        var Msg = new byte[200];
                        ret = ReadBaseMsg(Msg, 0);
                        if (ret > 0)
                        {
                            upload.name = System.Text.Encoding.Default.GetString(Msg.Take(31).ToArray());
                            UpdateStatus(string.Format(upload.name));
                            upload.gender = System.Text.Encoding.Default.GetString(Msg.Skip(31).Take(3).ToArray());
                            UpdateStatus(string.Format(upload.gender));
                            upload.nation = System.Text.Encoding.Default.GetString(Msg.Skip(34).Take(10).ToArray());
                            UpdateStatus(string.Format(upload.nation));
                            upload.birthday = System.Text.Encoding.Default.GetString(Msg.Skip(44).Take(9).ToArray());
                            UpdateStatus(string.Format(upload.birthday));
                            upload.idaddress = Encoding.Default.GetString(Msg.Skip(53).Take(71).ToArray());
                            UpdateStatus(string.Format(upload.idaddress));
                            upload.id = System.Text.Encoding.Default.GetString(Msg.Skip(124).Take(19).ToArray());
                            UpdateStatus(string.Format(upload.id));
                            upload.issuer = System.Text.Encoding.Default.GetString(Msg.Skip(143).Take(31).ToArray());
                            UpdateStatus(string.Format(upload.issuer));
                            upload.startdate = System.Text.Encoding.Default.GetString(Msg.Skip(174).Take(9).ToArray());
                            UpdateStatus(string.Format(upload.startdate));
                            upload.enddate = Encoding.Default.GetString(Msg.Skip(183).Take(9).ToArray());
                            UpdateStatus(string.Format(upload.enddate));
                            var FileNameIdtmp = Path.Combine(homepath, dllpath, "photo.bmp");
                            FileNameId = Path.GetTempFileName();
                            idphotofile = FileNameId;
                            using(var jpg=new Bitmap(FileNameIdtmp))
                            {
                                jpg.Save(FileNameId, ImageFormat.Jpeg);
                            }
                          //  Image.FromFile(FileNameIdtmp).Save(FileNameId, ImageFormat.Jpeg);
                            pictureid.Image = Image.FromFile(FileNameId);
                            UpdateStatus(string.Format("身份证信息读取成功"));
                        }
                    }
                    else
                    {
                        UpdateStatus(string.Format("请将身份证放在读卡器上:{0}", "please put your id card"));
                        System.Threading.Thread.Sleep(100);
                        continue;
                    }
                } while (ok);
            }

            ret = CloseComm();
        }

        private void pictureBoxsource_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer4_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (continuouscapture < 3)
            {
                UpdateStatus(string.Format("请先等待人脸照片抓取成功！-{0}", continuouscapture));
                return;
            }
            var ui = new NoidInput();
            for(int i = 0; i < 3; i++)
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
                //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", 111) });

                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                //   BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", 222) });
                using (var http = new HttpClient(handler))
                {
                    //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", 333) });
                    var content = new StringContent(JsonConvert.SerializeObject(ui));
                    //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", 444) });
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var hrm = http.PostAsync(url, content);
                    var response = hrm.Result;
                    string srcString = response.Content.ReadAsStringAsync().Result;
                    BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("NoidUpload.{0},", srcString) });
                    BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("NoidUpload.{0},", response.StatusCode) });
                    BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("NoidUpload.{0},", hrm.Status) });
                }
            }
            catch (Exception ex)
            {
                BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("NoidUpload.{0},", ex.Message) });
            }
        }
    }
}
