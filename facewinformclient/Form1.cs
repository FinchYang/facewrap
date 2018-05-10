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
        private string FileNameId = string.Empty;
        private string[] FileNameCapture = new string[] { string.Empty, string.Empty, string.Empty };
        private ComparedInfo upload = new ComparedInfo { address = "temp", operatingagency = "hehe" };
        private int continuouscapture = 0;
        private string version = string.Empty;
        private double _score = 0.74;
        private bool needReadId = true;
        [DllImport(@"idr210sdk\sdtapi.dll")]
        private extern static int ReadBaseMsgPhoto(byte[] pMsg, ref int len, string directory);

        [DllImport(@"idr210sdk\sdtapi.dll")]
        private extern static int InitComm(int iPort);
        [DllImport(@"idr210sdk\sdtapi.dll")]
        private extern static int CloseComm();
        [DllImport(@"idr210sdk\sdtapi.dll")]
        private extern static int Authenticate();
        //[DllImport(@"idr210sdk\sdtapi.dll")]
        //public extern static int ReadBaseMsg(byte[] pMsg, ref int len);
        //[DllImport(@"idr210sdk\sdtapi.dll")]
        //public extern static int ReadIINSNDN(string pMsg);
        //[DllImport(@"idr210sdk\sdtapi.dll")]
        //public extern static int GetSAMIDToStr(string pMsg);
        //[DllImport(@"idr210sdk\sdtapi.dll")]
        //public extern static int ReadNewAppMsg(byte[] pMsg, out int len);
        private Thread _tCheckSelfUpdate;//,_tReadId;
                                         //  private FilterInfoCollection videoDevices;
                                         //  private string sourceImage = string.Empty;
                                         //  private string currentImage = string.Empty;
                                         //   FisherFaceRecognizer recognizer = new FisherFaceRecognizer(10, 10.0);
                                         //List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();//Images
                                         //List<string> Names_List = new List<string>(); //labels
                                         // List<int> Names_List_ID = new List<int>();
                                         //  private string playpath = string.Empty;
                                         //  private string directory = string.Empty;
                                         //   VideoCapture grabber;
                                         //   Image<Bgr, Byte> currentFrame; //current image aquired from webcam for display
                                         //  Image<Gray, byte> result, TrainedFace = null; //used to store the result image and trained face
                                         //   Image<Gray, byte> gray_frame = null; //grayscale current image aquired from webcam for processing
                                         //   private int facenum = 0;
        private string homepath = string.Empty;
        private string host = string.Empty;
        private string cloudcompare = string.Empty;
        private string action = string.Empty;
        private string capturephotofile = string.Empty;
        private bool capturing = true;//抓拍标志
        private VideoCapture _capture = null;
        //  private bool _captureInProgress;
        private Mat _frame;
        private string lipath = string.Empty;
        public FormFace()
        {
            host = ConfigurationManager.AppSettings["host"];
            cloudcompare = ConfigurationManager.AppSettings["cloudcompare"];
            InitializeComponent();
            homepath = Environment.CurrentDirectory;

            lipath = Path.Combine(homepath, "what");
            if (!Directory.Exists(lipath)) Directory.CreateDirectory(lipath);

           
            action = ConfigurationManager.AppSettings["action"];
            CvInvoke.UseOpenCL = false;//不适用GPU

            _frame = new Mat();
            try
            {
                _capture = new VideoCapture();
                //    _capture = new VideoCapture(1);
                _capture.ImageGrabbed += ProcessFrame;
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            //lock (lockObj)
            //{
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                _capture.Retrieve(_frame, 0);
                pictureBoxsource.BackgroundImage = _frame.Bitmap;
                
                BeginInvoke(new UpdateStatusDelegate(UpdateLabeltip), new object[] { string.Format("扫描中。。。") });
                try
                {
                    // 
                    if (continuouscapture < 3 && detectfacethread(_frame))
                    {
                        FileNameCapture[continuouscapture] = Path.GetTempFileName() + "haveface.jpg";
                        _frame.Save(FileNameCapture[continuouscapture]);
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
                     //   BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("照片抓取成功,{0},{1}", continuouscapture, FileNameCapture[continuouscapture]) });
                        continuouscapture++;
                        if (continuouscapture > 2)
                        {
                            _capture.Pause();
                            capturing = false;
                            BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("3张照片抓取完成") });
                            BeginInvoke(new UpdateStatusDelegate(UpdateLabeltip), new object[] { string.Format("照片抓取完成") });
                        }
                    }
                  //  GC.Collect(111, GCCollectionMode.Forced);
                }
                catch (Exception ex)
                {
                    BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("in ProcessFrame,{0}", ex.Message) });
                }
            }
            // }
        }
        public class SmartCompareFaceInput
        {
            public string id { get; set; }
            public string idimage { get; set; }
            public string capture { get; set; }
        }
        public class screturn
        {
            public int code { get; set; }
            public string explanition { get; set; }
        }
        int cloudc(string idimage,string capture,string id)
        {
            var param = new SmartCompareFaceInput { id = id };
            param.idimage = Convert.ToBase64String(File.ReadAllBytes(idimage));
            param.capture = Convert.ToBase64String(File.ReadAllBytes(capture));

            var url = string.Format("http://{0}/{1}", host, cloudcompare);
            try
            {
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                using (var http = new HttpClient(handler))
                {
                    var content = new StringContent(JsonConvert.SerializeObject(param));
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var response = http.PostAsync(url, content).Result;
                    string srcString = response.Content.ReadAsStringAsync().Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                        var ret = JsonConvert.DeserializeObject<screturn>(srcString);
                        UpdateStatus(srcString);
                        if (ret.code == 1)
                        {
                            capturephotofile = capture;
                            return 1;
                        }
                        else return 0;
                    }
                    else
                    {
                        UpdateStatus(response.StatusCode+srcString);
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus("exception:"+ ex.Message );
            }
            return -2;
        }
        private void buttoncompare_Click(object sender, EventArgs e)
        {
            buttoncompare.Enabled = false;
            labeltip.Text = string.Empty;
            labeltip.Refresh();
            //try
            //{
            labelscore.Text = string.Empty;
            //  FileNameId = Path.Combine(homepath, dllpath, "photo.bmp");//临时没有读卡器时候测试用，正式必须注释，并反注释下面5行
            if (needReadId)
            {
                MessageBox.Show(string.Format("请先读取身份证信息！"));
                buttoncompare.Enabled = true;
                return;
            }
            if (continuouscapture < 3)
            {
                MessageBox.Show(string.Format("请先等待人脸照片抓取成功！"));//, continuouscapture));
                buttoncompare.Enabled = true;
                return;
            }
            labeltip.Text = "正在比对中。。。";
           
           
            var result = false;
            for (int i = 0; i < 3; i++)
            {
                // if (compareone(FileNameCapture[i], i + 1))
                var id = "37900919750819723X";
                if (!string.IsNullOrEmpty(upload.id)) id = upload.id;

              //  UpdateStatus(string.Format("before smart,{0}！{1}-", FileNameCapture[i], id));
                var cloud = cloudc(FileNameId, FileNameCapture[i], id);
                if(cloud==1)
                {                   
                    MessageBox.Show("云端比对成功，是同一个人！");
                    needReadId = true;
                    result = true;
                    break;
                }
                 else if(cloud== 0)
                {
                    UpdateStatus(string.Format("第{0}张照片比对不是同一人-{1}", i+1, id));
                    continue;
                }
                 else{
                    if(SmartCompare(FileNameCapture[i], i + 1, id))
                        {
                          
                            result = true;
                            break;
                        }
                }
                //{

                //}
                //else
                //{

                //}
                //if (SmartCompare(FileNameCapture[i], i + 1, id))
                //{
                //    var th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(uploadinfo));
                //    th.Start(upload);
                //    //  MessageBox.Show(stop.ElapsedMilliseconds + "比对成功，是同一个人" + m.Value);
                //    MessageBox.Show("比对成功，是同一个人！");
                //    needReadId = true;
                //    result = true;
                //    break;
                //}
            }
            labeltip.Text = "比对完成 ！";
            if (!result)
            {
                labeltip.Text = "相似度较低，不能确定是同一人！";
                UpdateStatus(string.Format("相似度较低，不能确定是同一人！"));
            }
            else
            {
                pictureid.BackgroundImage = null;
            }
            continuouscapture = 0;
            pictureBoxcurrentimage.BackgroundImage = null;
            picturecapture1.BackgroundImage = null;
            picturecapture2.BackgroundImage = null;

            //  GC.Collect(111, GCCollectionMode.Forced);
            //}
            //catch (Exception ex)
            //{
            //    UpdateStatus(string.Format("exception :{0}", ex.Message));
            //}
            buttoncompare.Enabled = true;
        }
        private bool detectfacethread(Mat frame)
        {
            try
            {
              //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("in detectfacethread,{0}", 111) });
                long detectionTime;
                List<Rectangle> faces = new List<Rectangle>();
                List<Rectangle> eyes = new List<Rectangle>();
                DetectFace.Detect(
                  frame, "haarcascade_frontalface_default.xml", "haarcascade_eye.xml",
                  faces, eyes,
                  out detectionTime);
                if (faces.Count == 1 && eyes.Count == 2) return true;

                return false;
            }
            catch (Exception ex)
            {
                BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("in detectfacethread,{0}", ex.Message) });
            }
            return false;
        }

            

        bool SmartCompare(string capturefile, int index, string id)
        {
            var stop = new Stopwatch();
            stop.Start();
            var a = new System.Diagnostics.Process();

            a.StartInfo.UseShellExecute = false;
            a.StartInfo.RedirectStandardOutput = true;
            a.StartInfo.CreateNoWindow = true;
            a.StartInfo.WorkingDirectory = homepath;
            var localimage = Path.Combine(lipath, id) + ".jpg";
            //if (_okFolks.Contains(id))
            //{
            //    a.StartInfo.Arguments = string.Format(" {0} {1}", localimage, capturefile);
            //    _score = 0.8;
            //}
            //else
            //{
            if (File.Exists(localimage))
            {
                a.StartInfo.Arguments = string.Format(" \"{0}\"  \"{1}\"", localimage, capturefile);
                _score = 0.80;
            }
            else
            {
                _score = 0.73;
                a.StartInfo.Arguments = string.Format(" \"{0}\"  \"{1}\"", FileNameId, capturefile);
            }
            // }

            UpdateStatus(string.Format("files:{0}", a.StartInfo.Arguments));
            capturephotofile = capturefile;
            a.StartInfo.FileName = Path.Combine(homepath, "compare.exe");
            a.Start();
            var output = a.StandardOutput.ReadToEnd();
            a.WaitForExit();
            var ret = a.ExitCode;

            var reg = @"(?<=terminate)0\.[\d]{4,}";
            var m = Regex.Match(output, reg);
            stop.Stop();
            BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("第{0}张照片比对用时{1}秒，相似度{2}", index, stop.ElapsedMilliseconds / 1000.0, m.Value) });

            if (m.Success)
            {
                var score = double.Parse(m.Value);
               // labelscore.Text = ((int)(score * 100)).ToString() + "%";
                if (score > _score)
                {
                    File.Copy(capturefile, localimage, true);
                    //  _okFolks.Add(id);
                    var th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(uploadinfo));
                    th.Start(upload);
                    //  MessageBox.Show(stop.ElapsedMilliseconds + "比对成功，是同一个人" + m.Value);
                    MessageBox.Show("本地比对成功，是同一个人！");
                    needReadId = true;
                    return true;
                }
            }
            return false;
        }
        // List<string> _okFolks = new List<string>();
        //bool compareone(string capturefile, int index)
        //{
        //    //var stop = new Stopwatch();
        //    //stop.Start();
        //    var a = new System.Diagnostics.Process();

        //    a.StartInfo.UseShellExecute = false;
        //    a.StartInfo.RedirectStandardOutput = true;
        //    a.StartInfo.CreateNoWindow = true;

        //    a.StartInfo.WorkingDirectory = Path.Combine(homepath, "compare");
        //    a.StartInfo.Arguments = string.Format(" {0} {1}", capturefile, FileNameId);
        //    //   UpdateStatus(string.Format("files:{0}", a.StartInfo.Arguments));
        //    capturephotofile = capturefile;
        //    a.StartInfo.FileName = Path.Combine(homepath, "compare", "FaceCompareCon.exe");
        //    //  a.StartInfo.FileName = Path.Combine(homepath, "compare", "ccompare.exe");
        //    a.Start();
        //    a.WaitForExit();
        //    var ret = a.ExitCode;
        //    //  stop.Stop();
        //    //  UpdateStatus(string.Format("time elapsed:{0},exitcode={1}", stop.ElapsedMilliseconds, ret));
        //    //if (ret == 1)
        //    //{
        //    //    MessageBox.Show("比对成功，是同一个人");
        //    //               FileNameId = string.Empty;
        //    //    return true;
        //    //}
        //    //else return false;
        //    var resultfile = Path.Combine(homepath, "compare", "compareresult.txt");
        //    var result = JsonConvert.DeserializeObject<result>(File.ReadAllText(resultfile));
        //    UpdateStatus(string.Format("result score:{0}", result.score));
        //    labelscore.Text = ((int)(result.score)).ToString() + "%";
        //    if (result.ok)
        //    {
        //        switch (result.status)
        //        {
        //            case CompareStatus.unkown:
        //                break;
        //            case CompareStatus.success:
        //                UpdateStatus(string.Format("第{1}张照片对比是同一个人，WARNING_VALUE={0}", 73, index));
        //                var th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(uploadinfo));
        //                th.Start(upload);
        //                File.Delete(resultfile);
        //                labeltip.Text = "比对完成 ！";
        //                MessageBox.Show("比对成功，是同一个人");
        //                FileNameId = string.Empty;
        //                break;
        //            case CompareStatus.failure:
        //                UpdateStatus(string.Format("第{1}张照片对比不是同一个人，WARNING_VALUE={0}", 73, index));
        //                break;
        //            case CompareStatus.uncertainty:
        //                UpdateStatus(string.Format("第{1}张照片不确定是否同一人，WARNING_VALUE={0}", 73, index));
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        UpdateStatus(string.Format("第{1}张照片比对出错，{0}", result.errcode, index));
        //    }

        //    if (result.ok && result.status == CompareStatus.success) return true;
        //    else return false;
        //}
        private void CheckUpdate()
        {
            var lv = long.Parse(version.Replace(".", ""));
            var url = string.Format("http://{0}/GetFaceDesktopUpdatePackage?version={1}", host, lv);
            var srcString = string.Empty;
           // var update = true;
            //do
            //{
            try
            {
                //Thread.Sleep(1000 * 60 );
                //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", 111) });
                using (var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip })
                using (var http = new HttpClient(handler))
                {
                    //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", 222) });
                    var response = http.GetAsync(url);
                    BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("CheckUpdate.{0},", response.Result.StatusCode) });
                    //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", "234234") });
                    if (!response.Result.IsSuccessStatusCode)
                    {
                        //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("no update") });
                        // Thread.Sleep(1000 * 60 * 60);
                        return;
                        // continue;
                    }
                    //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", 444) });
                    //   srcString = response.Result.Content.ReadAsStringAsync().Result;

                    if (response.Result.StatusCode == HttpStatusCode.OK)
                    {
                        var path = Path.GetTempFileName() + ".exe";// Path.Combine(exportPath, ui.Name);
                        BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", 2222) });
                        File.WriteAllBytes(path, response.Result.Content.ReadAsByteArrayAsync().Result);
                        BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("软件更新下载 {0}，{1} ok :", version, lv) });
                        if (MessageBox.Show("软件有新的版本，点击确定开始升级。", "确认", MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                        {
                            Process.Start(path);
                            Process.GetCurrentProcess().Kill();
                        }
                    }
                }
            }
            catch (Exception )
            {
                //if (ex.Message.Contains("发送请求时出错"))
                //    BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("软件更新查询:{0},url={1},{2}", version, url, "网站可能在更新，下次启动再查。") });
                //else
                //    BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("软件更新查询 error:{0},url={1},{2}", version, url, ex.Message) });
            }
            //  Thread.Sleep(1000 * 60 * 60);
            //  } while (update);
        }
        private void uploadinfo(object obj)
        {
            var ui = (ComparedInfo)obj;
            ui.capturephoto = File.ReadAllBytes(capturephotofile);
            ui.idphoto = File.ReadAllBytes(FileNameId);

            //  var url = string.Format("http://{0}/{1}", "localhost:5000", "PostCompared");
            var url = string.Format("http://{0}/{1}", host, action);// "api/Trails");// 
            try
            {
                //   BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", url) });

                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                //  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", 222) });
                using (var http = new HttpClient(handler))
                {
                    var input = JsonConvert.SerializeObject(ui);
                    var content = new StringContent(input);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var hrm = http.PostAsync(url, content);
                    var response = hrm.Result;
                    // BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("capturefile.{0},{1}", capturephotofile, ui.capturephoto) });
                    string srcString = response.Content.ReadAsStringAsync().Result;
                    //   BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},", srcString) });
                    BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("upload.{0},{1}", response.StatusCode, "") });
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
            richTextBox1.AppendText(Environment.NewLine);
        }
        private void UpdateIdInfoNoNewLine(string status)
        {
            richTextBox1.AppendText(status);
        }
        private void UpdateLabeltip(string status)
        {
            labeltip.Text = status;
        }
        private void UpdateStatus(string status)
        {
            //richTextBox1.AppendText(Environment.NewLine + string.Format("{0}--{1}", DateTime.Now, status));
            //richTextBox1.SelectionStart = richTextBox1.Text.Length;
            //richTextBox1.ScrollToCaret();
        }
        private void ReleaseData()
        {
            if (_capture != null)
                _capture.Dispose();
            if (_frame != null)
                _frame.Dispose();
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
                //videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                //if (videoDevices.Count == 0)
                //{
                //    UpdateStatus("没有摄像头，无法开始拍照，请连接摄像头！");
                //    return;
                //}
                //grabber = new VideoCapture();
                //grabber.QueryFrame();

                //Application.Idle += new EventHandler(FrameGrabber);
                _capture.Start();
                _tCheckSelfUpdate = new Thread(new ThreadStart(CheckUpdate));
                _tCheckSelfUpdate.Start();
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

        //void FrameGrabber(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        using (currentFrame = grabber.QueryFrame().ToImage<Bgr, Byte>())
        //        {
        //            if (currentFrame != null)
        //            {
        //                pictureBoxsource.BackgroundImage = currentFrame.Bitmap;

        //                if (HaveFace(currentFrame))
        //                {
        //                    FileNameCapture[continuouscapture] = Path.GetTempFileName() + "haveface.jpg";
        //                    currentFrame.Save(FileNameCapture[continuouscapture]);
        //                    switch (continuouscapture)
        //                    {
        //                        case 0:
        //                            pictureBoxcurrentimage.BackgroundImage = currentFrame.Bitmap;
        //                            break;
        //                        case 1:
        //                            picturecapture1.BackgroundImage = currentFrame.Bitmap;
        //                            break;
        //                        default:
        //                            picturecapture2.BackgroundImage = currentFrame.Bitmap;
        //                            break;
        //                    }

        //                    UpdateStatus(string.Format("照片抓取成功,{0}", continuouscapture));
        //                    continuouscapture++;
        //                    if (continuouscapture > 2)
        //                    {
        //                        capturing = false;
        //                        //Application.Idle -= new EventHandler(FrameGrabber);
        //                        //grabber.Dispose();

        //                        UpdateStatus(string.Format("3张照片抓取完成"));
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        UpdateStatus(string.Format("in FrameGrabber,{0}", ex.Message));
        //    }
        //}
        //bool HaveFace(Image<Bgr, Byte> fname)
        //{
        //    long detectionTime;
        //    List<Rectangle> faces = new List<Rectangle>();
        //    List<Rectangle> eyes = new List<Rectangle>();
        //    DetectFace.Detect(
        //      fname, "haarcascade_frontalface_default.xml", "haarcascade_eye.xml",
        //      faces, eyes,
        //      out detectionTime);
        //    if (faces.Count == 1 && eyes.Count == 2) return true;

        //    return false;
        //}


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
                capturing = true;
            }
            catch (Exception ex)
            {
                UpdateStatus(string.Format("restart:{0}", ex));
            }
        }

        private void buttonreadid_Click(object sender, EventArgs e)
        {
            int ret;
            int iPort = 1001;
            pictureid.BackgroundImage = null;
            pictureid.Refresh();
            //try
            //{
            ret = InitComm(iPort);
            if (ret ==1)
            {
                for (int i = 0; ; i++)
                {
                    ret = Authenticate();
                    if (ret ==1)
                    {
                        var outlen = 280;
                        var Msg = new byte[300];
                        //  MessageBox.Show("before read");
                        // ret = ReadBaseMsg(Msg, ref outlen);
                        var idpath = Path.GetTempFileName()+"cache";
                        Directory.CreateDirectory(idpath);
                        ret = ReadBaseMsgPhoto(Msg, ref outlen, idpath);
                        
                      //  MessageBox.Show("after read");
                        if (ret ==1)
                        {
                             richTextBox1.Clear();
                            upload.name = System.Text.Encoding.Default.GetString(Msg.Take(30).ToArray());
                            upload.gender = System.Text.Encoding.Default.GetString(Msg.Skip(31).Take(2).ToArray());
                            upload.nation = System.Text.Encoding.Default.GetString(Msg.Skip(34).Take(9).ToArray());
                            upload.birthday = System.Text.Encoding.Default.GetString(Msg.Skip(44).Take(8).ToArray());
                            upload.idaddress = Encoding.Default.GetString(Msg.Skip(53).Take(70).ToArray());
                            upload.id = System.Text.Encoding.Default.GetString(Msg.Skip(124).Take(18).ToArray());
                            upload.issuer = System.Text.Encoding.Default.GetString(Msg.Skip(143).Take(30).ToArray());
                            upload.startdate = System.Text.Encoding.Default.GetString(Msg.Skip(174).Take(8).ToArray());
                            upload.enddate = Encoding.Default.GetString(Msg.Skip(183).Take(8).ToArray());
                            //  var aaa = Encoding.Default.GetString(Msg.Skip(174).Take(18).ToArray());
                            UpdateIdInfo(string.Format("姓名：" + upload.name));
                            UpdateIdInfo(string.Format("性别：" + upload.gender));
                            UpdateIdInfo(string.Format("民族：" + upload.nation));
                            UpdateIdInfo(string.Format("住址：" + upload.idaddress));
                            UpdateIdInfo(string.Format("出生日期：{0}", upload.birthday));
                            UpdateIdInfo(string.Format("证件所属：" + upload.issuer));
                            UpdateIdInfo(string.Format("公民身份号码：" + upload.id));

                            UpdateIdInfoNoNewLine("身份证有效期：");
                            UpdateIdInfoNoNewLine(string.Format(upload.startdate));
                            UpdateIdInfoNoNewLine("-");
                            UpdateIdInfo(string.Format(upload.enddate));

                            needReadId = false;

                            FileNameId = Path.Combine(homepath, idpath, "photo.bmp");
                            pictureid.BackgroundImage = Image.FromFile(FileNameId);
                            UpdateStatus(string.Format("身份证信息读取成功"));
                            break;
                        }
                        else
                        {
                            UpdateStatus(string.Format("ReadBaseMsg --{0},{1},{2}", ret, outlen, Msg.ToString()));
                            MessageBox.Show("读卡器读卡错误");
                            break;
                        }
                    }
                    else
                    {
                        UpdateStatus(string.Format("Authenticate -第{1}次-{0}", ret, i));
                        UpdateStatus(string.Format("请将身份证放在读卡器上 !"));
                        System.Threading.Thread.Sleep(500);
                    }
                }
                ret = CloseComm();
                UpdateStatus(string.Format("关闭读卡器接口并退出"));
            }
            else
                UpdateStatus(string.Format("InitComm -error-{0}", ret));

            //}
            //catch (Exception ex)
            //{
            //    UpdateStatus(string.Format("身份证读卡器操作--{0} !", ex.Message));
            //}
        }
        // 身份证验证函数(标准18位验证)
        private bool CheckIDCard18(string idNumber)
        {
            long n = 0;
            if (long.TryParse(idNumber.Remove(17), out n) == false
                || n < Math.Pow(10, 16) || long.TryParse(idNumber.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证  
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(idNumber.Remove(2)) == -1)
            {
                return false;//省份验证  
            }
            string birth = idNumber.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证  
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = idNumber.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != idNumber.Substring(17, 1).ToLower())
            {
                return false;//校验码验证  
            }
            return true;//符合GB11643-1999标准  
        }
        private void button1_Click(object sender, EventArgs e)//无证件上传云端比对
        {
            if ((!Regex.IsMatch(textBoxid.Text, @"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", RegexOptions.IgnoreCase)))
            {
                MessageBox.Show("请输入正确的身份证号码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //  函数调用方法
            if (!CheckIDCard18(textBoxid.Text.ToString()))
            {
                MessageBox.Show("请输入正确的身份证号码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                  BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("GetNoidResult--11.{0},", url) });
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
                buttonnoid.BackgroundImage = Image.FromFile("image/rlsb_wsfz_you.png");
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
                ReleaseData();
                if (_tCheckSelfUpdate.ThreadState != System.Threading.ThreadState.Aborted)
                    _tCheckSelfUpdate.Abort();
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
            Pen pp = new Pen(Color.FromArgb(216, 216, 216));
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
    }
}