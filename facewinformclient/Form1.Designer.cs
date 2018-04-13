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
            this.buttoncompare = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.pictureBoxsource = new Emgu.CV.UI.ImageBox();
            this.buttonstopcapture = new System.Windows.Forms.Button();
            this.buttonreadid = new System.Windows.Forms.Button();
            this.buttoncloudcompare = new System.Windows.Forms.Button();
            this.buttonrestart = new System.Windows.Forms.Button();
            this.pictureBoxcurrentimage = new System.Windows.Forms.PictureBox();
            this.picturecapture2 = new System.Windows.Forms.PictureBox();
            this.picturecapture1 = new System.Windows.Forms.PictureBox();
            this.pictureid = new System.Windows.Forms.PictureBox();
            this.buttongetresult = new System.Windows.Forms.Button();
            this.textBoxname = new System.Windows.Forms.TextBox();
            this.textBoxid = new System.Windows.Forms.TextBox();
            this.buttonnoid = new System.Windows.Forms.Button();
            this.buttonhaveid = new System.Windows.Forms.Button();
            this.buttonclose = new System.Windows.Forms.Button();
            this.buttonmin = new System.Windows.Forms.Button();
            this.labelscore = new System.Windows.Forms.Label();
            this.labelversion = new System.Windows.Forms.Label();
            this.labeltip = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxsource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxcurrentimage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturecapture2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturecapture1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureid)).BeginInit();
            this.SuspendLayout();
            // 
            // buttoncompare
            // 
            this.buttoncompare.BackColor = System.Drawing.Color.LimeGreen;
            resources.ApplyResources(this.buttoncompare, "buttoncompare");
            this.buttoncompare.FlatAppearance.BorderColor = System.Drawing.Color.Green;
            this.buttoncompare.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.buttoncompare.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Green;
            this.buttoncompare.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.buttoncompare.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttoncompare.Name = "buttoncompare";
            this.buttoncompare.UseVisualStyleBackColor = false;
            this.buttoncompare.Click += new System.EventHandler(this.buttoncompare_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            resources.ApplyResources(this.richTextBox1, "richTextBox1");
            this.richTextBox1.Name = "richTextBox1";
            // 
            // pictureBoxsource
            // 
            this.pictureBoxsource.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.pictureBoxsource, "pictureBoxsource");
            this.pictureBoxsource.Name = "pictureBoxsource";
            this.pictureBoxsource.TabStop = false;
            // 
            // buttonstopcapture
            // 
            this.buttonstopcapture.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
            this.buttonstopcapture.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.buttonstopcapture, "buttonstopcapture");
            this.buttonstopcapture.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
            this.buttonstopcapture.FlatAppearance.BorderSize = 10;
            this.buttonstopcapture.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.buttonstopcapture.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Blue;
            this.buttonstopcapture.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.buttonstopcapture.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonstopcapture.Name = "buttonstopcapture";
            this.buttonstopcapture.UseVisualStyleBackColor = false;
            this.buttonstopcapture.Click += new System.EventHandler(this.buttonstopcapture_Click);
            // 
            // buttonreadid
            // 
            this.buttonreadid.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.buttonreadid, "buttonreadid");
            this.buttonreadid.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
            this.buttonreadid.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.buttonreadid.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.buttonreadid.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonreadid.Name = "buttonreadid";
            this.buttonreadid.UseVisualStyleBackColor = false;
            this.buttonreadid.Click += new System.EventHandler(this.buttonreadid_Click);
            // 
            // buttoncloudcompare
            // 
            this.buttoncloudcompare.BackColor = System.Drawing.Color.LimeGreen;
            resources.ApplyResources(this.buttoncloudcompare, "buttoncloudcompare");
            this.buttoncloudcompare.FlatAppearance.BorderColor = System.Drawing.Color.Green;
            this.buttoncloudcompare.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.buttoncloudcompare.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Green;
            this.buttoncloudcompare.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.buttoncloudcompare.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttoncloudcompare.Name = "buttoncloudcompare";
            this.buttoncloudcompare.UseVisualStyleBackColor = false;
            this.buttoncloudcompare.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonrestart
            // 
            this.buttonrestart.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.buttonrestart, "buttonrestart");
            this.buttonrestart.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
            this.buttonrestart.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.buttonrestart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.buttonrestart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.buttonrestart.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonrestart.Name = "buttonrestart";
            this.buttonrestart.UseVisualStyleBackColor = false;
            this.buttonrestart.Click += new System.EventHandler(this.buttonrestart_Click);
            // 
            // pictureBoxcurrentimage
            // 
            this.pictureBoxcurrentimage.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.pictureBoxcurrentimage, "pictureBoxcurrentimage");
            this.pictureBoxcurrentimage.Name = "pictureBoxcurrentimage";
            this.pictureBoxcurrentimage.TabStop = false;
            // 
            // picturecapture2
            // 
            this.picturecapture2.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.picturecapture2, "picturecapture2");
            this.picturecapture2.Name = "picturecapture2";
            this.picturecapture2.TabStop = false;
            // 
            // picturecapture1
            // 
            this.picturecapture1.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.picturecapture1, "picturecapture1");
            this.picturecapture1.Name = "picturecapture1";
            this.picturecapture1.TabStop = false;
            // 
            // pictureid
            // 
            this.pictureid.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.pictureid, "pictureid");
            this.pictureid.Name = "pictureid";
            this.pictureid.TabStop = false;
            // 
            // buttongetresult
            // 
            this.buttongetresult.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.buttongetresult, "buttongetresult");
            this.buttongetresult.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
            this.buttongetresult.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.buttongetresult.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.buttongetresult.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttongetresult.Name = "buttongetresult";
            this.buttongetresult.UseVisualStyleBackColor = false;
            this.buttongetresult.Click += new System.EventHandler(this.buttongetresult_Click);
            // 
            // textBoxname
            // 
            this.textBoxname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.textBoxname, "textBoxname");
            this.textBoxname.Name = "textBoxname";
            // 
            // textBoxid
            // 
            resources.ApplyResources(this.textBoxid, "textBoxid");
            this.textBoxid.Name = "textBoxid";
            // 
            // buttonnoid
            // 
            this.buttonnoid.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.buttonnoid, "buttonnoid");
            this.buttonnoid.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonnoid.Name = "buttonnoid";
            this.buttonnoid.UseVisualStyleBackColor = false;
            this.buttonnoid.Click += new System.EventHandler(this.buttonnoid_Click);
            // 
            // buttonhaveid
            // 
            this.buttonhaveid.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.buttonhaveid, "buttonhaveid");
            this.buttonhaveid.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonhaveid.Name = "buttonhaveid";
            this.buttonhaveid.UseVisualStyleBackColor = false;
            this.buttonhaveid.Click += new System.EventHandler(this.buttonhaveid_Click);
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
            // FormFace
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.GreenYellow;
            this.ControlBox = false;
            this.Controls.Add(this.labeltip);
            this.Controls.Add(this.labelversion);
            this.Controls.Add(this.labelscore);
            this.Controls.Add(this.buttonmin);
            this.Controls.Add(this.buttonclose);
            this.Controls.Add(this.buttonhaveid);
            this.Controls.Add(this.textBoxname);
            this.Controls.Add(this.buttongetresult);
            this.Controls.Add(this.textBoxid);
            this.Controls.Add(this.buttonnoid);
            this.Controls.Add(this.buttonstopcapture);
            this.Controls.Add(this.buttoncompare);
            this.Controls.Add(this.buttonrestart);
            this.Controls.Add(this.buttoncloudcompare);
            this.Controls.Add(this.buttonreadid);
            this.Controls.Add(this.picturecapture2);
            this.Controls.Add(this.pictureid);
            this.Controls.Add(this.picturecapture1);
            this.Controls.Add(this.pictureBoxsource);
            this.Controls.Add(this.pictureBoxcurrentimage);
            this.Controls.Add(this.richTextBox1);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ForeColor = System.Drawing.Color.LawnGreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFace";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxsource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxcurrentimage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturecapture2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturecapture1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttoncompare;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private Emgu.CV.UI.ImageBox pictureBoxsource;
        private System.Windows.Forms.Button buttonstopcapture;
        private System.Windows.Forms.Button buttonrestart;
        private System.Windows.Forms.Button buttoncloudcompare;
        private System.Windows.Forms.Button buttonreadid;
        private System.Windows.Forms.PictureBox pictureBoxcurrentimage;
        private System.Windows.Forms.PictureBox pictureid;
        private System.Windows.Forms.PictureBox picturecapture2;
        private System.Windows.Forms.PictureBox picturecapture1;
        private System.Windows.Forms.TextBox textBoxname;
        private System.Windows.Forms.TextBox textBoxid;
        private System.Windows.Forms.Button buttongetresult;
        private System.Windows.Forms.Button buttonnoid;
        private System.Windows.Forms.Button buttonhaveid;
        private System.Windows.Forms.Button buttonclose;
        private System.Windows.Forms.Button buttonmin;
        private System.Windows.Forms.Label labelscore;
        private System.Windows.Forms.Label labelversion;
        private System.Windows.Forms.Label labeltip;
    }
}

