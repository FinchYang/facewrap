namespace face
{
    partial class FormFace
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            ReleaseData();
            if (disposing && (components != null))
            {
                components.Dispose();
              //  grabber.Dispose();
               // recognizer.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFace));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.pictureBoxsource = new Emgu.CV.UI.ImageBox();
            this.pictureid = new System.Windows.Forms.PictureBox();
            this.buttonclose = new System.Windows.Forms.Button();
            this.buttonmin = new System.Windows.Forms.Button();
            this.labelscore = new System.Windows.Forms.Label();
            this.labelversion = new System.Windows.Forms.Label();
            this.labeltip = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBoxcapture = new System.Windows.Forms.PictureBox();
            this.groupBoxid = new System.Windows.Forms.GroupBox();
            this.textBoxid = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxnation = new System.Windows.Forms.TextBox();
            this.textBoxname = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxgender = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxresult = new System.Windows.Forms.TextBox();
            this.groupBoxinspect = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxsource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxcapture)).BeginInit();
            this.groupBoxid.SuspendLayout();
            this.groupBoxinspect.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            resources.ApplyResources(this.richTextBox1, "richTextBox1");
            this.richTextBox1.Name = "richTextBox1";
            // 
            // pictureBoxsource
            // 
            this.pictureBoxsource.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            resources.ApplyResources(this.pictureBoxsource, "pictureBoxsource");
            this.pictureBoxsource.Name = "pictureBoxsource";
            this.pictureBoxsource.TabStop = false;
            // 
            // pictureid
            // 
            this.pictureid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            resources.ApplyResources(this.pictureid, "pictureid");
            this.pictureid.Name = "pictureid";
            this.pictureid.TabStop = false;
            // 
            // buttonclose
            // 
            this.buttonclose.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.buttonclose, "buttonclose");
            this.buttonclose.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonclose.Name = "buttonclose";
            this.buttonclose.UseVisualStyleBackColor = false;
            this.buttonclose.Click += new System.EventHandler(this.buttonclose_Click);
            // 
            // buttonmin
            // 
            this.buttonmin.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.buttonmin, "buttonmin");
            this.buttonmin.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonmin.Name = "buttonmin";
            this.buttonmin.UseVisualStyleBackColor = false;
            this.buttonmin.Click += new System.EventHandler(this.buttonmin_Click);
            // 
            // labelscore
            // 
            this.labelscore.BackColor = System.Drawing.Color.Transparent;
            this.labelscore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            resources.ApplyResources(this.labelscore, "labelscore");
            this.labelscore.ForeColor = System.Drawing.Color.Green;
            this.labelscore.Name = "labelscore";
            // 
            // labelversion
            // 
            this.labelversion.BackColor = System.Drawing.Color.Transparent;
            this.labelversion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            resources.ApplyResources(this.labelversion, "labelversion");
            this.labelversion.ForeColor = System.Drawing.Color.DimGray;
            this.labelversion.Name = "labelversion";
            // 
            // labeltip
            // 
            this.labeltip.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labeltip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            resources.ApplyResources(this.labeltip, "labeltip");
            this.labeltip.ForeColor = System.Drawing.Color.DimGray;
            this.labeltip.Name = "labeltip";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBoxcapture
            // 
            this.pictureBoxcapture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            resources.ApplyResources(this.pictureBoxcapture, "pictureBoxcapture");
            this.pictureBoxcapture.Name = "pictureBoxcapture";
            this.pictureBoxcapture.TabStop = false;
            // 
            // groupBoxid
            // 
            this.groupBoxid.Controls.Add(this.textBoxid);
            this.groupBoxid.Controls.Add(this.label5);
            this.groupBoxid.Controls.Add(this.label2);
            this.groupBoxid.Controls.Add(this.textBoxnation);
            this.groupBoxid.Controls.Add(this.textBoxname);
            this.groupBoxid.Controls.Add(this.label4);
            this.groupBoxid.Controls.Add(this.label3);
            this.groupBoxid.Controls.Add(this.textBoxgender);
            resources.ApplyResources(this.groupBoxid, "groupBoxid");
            this.groupBoxid.Name = "groupBoxid";
            this.groupBoxid.TabStop = false;
            // 
            // textBoxid
            // 
            resources.ApplyResources(this.textBoxid, "textBoxid");
            this.textBoxid.Name = "textBoxid";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Name = "label5";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // textBoxnation
            // 
            resources.ApplyResources(this.textBoxnation, "textBoxnation");
            this.textBoxnation.Name = "textBoxnation";
            // 
            // textBoxname
            // 
            resources.ApplyResources(this.textBoxname, "textBoxname");
            this.textBoxname.Name = "textBoxname";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Name = "label3";
            // 
            // textBoxgender
            // 
            resources.ApplyResources(this.textBoxgender, "textBoxgender");
            this.textBoxgender.Name = "textBoxgender";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Name = "label1";
            // 
            // textBoxresult
            // 
            resources.ApplyResources(this.textBoxresult, "textBoxresult");
            this.textBoxresult.Name = "textBoxresult";
            // 
            // groupBoxinspect
            // 
            this.groupBoxinspect.Controls.Add(this.label7);
            this.groupBoxinspect.Controls.Add(this.textBoxresult);
            this.groupBoxinspect.Controls.Add(this.label1);
            this.groupBoxinspect.Controls.Add(this.label6);
            this.groupBoxinspect.Controls.Add(this.pictureBoxcapture);
            this.groupBoxinspect.Controls.Add(this.pictureid);
            resources.ApplyResources(this.groupBoxinspect, "groupBoxinspect");
            this.groupBoxinspect.Name = "groupBoxinspect";
            this.groupBoxinspect.TabStop = false;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.White;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Name = "label6";
            // 
            // FormFace
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.White;
            this.ControlBox = false;
            this.Controls.Add(this.groupBoxinspect);
            this.Controls.Add(this.groupBoxid);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labeltip);
            this.Controls.Add(this.labelversion);
            this.Controls.Add(this.labelscore);
            this.Controls.Add(this.buttonmin);
            this.Controls.Add(this.buttonclose);
            this.Controls.Add(this.pictureBoxsource);
            this.Controls.Add(this.richTextBox1);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFace";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormFace_Paint);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.FormFace_Layout);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxsource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxcapture)).EndInit();
            this.groupBoxid.ResumeLayout(false);
            this.groupBoxid.PerformLayout();
            this.groupBoxinspect.ResumeLayout(false);
            this.groupBoxinspect.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox richTextBox1;
        private Emgu.CV.UI.ImageBox pictureBoxsource;
        private System.Windows.Forms.PictureBox pictureid;
        private System.Windows.Forms.Button buttonclose;
        private System.Windows.Forms.Button buttonmin;
        private System.Windows.Forms.Label labelscore;
        private System.Windows.Forms.Label labelversion;
        private System.Windows.Forms.Label labeltip;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBoxcapture;
        private System.Windows.Forms.GroupBox groupBoxid;
        private System.Windows.Forms.TextBox textBoxid;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxnation;
        private System.Windows.Forms.TextBox textBoxname;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxgender;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxresult;
        private System.Windows.Forms.GroupBox groupBoxinspect;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
    }
}

