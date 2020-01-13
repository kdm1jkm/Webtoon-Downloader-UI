namespace WebtoonDownloader
{
    partial class FileManagementForm
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
            this.cLstBx_webtoonList = new System.Windows.Forms.CheckedListBox();
            this.btn_convertHtml = new System.Windows.Forms.Button();
            this.btn_convertZip = new System.Windows.Forms.Button();
            this.btn_delete = new System.Windows.Forms.Button();
            this.checkBox_SelectAll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cLstBx_webtoonList
            // 
            this.cLstBx_webtoonList.FormattingEnabled = true;
            this.cLstBx_webtoonList.Location = new System.Drawing.Point(12, 12);
            this.cLstBx_webtoonList.Name = "cLstBx_webtoonList";
            this.cLstBx_webtoonList.Size = new System.Drawing.Size(345, 324);
            this.cLstBx_webtoonList.TabIndex = 0;
            this.cLstBx_webtoonList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cLstBx_webtoonList_KeyPress);
            this.cLstBx_webtoonList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cLstBx_webtoonList_MouseUp);
            // 
            // btn_convertHtml
            // 
            this.btn_convertHtml.Location = new System.Drawing.Point(363, 12);
            this.btn_convertHtml.Name = "btn_convertHtml";
            this.btn_convertHtml.Size = new System.Drawing.Size(180, 54);
            this.btn_convertHtml.TabIndex = 1;
            this.btn_convertHtml.Text = "html(pc용)로 내보내기";
            this.btn_convertHtml.UseVisualStyleBackColor = true;
            this.btn_convertHtml.Click += new System.EventHandler(this.btn_convertHtml_Click);
            // 
            // btn_convertZip
            // 
            this.btn_convertZip.Location = new System.Drawing.Point(363, 72);
            this.btn_convertZip.Name = "btn_convertZip";
            this.btn_convertZip.Size = new System.Drawing.Size(180, 54);
            this.btn_convertZip.TabIndex = 2;
            this.btn_convertZip.Text = "zip(mobile)로 내보내기";
            this.btn_convertZip.UseVisualStyleBackColor = true;
            this.btn_convertZip.Click += new System.EventHandler(this.btn_convertZip_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.Location = new System.Drawing.Point(468, 315);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(75, 23);
            this.btn_delete.TabIndex = 3;
            this.btn_delete.Text = "삭제하기";
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // checkBox_SelectAll
            // 
            this.checkBox_SelectAll.AutoSize = true;
            this.checkBox_SelectAll.Location = new System.Drawing.Point(363, 319);
            this.checkBox_SelectAll.Name = "checkBox_SelectAll";
            this.checkBox_SelectAll.Size = new System.Drawing.Size(72, 16);
            this.checkBox_SelectAll.TabIndex = 4;
            this.checkBox_SelectAll.Text = "전체선택";
            this.checkBox_SelectAll.UseVisualStyleBackColor = true;
            this.checkBox_SelectAll.Click += new System.EventHandler(this.checkBox_SelectAll_Click);
            // 
            // FileManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 350);
            this.Controls.Add(this.checkBox_SelectAll);
            this.Controls.Add(this.btn_delete);
            this.Controls.Add(this.btn_convertZip);
            this.Controls.Add(this.btn_convertHtml);
            this.Controls.Add(this.cLstBx_webtoonList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FileManagementForm";
            this.Text = "FileManagementForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox cLstBx_webtoonList;
        private System.Windows.Forms.Button btn_convertHtml;
        private System.Windows.Forms.Button btn_convertZip;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.CheckBox checkBox_SelectAll;
    }
}