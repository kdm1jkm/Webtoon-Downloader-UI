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
            this.btn_saveAndExit.Location = new System.Drawing.Point(269, 283);
            this.btn_saveAndExit.Name = "btn_saveAndExit";
            this.btn_saveAndExit.Size = new System.Drawing.Size(90, 23);
            this.btn_saveAndExit.TabIndex = 2;
            this.btn_saveAndExit.Text = "저장 및 종료";
            this.btn_saveAndExit.UseVisualStyleBackColor = true;
            this.btn_saveAndExit.Click += new System.EventHandler(this.btn_saveAndExit_Click);
            // 
            // EditFavoriteWebtoonsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(371, 318);
            this.Controls.Add(this.btn_saveAndExit);
            this.Controls.Add(this.btn_LoadFromNaverAcnt);
            this.Controls.Add(this.btn_saveToNaverAcnt);
            this.Controls.Add(this.cLstBox_WebtoonList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "EditFavoriteWebtoonsForm";
            this.ShowInTaskbar = false;
            this.Text = "EditFavoriteWebtoonsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditFavoriteWebtoonsForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox cLstBox_WebtoonList;
        private System.Windows.Forms.Button btn_saveToNaverAcnt;
        private System.Windows.Forms.Button btn_LoadFromNaverAcnt;
        private System.Windows.Forms.Button btn_saveAndExit;
    }
}