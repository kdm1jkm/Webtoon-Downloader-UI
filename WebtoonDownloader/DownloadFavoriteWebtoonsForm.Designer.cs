namespace WebtoonDownloader
{
    partial class DownloadFavoriteWebtoonsForm
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
            this.tmpk_from = new System.Windows.Forms.DateTimePicker();
            this.tmpk_to = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_download = new System.Windows.Forms.Button();
            this.checkBox_zip = new System.Windows.Forms.CheckBox();
            this.checkBox_HTML = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmpk_from
            // 
            this.tmpk_from.Location = new System.Drawing.Point(6, 20);
            this.tmpk_from.Name = "tmpk_from";
            this.tmpk_from.Size = new System.Drawing.Size(200, 21);
            this.tmpk_from.TabIndex = 1;
            // 
            // tmpk_to
            // 
            this.tmpk_to.Location = new System.Drawing.Point(264, 22);
            this.tmpk_to.Name = "tmpk_to";
            this.tmpk_to.Size = new System.Drawing.Size(200, 21);
            this.tmpk_to.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tmpk_from);
            this.groupBox1.Controls.Add(this.tmpk_to);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(503, 53);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "날자 범위 설정";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(470, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "까지";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(213, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "부터";
            // 
            // btn_download
            // 
            this.btn_download.Location = new System.Drawing.Point(209, 91);
            this.btn_download.Name = "btn_download";
            this.btn_download.Size = new System.Drawing.Size(112, 23);
            this.btn_download.TabIndex = 6;
            this.btn_download.Text = "대기 목록에 추가";
            this.btn_download.UseVisualStyleBackColor = true;
            this.btn_download.Click += new System.EventHandler(this.btn_download_Click);
            // 
            // checkBox_zip
            // 
            this.checkBox_zip.AutoSize = true;
            this.checkBox_zip.Location = new System.Drawing.Point(276, 71);
            this.checkBox_zip.Name = "checkBox_zip";
            this.checkBox_zip.Size = new System.Drawing.Size(89, 16);
            this.checkBox_zip.TabIndex = 9;
            this.checkBox_zip.Text = "zip(mobile)";
            this.checkBox_zip.UseVisualStyleBackColor = true;
            // 
            // checkBox_HTML
            // 
            this.checkBox_HTML.AutoSize = true;
            this.checkBox_HTML.Location = new System.Drawing.Point(161, 71);
            this.checkBox_HTML.Name = "checkBox_HTML";
            this.checkBox_HTML.Size = new System.Drawing.Size(75, 16);
            this.checkBox_HTML.TabIndex = 10;
            this.checkBox_HTML.Text = "html(PC)";
            this.checkBox_HTML.UseVisualStyleBackColor = true;
            // 
            // DownloadFavoriteWebtoonsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 126);
            this.Controls.Add(this.checkBox_zip);
            this.Controls.Add(this.checkBox_HTML);
            this.Controls.Add(this.btn_download);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DownloadFavoriteWebtoonsForm";
            this.Text = "DownloadFavoriteWebtoonsForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DateTimePicker tmpk_from;
        private System.Windows.Forms.DateTimePicker tmpk_to;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_download;
        private System.Windows.Forms.CheckBox checkBox_zip;
        private System.Windows.Forms.CheckBox checkBox_HTML;
    }
}