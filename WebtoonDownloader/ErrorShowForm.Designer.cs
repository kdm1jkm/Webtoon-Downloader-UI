namespace WebtoonDownloader
{
    partial class ErrorShowForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.tBox_ErrorMessege = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 346);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(497, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "오류 발생시 하던 행동과 오류 메세지를 제작자에게 전해주시면 오류 수정에 도움이 됩니다.";
            // 
            // tBox_ErrorMessege
            // 
            this.tBox_ErrorMessege.BackColor = System.Drawing.SystemColors.Window;
            this.tBox_ErrorMessege.Location = new System.Drawing.Point(12, 12);
            this.tBox_ErrorMessege.Name = "tBox_ErrorMessege";
            this.tBox_ErrorMessege.ReadOnly = true;
            this.tBox_ErrorMessege.Size = new System.Drawing.Size(539, 331);
            this.tBox_ErrorMessege.TabIndex = 2;
            this.tBox_ErrorMessege.Text = "";
            // 
            // ErrorShowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 365);
            this.Controls.Add(this.tBox_ErrorMessege);
            this.Controls.Add(this.label1);
            this.Name = "ErrorShowForm";
            this.Text = "ErrorShowForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox tBox_ErrorMessege;
    }
}