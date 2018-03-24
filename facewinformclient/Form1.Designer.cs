namespace face
{
    partial class Form1
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttoncompare = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.pictureBoxsource = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonstopcapture = new System.Windows.Forms.Button();
            this.buttonreadid = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonrestart = new System.Windows.Forms.Button();
            this.pictureBoxcurrentimage = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.picturecapture2 = new System.Windows.Forms.PictureBox();
            this.picturecapture1 = new System.Windows.Forms.PictureBox();
            this.pictureid = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBoxname = new System.Windows.Forms.TextBox();
            this.textBoxid = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.buttongetresult = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxsource)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxcurrentimage)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturecapture2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturecapture1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureid)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttoncompare
            // 
            resources.ApplyResources(this.buttoncompare, "buttoncompare");
            this.buttoncompare.Name = "buttoncompare";
            this.buttoncompare.UseVisualStyleBackColor = true;
            this.buttoncompare.Click += new System.EventHandler(this.buttoncompare_Click);
            // 
            // richTextBox1
            // 
            resources.ApplyResources(this.richTextBox1, "richTextBox1");
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // pictureBoxsource
            // 
            resources.ApplyResources(this.pictureBoxsource, "pictureBoxsource");
            this.pictureBoxsource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxsource.Name = "pictureBoxsource";
            this.pictureBoxsource.TabStop = false;
            this.pictureBoxsource.Click += new System.EventHandler(this.pictureBoxsource_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click_1);
            // 
            // buttonstopcapture
            // 
            resources.ApplyResources(this.buttonstopcapture, "buttonstopcapture");
            this.buttonstopcapture.Name = "buttonstopcapture";
            this.buttonstopcapture.UseVisualStyleBackColor = true;
            this.buttonstopcapture.Click += new System.EventHandler(this.buttonstopcapture_Click);
            // 
            // buttonreadid
            // 
            resources.ApplyResources(this.buttonreadid, "buttonreadid");
            this.buttonreadid.Name = "buttonreadid";
            this.buttonreadid.UseVisualStyleBackColor = true;
            this.buttonreadid.Click += new System.EventHandler(this.buttonreadid_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonrestart
            // 
            resources.ApplyResources(this.buttonrestart, "buttonrestart");
            this.buttonrestart.Name = "buttonrestart";
            this.buttonrestart.UseVisualStyleBackColor = true;
            this.buttonrestart.Click += new System.EventHandler(this.buttonrestart_Click);
            // 
            // pictureBoxcurrentimage
            // 
            resources.ApplyResources(this.pictureBoxcurrentimage, "pictureBoxcurrentimage");
            this.pictureBoxcurrentimage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxcurrentimage.Name = "pictureBoxcurrentimage";
            this.pictureBoxcurrentimage.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonreadid);
            this.groupBox1.Controls.Add(this.buttoncompare);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.picturecapture2);
            this.groupBox2.Controls.Add(this.picturecapture1);
            this.groupBox2.Controls.Add(this.pictureBoxcurrentimage);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // picturecapture2
            // 
            resources.ApplyResources(this.picturecapture2, "picturecapture2");
            this.picturecapture2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picturecapture2.Name = "picturecapture2";
            this.picturecapture2.TabStop = false;
            // 
            // picturecapture1
            // 
            resources.ApplyResources(this.picturecapture1, "picturecapture1");
            this.picturecapture1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picturecapture1.Name = "picturecapture1";
            this.picturecapture1.TabStop = false;
            // 
            // pictureid
            // 
            resources.ApplyResources(this.pictureid, "pictureid");
            this.pictureid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureid.Name = "pictureid";
            this.pictureid.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.pictureBoxsource);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.pictureid);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.buttongetresult);
            this.groupBox5.Controls.Add(this.textBoxname);
            this.groupBox5.Controls.Add(this.textBoxid);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.button1);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // textBoxname
            // 
            resources.ApplyResources(this.textBoxname, "textBoxname");
            this.textBoxname.Name = "textBoxname";
            // 
            // textBoxid
            // 
            resources.ApplyResources(this.textBoxid, "textBoxid");
            this.textBoxid.Name = "textBoxid";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Name = "label1";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.buttonstopcapture);
            this.groupBox6.Controls.Add(this.buttonrestart);
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // buttongetresult
            // 
            resources.ApplyResources(this.buttongetresult, "buttongetresult");
            this.buttongetresult.Name = "buttongetresult";
            this.buttongetresult.UseVisualStyleBackColor = true;
            this.buttongetresult.Click += new System.EventHandler(this.buttongetresult_Click);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.menuStrip1);
            this.ForeColor = System.Drawing.SystemColors.Highlight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.HelpButton = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxsource)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxcurrentimage)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picturecapture2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturecapture1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureid)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttoncompare;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.PictureBox pictureBoxsource;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.Button buttonstopcapture;
        private System.Windows.Forms.Button buttonrestart;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonreadid;
        private System.Windows.Forms.PictureBox pictureBoxcurrentimage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pictureid;
        private System.Windows.Forms.PictureBox picturecapture2;
        private System.Windows.Forms.PictureBox picturecapture1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox textBoxname;
        private System.Windows.Forms.TextBox textBoxid;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button buttongetresult;
    }
}

