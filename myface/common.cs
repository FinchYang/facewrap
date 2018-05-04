using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
namespace face
{
   public class common
    {
       [DllImport("RdCard.dll")]
       public static extern  int UCommand1(Byte[] pCmd, ref int parg0, ref int parg1, ref byte parg2);
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
       


        public   bool connectDev(int port)
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


        public  bool findIDCard()
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

        public  bool readIDInfo(out byte[] txtBuff)
        {
            bool ret = false;
            txtBuff = new byte[256];            
            try
            {
               // byte[] cmd = { 0x44 };
                byte[] cmd = { 0x44 };
                int para0 = 1;
                int para1 = 8811;
                byte[] para2 = { 0x02, 0x27, 0x00, 0x00 };    //9986 

                int nRet = UCommand1(cmd, ref para0, ref para1,ref para2[0]);

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


        public  string getNewaddr()
        {
            string  address= "";            
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

        public  byte[] readFileToBytes(string fileFullName)
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


        public  IdInfo ParaseIdInfo()
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
            catch (Exception ex)
            {
                
                return null;
            }

        }

       public  void getsamID(ref byte samid)
        {
            GetSAMIDToStr(ref samid);
            //GetSamID(ref samid);
        }

        //解析二代证信息，BmpPhotoFileName :生成的二代证BMP照片的全路径,以及指纹信息
        public  IdInfo ParaseIDBuff(byte[] txtBuff)
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
        public  string[] Folks = new string[60]
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

        public  void closeDev()
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
}
