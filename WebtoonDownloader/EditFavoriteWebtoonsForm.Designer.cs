namespace WebtoonDownloader
{
    partial class EditFavoriteWebtoonsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cLstBox_WebtoonList = new System.Windows.Forms.CheckedListBox();
            this.btn_saveToNaverAcnt = new System.Windows.Forms.Button();
            this.btn_LoadFromNaverAcnt = new System.Windows.Forms.Button();
            this.btn_saveAndExit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cLstBox_WebtoonList
            // 
            this.cLstBox_WebtoonList.FormattingEnabled = true;
            this.cLstBox_WebtoonList.Location = new System.Drawing.Point(12, 12);
            this.cLstBox_WebtoonList.Name = "cLstBox_WebtoonList";
            this.cLstBox_WebtoonList.Size = new System.Drawing.Size(348, 228);
            this.cLstBox_WebtoonList.TabIndex = 0;
            // 
            // btn_saveToNaverAcnt
            // 
            this.btn_saveToNaverAcnt.Location = new System.Drawing.Point(12, 246);
            this.btn_saveToNaverAcnt.Name = "btn_saveToNaverAcnt";
            this.btn_saveToNaverAcnt.Size = new System.Drawing.Size(169, 28);
            this.btn_saveToNaverAcnt.TabIndex = 1;
            this.btn_saveToNaverAcnt.Text = "네이버 계정에 저장하기";
            this.btn_saveToNaverAcnt.UseVisualStyleBackColor = true;
            this.btn_saveToNaverAcnt.Click += new System.EventHandler(this.NotDevelopedMessage);
            // 
            // btn_LoadFromNaverAcnt
            // 
            this.btn_LoadFromNaverAcnt.Location = new System.Drawing.Point(12, 280);
            this.btn_LoadFromNaverAcnt.Name = "btn_LoadFromNaverAcnt";
            this.btn_LoadFromNaverAcnt.Size = new System.Drawing.Size(169, 28);
            this.btn_LoadFromNaverAcnt.TabIndex = 1;
            this.btn_LoadFromNaverAcnt.Text = "네이버 계정에서 불러오기";
            this.btn_LoadFromNaverAcnt.UseVisualStyleBackColor = true;
            this.btn_LoadFromNaverAcnt.Click += new System.EventHandler(this.btn_LoadFromNaverAcnt_Click);
            // 
            // btn_saveAndExit
            // 
            this.btn_saveAndExit.Location = new System.Drawing.Point(733, 283);
            this.btn_saveAndExit.Name = "btn_saveAndExit";
            this.btn_saveAndExit.Size = new System.Drawing.Size(90, 23);
            this.btn_saveAndExit.TabIndex = 2;
            this.btn_saveAndExit.Text = "저장 및 종료";
            this.btn_saveAndExit.UseVisualStyleBackColor = true;
            this.btn_saveAndExit.Click += new System.EventHandler(this.btn_saveAndExit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(366, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(457, 130);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "웹툰 정보";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(6, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(125, 101);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(137, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(229, 60);
            this.label1.TabIndex = 1;
            this.label1.Text = "연애혁명|232[목]\r\n로맨스, 그런 건 우리에게 있을 수가 없어!\r\n신개념 개그 로맨스\r\n\r\n스토리, 개그, 드라마";
            // 
            // EditFavoriteWebtoonsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(835, 318);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_saveAndExit);
            this.Controls.Add(this.btn_LoadFromNaverAcnt);
            this.Controls.Add(this.btn_saveToNaverAcnt);
            this.Controls.Add(this.cLstBox_WebtoonList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "EditFavoriteWebtoonsForm";
            this.ShowInTaskbar = false;
            this.Text = "EditFavoriteWebtoonsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditFavoriteWebtoonsForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox cLstBox_WebtoonList;
        private System.Windows.Forms.Button btn_saveToNaverAcnt;
        private System.Windows.Forms.Button btn_LoadFromNaverAcnt;
        private System.Windows.Forms.Button btn_saveAndExit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}