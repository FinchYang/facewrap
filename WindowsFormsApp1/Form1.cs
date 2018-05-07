using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private delegate void UpdateStatusDelegate(string status);
        private void UpdateStatus(string status)
        {
            richTextBox1.AppendText(Environment.NewLine + string.Format("{0}--{1}", DateTime.Now, status));
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //ImageViewer viewer = new ImageViewer(); //create an image viewer
            //Capture capture = new Capture(); //create a camera captue
            //Application.Idle += new EventHandler(delegate (object sender, EventArgs e)
            //{  //run this until application closed (close button click on image viewer)
            //    viewer.Image = capture.QueryFrame(); //draw the image obtained from camera
            //});
            //viewer.ShowDialog(); //show the image viewer
            homepath = Environment.CurrentDirectory;
            //ThreadPool.QueueUserWorkItem(ar =>
            //{

            //    Thread.Sleep(1000);


            //    for (; ; )
            //    {
            //        Thread.Sleep(100);
            //        // if (memid.memoid !=string.Empty && memid.checkok == true) continue;
            //        var comm = new common();
            //        int port = 0;  //自动搜索端口          
            //        bool ret = comm.connectDev(port);
            //        if (!ret)
            //        {
            //            comm.closeDev();
            //            MessageBox.Show("连接设备失败", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            continue;
            //        }
            //        ret = comm.findIDCard();
            //        if (!ret)
            //        {
            //            comm.closeDev();
            //            // BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("请把二代证放在读卡区 ")});
            //            continue;
            //        }

            //        byte[] txtBuff = new byte[256];

            //        ret = comm.readIDInfo(out txtBuff);//默认生成zp.bmp照片
            //        if (!ret)
            //        {
            //            comm.closeDev();
            //            BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("读取二代证信息失败  ") });
            //            continue;
            //        }

            //        var idInfo = comm.ParaseIdInfo();
            //        comm.closeDev();

            //        var name = idInfo.getName();
            //        var id = idInfo.getId();
            //        var gender = idInfo.getSex();
            //        var folk = idInfo.getFolk();
            //        BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { id });
            //        BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { folk });
            //        BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { gender });
            //        BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { name });


            //        //BeginInvoke(new UpdateStatusDelegate(UpdateIdPhoto), new object[] { "zp.bmp" });
            //        //while (FileNameCapture == string.Empty)
            //        //{
            //        //    Thread.Sleep(100);
            //        //}


            //        //lock (lockObj)
            //        //{
            //        //    var stop = new Stopwatch();
            //        //    stop.Start();

            //        //    var rrr = compare(FileNameCapture, "zp.bmp");
            //        //    var ok = true;

            //        //    BeginInvoke(new UpdateStatusDelegate(UpdateCapturePhoto), new object[] { "清空照片" });
            //        //    BeginInvoke(new UpdateStatusDelegate(UpdateMemId), new object[] { id });
            //        //    var reg = @"[\d]+";
            //        //    var m = Regex.Match(rrr, reg);
            //        //    stop.Stop();
            //        //    var per = double.Parse(m.Value) / 100000;
            //        //    BeginInvoke(new UpdateStatusDelegate(UpdateResult), new object[] { (per * 100).ToString() });

            //        //    if (m.Success && per > 0.7)
            //        //    {
            //        //        BeginInvoke(new UpdateStatusDelegate(UpdateCaputureFile), new object[] { string.Empty });
            //        //        BeginInvoke(new UpdateStatusDelegate(UpdateMemId), new object[] { string.Empty });
            //        //        MessageBox.Show(stop.ElapsedMilliseconds + "比对成功，是同一个人" + m.Value);
            //        //        ok = true;
            //        //    }
            //        //    else ok = false;
            //        //    BeginInvoke(new UpdateStatusDelegate(UpdateCheckok), new object[] { ok.ToString() });

            //        //    BeginInvoke(new UpdateStatusDelegate(UpdateStatus), new object[] { string.Format("similarity--{0}-{1}-", rrr, stop.ElapsedMilliseconds) });


            //        //}

            //    }

            //});
        }
        public string homepath = string.Empty;
        [DllImport(@"idr210sdk\sdtapi.dll")]
        public extern static int ReadBaseInfos(byte[] Name, byte[] Gender, byte[] Folk,
byte[] BirthDay, byte[] Code, byte[] Address, byte[] Agency, byte[] ExpireStart, byte[] ExpireEnd);

        [DllImport(@"idr210sdk\sdtapi.dll")]
        public extern static int InitComm(int iPort);
        [DllImport(@"idr210sdk\sdtapi.dll")]
        public extern static int CloseComm();
        [DllImport(@"idr210sdk\sdtapi.dll")]
        public extern static int Authenticate();
        [DllImport(@"idr210sdk\sdtapi.dll")]
        public extern static int ReadBaseMsg(byte[] pMsg, ref int len);
        private ComparedInfo upload = new ComparedInfo { address = "temp", operatingagency = "hehe" };
        public class ComparedInfo
        {
            public string id { get; set; }
            public string name { get; set; }
            public string nation { get; set; }
            public string nationality { get; set; }
            public string address { get; set; }
            public string idaddress { get; set; }
            public string operatingagency { get; set; }
            public string issuer { get; set; }
            public string gender { get; set; }
            public string birthday { get; set; }
            public string startdate { get; set; }
            public string enddate { get; set; }
            public byte[] idphoto { get; set; }
            public byte[] capturephoto { get; set; }
            //public string idphotofile { get; set; }
            //public string capturephotofile { get; set; }
        }
        private void UpdateIdInfo(string status)
        {
            richTextBox1.AppendText(status);
            richTextBox1.AppendText(Environment.NewLine);
        }
        private void button1_Click(object sender, EventArgs e)
        {
          
                int ret;
                int iPort = 1001;
                //try
                //{
                ret = InitComm(iPort);
                if (ret == 1)
                {
                    for (int i = 0; ; i++)
                    {
                        ret = Authenticate();
                        if (ret == 1)
                        {
                            var outlen = 280;
                            var Msg = new byte[300];
                            ret = ReadBaseMsg(Msg, ref outlen);
                            if (ret == 1)
                            {
                                // richTextBox1.Clear();
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

                                //UpdateIdInfoNoNewLine("身份证有效期：");
                                //UpdateIdInfoNoNewLine(string.Format(upload.startdate));
                                //UpdateIdInfoNoNewLine("-");
                                //UpdateIdInfo(string.Format(upload.enddate));

                                //needReadId = false;
                                //pictureid.BackgroundImage = Image.FromFile(FileNameId);
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

             
        }
        public class common
        {
            [DllImport("RdCard.dll")]
            public static extern int UCommand1(Byte[] pCmd, ref int parg0, ref int parg1, ref byte parg2);
            [DllImport("RdCard.dll")]
            public static extern int GetSamID(ref byte samIDBuff);

            [DllImport("RdCard.dll")]
            extern static int GetAddr(ref byte strBuff);
            [DllImport("RdCard.dll")]
            extern static int GetBegin(ref byte strBuff);
            [DllImport("RdCard.dll")]
            extern static int GetName(ref byte strBuff);
            [DllImport("RdCard.dll")]
            extern static int GetSex(ref byte strBuff);
            [DllImport("RdCard.dll")]
            extern static int GetFolk(ref byte strBuff);
            [DllImport("RdCard.dll")]
            extern static int GetIDNum(ref byte strBuff);
            [DllImport("RdCard.dll")]
            extern static int GetDep(ref byte strBuff);
            [DllImport("RdCard.dll")]
            extern static int GetBirth(ref byte strBuff);
            [DllImport("RdCard.dll")]
            extern static int GetEnd(ref byte strBuff);
            [DllImport("RdCard.dll")]
            extern static int GetBmpPath(ref byte strBuff);
            [DllImport("RdCard.dll")]
            extern static int GetNewAddr(ref byte strBuff);
            [DllImport("RdCard.dll")]
            extern static int GetSAMIDToStr(ref byte strBuff);



            public bool connectDev(int port)
            {
                bool ret = false;
                try
                {
                    byte[] cmd = { 0x41 };
                    int para0 = port;
                    int para1 = 8811;
                    byte[] para2 = { 0x02, 0x27, 0x00, 0x00 };    //9986 
                    int nRet = UCommand1(cmd, ref para0, ref para1, ref para2[0]);
                    if (nRet == 62171 || nRet == -5 || nRet == -7)
                    {
                        byte[] abSAMId = new byte[16];
                        if (GetSamID(ref abSAMId[0]) == 1)
                        {

                            ret = true;
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(" connect occurs exception：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                return ret;
            }


            public bool findIDCard()
            {
                bool ret = false;
                try
                {
                    byte[] cmd = { 0x43 };
                    int para0 = 1;
                    int para1 = 8811;
                    byte[] para2 = { 0x02, 0x27, 0x00, 0x00 };    //9986 
                    int nRet = UCommand1(cmd, ref para0, ref para1, ref para2[0]);

                    if (nRet == 62171 || nRet == 62172)
                    {
                        ret = true;
                    }
                    else if (nRet < 0)
                    {
                        ret = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(" find occurs exception：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                return ret;
            }

            public bool readIDInfo(out byte[] txtBuff)
            {
                bool ret = false;
                txtBuff = new byte[256];
                try
                {
                    byte[] cmd = { 0x44 };
                    int para0 = 1;
                    int para1 = 8811;
                    byte[] para2 = { 0x02, 0x27, 0x00, 0x00 };    //9986 

                    int nRet = UCommand1(cmd, ref para0, ref para1, ref para2[0]);

                    byte[] strBuff = new byte[70];
                    if (nRet == 62171 || nRet == 62172)
                    {

                        txtBuff = readFileToBytes("wz.txt");
                        File.Delete("pic.wlt");
                        File.Delete("wz.txt");
                        File.Delete("wx.txt");
                        ret = true;
                    }
                    else if (nRet < 0)
                    {
                        ret = false;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(" readcard occurs exception：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                return ret;
            }


            public string getNewaddr()
            {
                string address = "";
                try
                {
                    byte[] cmd = { 0x45 };
                    int para0 = 1;
                    int para1 = 8811;
                    byte[] para2 = { 0x02, 0x27, 0x00, 0x00 };    //9986 

                    int nRet = UCommand1(cmd, ref para0, ref para1, ref para2[0]);

                    byte[] strBuff = new byte[70];
                    if (nRet == 62171)
                    {
                        byte[] addrBuff = new byte[70];
                        addrBuff = readFileToBytes("NewAdd.txt");//Unicode                    
                        address = Encoding.Unicode.GetString(addrBuff);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(" readcard occurs exception：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                return address;
            }

            public byte[] readFileToBytes(string fileFullName)
            {
                byte[] bytes = null;
                try
                {
                    if ((!String.IsNullOrEmpty(fileFullName)) && (File.Exists(fileFullName)))
                    {
                        using (FileStream fStream = File.OpenRead(fileFullName))
                        {
                            int fileLen = (int)fStream.Length;
                            bytes = new byte[fileLen];
                            fStream.Read(bytes, 0, fileLen);
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(" readFileToBytes occurs exception：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                return bytes;
            }


            public IdInfo ParaseIdInfo()
            {
                IdInfo idInfo = new IdInfo();
                try
                {
                    byte[] dataBuff = new byte[30];
                    GetName(ref dataBuff[0]);
                    string strTemp = Encoding.GetEncoding("GBK").GetString(dataBuff, 0, 30);
                    idInfo.setName(strTemp);

                    dataBuff = new byte[2];
                    GetSex(ref dataBuff[0]);
                    strTemp = Encoding.GetEncoding("GBK").GetString(dataBuff, 0, 1);
                    idInfo.setSex("男");
                    if (strTemp.Equals("2"))
                    {
                        idInfo.setSex("女");
                    }

                    dataBuff = new byte[4];
                    GetFolk(ref dataBuff[0]);
                    strTemp = Encoding.GetEncoding("GBK").GetString(dataBuff, 0, 2);
                    idInfo.setFolk(Folks[int.Parse(strTemp) - 1]);

                    dataBuff = new byte[16];
                    GetBirth(ref dataBuff[0]);
                    strTemp = Encoding.GetEncoding("GBK").GetString(dataBuff, 0, 8);
                    idInfo.setBirthDay(strTemp);

                    dataBuff = new byte[70];
                    GetAddr(ref dataBuff[0]);
                    strTemp = Encoding.GetEncoding("GBK").GetString(dataBuff, 0, 70);
                    idInfo.setAddress(strTemp);

                    dataBuff = new byte[36];
                    GetIDNum(ref dataBuff[0]);
                    strTemp = Encoding.GetEncoding("GBK").GetString(dataBuff, 0, 18);
                    idInfo.setId(strTemp);

                    dataBuff = new byte[30];
                    GetDep(ref dataBuff[0]);
                    strTemp = Encoding.GetEncoding("GBK").GetString(dataBuff, 0, 30);
                    idInfo.setIssueOrgan(strTemp);

                    dataBuff = new byte[16];
                    GetBegin(ref dataBuff[0]);
                    strTemp = Encoding.GetEncoding("GBK").GetString(dataBuff, 0, 8);
                    idInfo.setAvailityBegin(strTemp);

                    dataBuff = new byte[16];
                    GetEnd(ref dataBuff[0]);
                    strTemp = Encoding.GetEncoding("GBK").GetString(dataBuff, 0, 8);
                    idInfo.setAvailityEnd(strTemp);

                    dataBuff = new byte[100];
                    GetBmpPath(ref dataBuff[0]);
                    strTemp = Encoding.GetEncoding("GBK").GetString(dataBuff, 0, 100);
                    idInfo.setbmpPath(strTemp);

                    return idInfo;
                }
                catch (Exception )
                {

                    return null;
                }

            }

            public void getsamID(ref byte samid)
            {
                GetSAMIDToStr(ref samid);
                //GetSamID(ref samid);
            }

            //解析二代证信息，BmpPhotoFileName :生成的二代证BMP照片的全路径,以及指纹信息
            public IdInfo ParaseIDBuff(byte[] txtBuff)
            {
                IdInfo idinfo = new IdInfo();
                try
                {
                    int iStartIndex = 0;

                    Array.Copy(txtBuff, iStartIndex, idinfo.abName, 0, idinfo.abName.Length);

                    iStartIndex = idinfo.abName.Length;
                    Array.Copy(txtBuff, iStartIndex, idinfo.abSex, 0, idinfo.abSex.Length);

                    iStartIndex = iStartIndex + idinfo.abSex.Length;
                    Array.Copy(txtBuff, iStartIndex, idinfo.abFolk, 0, idinfo.abFolk.Length);

                    iStartIndex = iStartIndex + idinfo.abFolk.Length;
                    Array.Copy(txtBuff, iStartIndex, idinfo.abBirth, 0, idinfo.abBirth.Length);

                    iStartIndex = iStartIndex + idinfo.abBirth.Length;
                    Array.Copy(txtBuff, iStartIndex, idinfo.abAddress, 0, idinfo.abAddress.Length);

                    iStartIndex = iStartIndex + idinfo.abAddress.Length;
                    Array.Copy(txtBuff, iStartIndex, idinfo.abId, 0, idinfo.abId.Length);

                    iStartIndex = iStartIndex + idinfo.abId.Length;
                    Array.Copy(txtBuff, iStartIndex, idinfo.abIssueOrgan, 0, idinfo.abIssueOrgan.Length);

                    iStartIndex = iStartIndex + idinfo.abIssueOrgan.Length;
                    Array.Copy(txtBuff, iStartIndex, idinfo.abAvailityBegin, 0, idinfo.abAvailityBegin.Length);

                    iStartIndex = iStartIndex + idinfo.abAvailityBegin.Length;
                    Array.Copy(txtBuff, iStartIndex, idinfo.abAvailityEnd, 0, idinfo.abAvailityEnd.Length);

                    iStartIndex = iStartIndex + idinfo.abAvailityEnd.Length;
                    Array.Copy(txtBuff, iStartIndex, idinfo.abOther, 0, idinfo.abOther.Length);

                    idinfo.setName(Encoding.Unicode.GetString(idinfo.abName));
                    string sexCode = Encoding.Unicode.GetString(idinfo.abSex);
                    idinfo.setSex("女");
                    if (sexCode.Equals("1"))
                    {
                        idinfo.setSex("男");
                    }
                    string folkCode = Encoding.Unicode.GetString(idinfo.abFolk);
                    idinfo.setFolkId(folkCode);
                    idinfo.setFolk(Folks[int.Parse(folkCode) - 1]);

                    idinfo.setBirthDay(Encoding.Unicode.GetString(idinfo.abBirth));
                    idinfo.setAddress(Encoding.Unicode.GetString(idinfo.abAddress));
                    idinfo.setId(Encoding.Unicode.GetString(idinfo.abId));
                    idinfo.setIssueOrgan(Encoding.Unicode.GetString(idinfo.abIssueOrgan));
                    idinfo.setAvailityBegin(Encoding.Unicode.GetString(idinfo.abAvailityBegin));
                    idinfo.setAvailityEnd(Encoding.Unicode.GetString(idinfo.abAvailityEnd));

                    return idinfo;
                }
                catch (Exception ex)
                {

                    MessageBox.Show("ParaseIDBuff occurs exception：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return null;
                }


            }
            //民族列表
            public static string[] Folks = new string[60]
             {
             "汉",
             "蒙古",
             "回",
             "藏",
             "维吾尔",
             "苗",
             "彝",
             "壮",
             "布依",
             "朝鲜",
             "满",
             "侗",
             "瑶",
             "白",
             "土家",
             "哈尼",
             "哈萨克",
             "傣",
             "黎",
             "傈僳",
             "佤",
             "畲",
             "高山",
             "拉祜",
             "水",
             "东乡",
             "纳西",
             "景颇",
             "柯尔克孜",
             "土",
             "达斡尔",
             "仫佬",
             "羌",
             "布朗",
             "撒拉",
             "毛南",
             "仡佬",
             "锡伯",
             "阿昌",
             "普米",
             "塔吉克",
             "怒",
             "乌孜别克",
             "俄罗斯",
             "鄂温克",
             "德昂",
             "保安",
             "裕固",
             "京",
             "塔塔尔",
             "独龙",
             "鄂伦春",
             "赫哲",
             "门巴",
             "珞巴",
             "基诺",
             "穿青人",
             "家人",
             "入籍",
             "其它"
             };

            public void closeDev()
            {
                try
                {
                    byte[] cmd = { 0x42 };
                    int para0 = 1001;
                    int para1 = 8811;
                    byte[] para2 = { 0x02, 0x27, 0x00, 0x00 };    //9986 
                    int nRet = UCommand1(cmd, ref para0, ref para1, ref para2[0]);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("closedevice occurs exception：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }
        }
        public class IdInfo
        {
            private string Name = "";
            private string Sex = "";
            private string Folk = "";
            private string FolkId = "";
            private string BirthDay = "";
            private string Address = "";
            private string Id = "";
            private string IssueOrgan = "";
            private string AvailityBegin = "";
            private string AvailityEnd = "";
            private string Newaddress = "";
            private string IdBase64Photo = "";
            private string bmpPath = "";
            public byte[] abName = new byte[30];
            public byte[] abSex = new byte[2];
            public byte[] abFolk = new byte[4];
            public byte[] abBirth = new byte[16];
            public byte[] abAddress = new byte[70];
            public byte[] abId = new byte[36];
            public byte[] abIssueOrgan = new byte[30];
            public byte[] abAvailityBegin = new byte[16];
            public byte[] abAvailityEnd = new byte[16];
            public byte[] abOther = new byte[36];
            public byte[] abPhoto = new byte[1024];
            public byte[] abNewaddress = new byte[70];

            public void setName(string name)
            {
                Name = name;
            }
            public string getName()
            {
                return Name;
            }
            public void setSex(string sex)
            {
                Sex = sex;
            }
            public string getSex()
            {
                return Sex;
            }
            public void setFolk(string folk)
            {
                Folk = folk;
            }
            public string getFolk()
            {
                return Folk;
            }
            public string getFolkId()
            {
                return FolkId;
            }
            public void setFolkId(string folkid)
            {
                FolkId = folkid;
            }

            public void setBirthDay(string birthDay)
            {
                BirthDay = birthDay;
            }
            public string getBirthDay()
            {
                return BirthDay;
            }
            public void setAddress(string address)
            {
                Address = address;
            }
            public string getAddress()
            {
                return Address;
            }

            public void setId(string id)
            {
                Id = id;
            }
            public string getId()
            {
                return Id;
            }
            public void setIssueOrgan(string issueOrgan)
            {
                IssueOrgan = issueOrgan;
            }
            public string getIssueOrgan()
            {
                return IssueOrgan;
            }
            public void setAvailityBegin(string availityBegin)
            {
                AvailityBegin = availityBegin;
            }
            public string getAvailityBegin()
            {
                return AvailityBegin;
            }

            public void setAvailityEnd(string availityEnd)
            {
                AvailityEnd = availityEnd;
            }
            public string getAvailityEnd()
            {
                return AvailityEnd;
            }
            public void setNewaddress(string newaddress)
            {
                Newaddress = newaddress;
            }
            public string getNewaddress()
            {
                return Newaddress;
            }

            public void setbmpPath(string path)
            {
                bmpPath = path;
            }
            public string getbmpPath()
            {
                return bmpPath;
            }
            public void setIdBase64Photo(string idBase64Photo)
            {
                IdBase64Photo = idBase64Photo;
            }
            public string getIdBase64Photo()
            {
                return IdBase64Photo;
            }



        }
        common comm = new common();
        IdInfo idInfo = new IdInfo();
        private void button2_Click(object sender, EventArgs e)
        {

            int port = 0;  //自动搜索端口          
            bool ret = comm.connectDev(port);
            if (!ret)
            {
                MessageBox.Show("连接设备失败", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ret = comm.findIDCard();
            if (!ret)
            {
                MessageBox.Show("请把二代证放在读卡区 ", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] txtBuff = new byte[256];

            ret = comm.readIDInfo(out txtBuff);//默认生成zp.bmp照片
            if (!ret)
            {
                MessageBox.Show("读取二代证信息失败  ", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //idInfo = comm.ParaseIDBuff(txtBuff);
            idInfo = comm.ParaseIdInfo();
           UpdateStatus( idInfo.getName());
            UpdateStatus(idInfo.getId());
            UpdateStatus(idInfo.getAddress());
            UpdateStatus(comm.getNewaddr());
            string temp = "";
            temp += "性别:" + idInfo.getSex() + "^_^";
            temp += "民族:" + idInfo.getFolk() + "^_^";
            temp += "出生日期:" + idInfo.getBirthDay() + "^_^";
            temp += "有效日期:" + idInfo.getAvailityBegin() + "--" + idInfo.getAvailityEnd() + "^_^";
            temp += "照片:" + idInfo.getbmpPath() + "^_^";

            temp += "签发机关:" + idInfo.getIssueOrgan().Trim() + "^_^";

            UpdateStatus( temp);

            //using (FileStream fs = new FileStream("zp.bmp", FileMode.Open, FileAccess.Read, FileShare.Read))
            //{

            //    BinaryReader br = new BinaryReader(fs);
            //    MemoryStream ms = new MemoryStream(br.ReadBytes((int)fs.Length));
            //    imgIdrz.Image = Image.FromStream(ms);
            //}
            comm.closeDev();
        }
    }
}
