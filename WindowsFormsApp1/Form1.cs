using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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
        private void button1_Click(object sender, EventArgs e)
        {
            var id = "37900919750819723X";
            UpdateStatus(string.Format("---{0}", 777));
            var lipath = Path.Combine(homepath, "what");
            UpdateStatus(string.Format("-{1}--{0}", 888, lipath));
            if (!Directory.Exists(lipath)) Directory.CreateDirectory(lipath);
            UpdateStatus(string.Format("-{1}--{0}", 999, id));
            var localimage = Path.Combine(lipath, id);// + ".jpg";
            UpdateStatus(string.Format("---{0}", 222));
        }
    }
}
