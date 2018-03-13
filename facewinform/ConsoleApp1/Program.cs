using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        [DllImport(@"C:\dev\FaceServer\facewinform\idr210sdk\desktop\sdtapi.dll")]
        public extern static int InitComm(int iPort);
        [DllImport(@"C:\dev\FaceServer\facewinform\idr210sdk\desktop\sdtapi.dll")]
        public extern static int CloseComm();
        [DllImport(@"C:\dev\FaceServer\facewinform\idr210sdk\desktop\sdtapi.dll")]
        public extern static int Routon_IC_HL_ReadCardSN(StringBuilder SN);
        [DllImport(@"C:\dev\FaceServer\facewinform\idr210sdk\desktop\sdtapi.dll")]
        public extern static int Routon_IC_HL_ReadCard(int SID, int BID, int KeyType, byte[] Key, byte[] data);


        [DllImport(@"C:\dev\FaceServer\facewinform\idr210sdk\desktop\sdtapi.dll")]
        public extern static int Authenticate();

        [DllImport(@"C:\dev\FaceServer\facewinform\idr210sdk\desktop\sdtapi.dll")]
        public extern static int ReadBaseMsg(byte[] pMsg, int len);

        [DllImport(@"C:\dev\FaceServer\facewinform\idr210sdk\desktop\sdtapi.dll")]
        public extern static int ReadNewAppMsg(byte[] pMsg, out int len);
        static void Main(string[] args)
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
                            //显示文字及图片信息
                            Console.WriteLine(Msg);
                            var name = Msg.Take(31).ToArray();
                            Console.WriteLine(System.Text.Encoding.Default.GetString(Msg.Take(31).ToArray()));
                            Console.WriteLine(System.Text.Encoding.Default.GetString(Msg.Skip(31).Take(3).ToArray()));
                            Console.WriteLine(System.Text.Encoding.Default.GetString(Msg.Skip(34).Take(10).ToArray()));
                            Console.WriteLine(System.Text.Encoding.Default.GetString(Msg.Skip(44).Take(9).ToArray()));
                            Console.WriteLine(System.Text.Encoding.Default.GetString(Msg.Skip(53).Take(71).ToArray()));
                            Console.WriteLine(System.Text.Encoding.Default.GetString(Msg.Skip(124).Take(19).ToArray()));
                            Console.WriteLine(System.Text.Encoding.Default.GetString(Msg.Skip(143).Take(31).ToArray()));
                            Console.WriteLine(System.Text.Encoding.Default.GetString(Msg.Skip(174).Take(9).ToArray()));
                            Console.WriteLine(System.Text.Encoding.Default.GetString(Msg.Skip(183).Take(9).ToArray()));
                            
                        }
                       
                        //// char Msg1[200];
                        //var Msg1 = new byte[200];
                        ////int num;
                        //ret = ReadNewAppMsg(Msg1, out int num);
                        //if (ret > 0)
                        //{
                        //    //显示追加地址信息
                        //    Console.WriteLine("{0},{1}", Msg, num);
                        //}
                    }
                    else
                    {
                        Console.WriteLine("please put your id card");
                        System.Threading.Thread.Sleep(100);
                        continue;
                    }
                } while (ok);
            }

            ret = CloseComm();
            Console.ReadLine();
        }
    }
}
