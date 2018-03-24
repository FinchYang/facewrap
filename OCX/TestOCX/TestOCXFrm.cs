using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
//using Newtonsoft.Json;

namespace TestOCX
{
    [Guid("7cbebdca-776e-4238-9191-1554c6872b8b")]
    public partial class TestOCXFrm : UserControl, IObjectSafety
    {
        delegate void D(object obj);
        const double WARNING_VALUE = 73.0f;
        public struct Engine { };
      
        [DllImport(@"core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int mgv_set_log(int level);
        [DllImport(@"core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static unsafe int mgv_create_engine(string model_path, Engine** pengine);
        [DllImport(@"core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static string mgv_get_error_str(int code);
        [DllImport(@"core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static unsafe int mgv_destroy_engine(Engine* engine);
        [DllImport(@"core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static unsafe int GetFeatureFromJpeg(byte[] f1, int len1, byte[] f2, int len2);
        [DllImport(@"core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static unsafe float CalcFeatureSimilarity(byte[] featData1, int featLen1, byte[] featData2, int featLen2);
        public TestOCXFrm()
        {
            InitializeComponent();
         
        }
       
        public string face_compare(object a, object b)
        {
            try
            {
                unsafe
                {
                    Engine* engine = null;

                    mgv_set_log(0);
                    //  fixed (unsafe engine = new Engine()) {
                    mgv_create_engine("", &engine);
                    //  }

                    //  Console.WriteLine("mgv_create_engine return:{0}\n", mgv_get_error_str(ret));



                    var ret = new result { ok = true, score = 0, status = CompareStatus.unkown, errcode = mgverror.unkown };
                    FaceFile fcontent1, fcontent2;


                    var fbyte1 = Convert.FromBase64String(a.ToString());
                    var fbyte2 = Convert.FromBase64String(b.ToString());
                    int featLen1 = 0;
                    int featLen2 = 0;
                    byte[] featData1 = new byte[4096];
                    byte[] featData2 = new byte[4096];

                    featLen1 = GetFeatureFromJpeg(fbyte1, fbyte1.Length, featData1, 4096 * 8);
                    mgv_destroy_engine(engine);
                    //   var mgvret = ShowReturnCode(featLen1);
                    if (featLen1 <= 0)
                    {

                        return featLen1.ToString();
                    }
                    featLen2 = GetFeatureFromJpeg(fbyte2, fbyte2.Length, featData2, 4096 * 8);
                    mgv_destroy_engine(engine);
                    //   mgvret = ShowReturnCode(featLen2);
                    if (featLen2 <= 0)
                    {

                        return featLen2.ToString();
                    }

                    float score = CalcFeatureSimilarity(featData1, featLen1, featData2, featLen2);
                    mgv_destroy_engine(engine);
                    ret.score = score;
                    if (score <= 57.0f)
                    {

                        return 2.ToString();
                    }
                    else if (score > WARNING_VALUE)
                    {

                        return 1.ToString();
                    }
                    else
                    {

                        return 3.ToString();
                    }
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
        public struct FaceFile
        {
            public byte[] fcontent;
            public int flen;
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
        public void Start(object obj)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(ShowTime));
                t.Start(obj.ToString() + "，线程：" + i.ToString());
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Start("Hello World");
        }
        void ShowTime(object obj)
        {
            if (this.listBox1.InvokeRequired)
            {
                D d = new D(DelegateShowTime);
                listBox1.Invoke(d, obj);
            }
            else
            {
                this.listBox1.Items.Add(obj);
            }


        }


        void DelegateShowTime(object obj)
        {
            this.listBox1.Items.Add(obj);
        }

        #region IObjectSafety 成员

        private const string _IID_IDispatch = "{00020400-0000-0000-C000-000000000046}";
        private const string _IID_IDispatchEx = "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}";
        private const string _IID_IPersistStorage = "{0000010A-0000-0000-C000-000000000046}";
        private const string _IID_IPersistStream = "{00000109-0000-0000-C000-000000000046}";
        private const string _IID_IPersistPropertyBag = "{37D84F60-42CB-11CE-8135-00AA004BB851}";

        private const int INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001;
        private const int INTERFACESAFE_FOR_UNTRUSTED_DATA = 0x00000002;
        private const int S_OK = 0;
        private const int E_FAIL = unchecked((int)0x80004005);
        private const int E_NOINTERFACE = unchecked((int)0x80004002);

        private bool _fSafeForScripting = true;
        private bool _fSafeForInitializing = true;


        public int GetInterfaceSafetyOptions(ref Guid riid, ref int pdwSupportedOptions, ref int pdwEnabledOptions)
        {
            int Rslt = E_FAIL;

            string strGUID = riid.ToString("B");
            pdwSupportedOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForScripting == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForInitializing == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_DATA;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }

            return Rslt;
        }

        public int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions)
        {
            int Rslt = E_FAIL;

            string strGUID = riid.ToString("B");
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_CALLER) &&
                            (_fSafeForScripting == true))
                        Rslt = S_OK;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_DATA) &&
                            (_fSafeForInitializing == true))
                        Rslt = S_OK;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }

            return Rslt;
        }

        #endregion

      

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(@"G:\fruits-pictures\1555.jpg");
        }
    }
}
