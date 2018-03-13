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

namespace face
{
    public partial class Form1 : Form
    {
        string FileNameId = string.Empty;
        string FileNameCapture = string.Empty;
        string dllpath = @"idr210sdk";

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

        //[DllImport(@"idr210sdk\sdtapi.dll")]
        //public extern static int InitComm(int iPort);
        //[DllImport(@"C:\dev\FaceServer\facewinform\idr210sdk\desktop\sdtapi.dll")]
        //public extern static int CloseComm();
        //[DllImport(@"C:\dev\FaceServer\facewinform\idr210sdk\desktop\sdtapi.dll")]
        //public extern static int Routon_IC_HL_ReadCardSN(StringBuilder SN);
        //[DllImport(@"C:\dev\FaceServer\facewinform\idr210sdk\desktop\sdtapi.dll")]
        //public extern static int Routon_IC_HL_ReadCard(int SID, int BID, int KeyType, byte[] Key, byte[] data);


        //[DllImport(@"C:\dev\FaceServer\facewinform\idr210sdk\desktop\sdtapi.dll")]
        //public extern static int Authenticate();

        //[DllImport(@"C:\dev\FaceServer\facewinform\idr210sdk\desktop\sdtapi.dll")]
        //public extern static int ReadBaseMsg(byte[] pMsg, int len);

        //[DllImport(@"C:\dev\FaceServer\facewinform\idr210sdk\desktop\sdtapi.dll")]
        //public extern static int ReadNewAppMsg(byte[] pMsg, out int len);

        //const double WARNING_VALUE = 73.0f;
        //public struct Engine { };

        //[DllImport(@"C:\dev\facesdk\lib\x86\core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        //public extern static int mgv_set_log(int level);
        //[DllImport(@"C:\dev\facesdk\lib\x86\core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        //public extern static unsafe int mgv_create_engine(string model_path, Engine** pengine);
        //[DllImport(@"C:\dev\facesdk\lib\x86\core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        //public extern static string mgv_get_error_str(int code);
        //[DllImport(@"C:\dev\facesdk\lib\x86\core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        //public extern static unsafe int mgv_destroy_engine(Engine* engine);
        //[DllImport(@"C:\dev\facesdk\lib\x86\core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        //public extern static unsafe int GetFeatureFromJpeg(byte[] f1, int len1, byte[] f2, int len2);
        //[DllImport(@"C:\dev\facesdk\lib\x86\core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        //public extern static unsafe float CalcFeatureSimilarity(byte[] featData1, int featLen1, byte[] featData2, int featLen2);

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
        public Form1()
        {
            InitializeComponent();
            homepath = Environment.CurrentDirectory;
        }
        public class result
        {
            public bool ok { get; set; }
            public float score { get; set; }
            public CompareStatus status { get; set; }
            public mgverror errcode { get; set; }
        }
        public enum CompareStatus
        {
            unkown, success, failure, uncertainty
        }

        public enum mgverror
        {
            unkown,
            MGV_ERR = -1,
            MGV_MALLOC_ERR = -2,
            MGV_IMAGE_FORMAT_ERR = -3,
            MGV_PARA_ERR = -4,
            MGV_IMAGE_OUT_OF_RANGE = -5,
            MGV_NO_FACE_DETECTED = -6,
            MGV_MULTIPLE_FACES_DETECTED = -7,
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
                if (FileNameCapture == string.Empty)
                {
                    UpdateStatus(string.Format("请先等待人脸照片抓取成功！"));
                    return;
                }
                var a = new System.Diagnostics.Process();
                a.StartInfo.UseShellExecute = false;
                a.StartInfo.WorkingDirectory = Path.Combine(homepath, "compare");
                a.StartInfo.CreateNoWindow = true ;
                a.StartInfo.Arguments = string.Format(" {0} {1}", FileNameCapture, FileNameId);
                a.StartInfo.FileName = Path.Combine(homepath, "compare", "FaceCompareCon.exe");
                //  a.StartInfo.FileName = @"C:\dev\ocx\TestOCX\ConsoleApplication1\aaa.exe";
                a.Start();
                a.WaitForExit();
                // var ret = a.ExitCode;
                var result = JsonConvert.DeserializeObject<result>(File.ReadAllText(Path.Combine(homepath, "compare","compareresult.txt")));
                FileNameCapture = string.Empty;
                UpdateStatus(string.Format("result score:{0}", result.score));
                if (result.ok)
                {
                    switch (result.status)
                    {
                        case CompareStatus.unkown:
                            break;
                        case CompareStatus.success:
                            UpdateStatus(string.Format("是同一个人，WARNING_VALUE={0}", 73));
                            MessageBox.Show("比对成功，是同一个人");
                            FileNameId = string.Empty;
                            //to do ：upload cloud
                            break;
                        case CompareStatus.failure:
                            UpdateStatus(string.Format("不是同一个人，WARNING_VALUE={0}",73));
                            break;
                        case CompareStatus.uncertainty:
                            UpdateStatus(string.Format("不确定是否同一人，WARNING_VALUE={0}", 73));
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    MessageBox.Show(string.Format( "比对出错，{0}",result.errcode));
                }
            }
            catch (Exception ex)
            {
                UpdateStatus(string.Format("exception :{0},{1},{2}", FileNameCapture, FileNameId, ex.Message));
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
                            FileNameCapture = Path.GetTempFileName() + "haveface.jpg";
                            currentFrame.Save(FileNameCapture);
                            pictureBoxcurrentimage.Image = currentFrame.Bitmap;
                            UpdateStatus(string.Format("照片抓取成功,{0}", ++facenum));
                            Application.Idle -= new EventHandler(FrameGrabber);
                            grabber.Dispose();
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
                            UpdateStatus(string.Format(System.Text.Encoding.Default.GetString(Msg.Take(31).ToArray())));
                            UpdateStatus(string.Format(System.Text.Encoding.Default.GetString(Msg.Skip(31).Take(3).ToArray())));
                            UpdateStatus(string.Format(System.Text.Encoding.Default.GetString(Msg.Skip(34).Take(10).ToArray())));
                            UpdateStatus(string.Format(System.Text.Encoding.Default.GetString(Msg.Skip(44).Take(9).ToArray())));
                            UpdateStatus(string.Format(Encoding.Default.GetString(Msg.Skip(53).Take(71).ToArray())));
                            UpdateStatus(string.Format(System.Text.Encoding.Default.GetString(Msg.Skip(124).Take(19).ToArray())));
                            UpdateStatus(string.Format(System.Text.Encoding.Default.GetString(Msg.Skip(143).Take(31).ToArray())));
                            UpdateStatus(string.Format(System.Text.Encoding.Default.GetString(Msg.Skip(174).Take(9).ToArray())));
                            UpdateStatus(string.Format(Encoding.Default.GetString(Msg.Skip(183).Take(9).ToArray())));
                            var FileNameIdtmp = Path.Combine(homepath, dllpath, "photo.bmp");
                            FileNameId = Path.Combine(homepath, dllpath, "photo.jpg");
                            Image.FromFile(FileNameIdtmp).Save(FileNameId, ImageFormat.Jpeg);
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
    }
}
