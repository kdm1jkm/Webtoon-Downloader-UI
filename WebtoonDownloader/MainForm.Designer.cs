namespace WebtoonDownloader
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.tBox_WebtoonName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tBox_titleId = new System.Windows.Forms.TextBox();
            this.num_StartNo = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.num_endNo = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBox_SelectAll = new System.Windows.Forms.CheckBox();
            this.btn_FileManagement = new System.Windows.Forms.Button();
            this.btn_ModifyFavorite = new System.Windows.Forms.Button();
            this.btn_AddFavorite = new System.Windows.Forms.Button();
            this.btn_AddTask = new System.Windows.Forms.Button();
            this.checkBox_HTML = new System.Windows.Forms.CheckBox();
            this.checkBox_zip = new System.Windows.Forms.CheckBox();
            this.btn_Search = new System.Windows.Forms.Button();
            this.btn_TogglePause = new System.Windows.Forms.Button();
            this.lBox_queue = new System.Windows.Forms.ListBox();
            this.lBox_DownloadedWebtoons = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_clearDownloadedList = new System.Windows.Forms.Button();
            this.btn_clrQueue = new System.Windows.Forms.Button();
            this.prgsBar_Webtoon = new System.Windows.Forms.ProgressBar();
            this.label7 = new System.Windows.Forms.Label();
            this.num_Thread = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.num_StartNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_endNo)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_Thread)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "웹툰명";
            // 
            // tBox_WebtoonName
            // 
            this.tBox_WebtoonName.Location = new System.Drawing.Point(59, 12);
            this.tBox_WebtoonName.Name = "tBox_WebtoonName";
            this.tBox_WebtoonName.Size = new System.Drawing.Size(97, 21);
            this.tBox_WebtoonName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "titleId";
            // 
            // tBox_titleId
            // 
            this.tBox_titleId.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.tBox_titleId.Location = new System.Drawing.Point(59, 38);
            this.tBox_titleId.Name = "tBox_titleId";
            this.tBox_titleId.Size = new System.Drawing.Size(151, 21);
            this.tBox_titleId.TabIndex = 3;
            this.tBox_titleId.TextChanged += new System.EventHandler(this.tBox_titleId_TextChanged);
            this.tBox_titleId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.InputOnlyNum);
            this.tBox_titleId.ImeModeChanged += new System.EventHandler(this.tBox_titleId_ImeModeChanged);
            // 
            // num_StartNo
            // 
            this.num_StartNo.Location = new System.Drawing.Point(13, 65);
            this.num_StartNo.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_StartNo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_StartNo.Name = "num_StartNo";
            this.num_StartNo.Size = new System.Drawing.Size(40, 21);
            this.num_StartNo.TabIndex = 4;
            this.num_StartNo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_StartNo.ValueChanged += new System.EventHandler(this.num_StartNo_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(57, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "화 ~";
            // 
            // num_endNo
            // 
            this.num_endNo.Location = new System.Drawing.Point(93, 65);
            this.num_endNo.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_endNo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_endNo.Name = "num_endNo";
            this.num_endNo.Size = new System.Drawing.Size(40, 21);
            this.num_endNo.TabIndex = 4;
            this.num_endNo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_endNo.ValueChanged += new System.EventHandler(this.num_endNo_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(139, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "화";
            // 
            // checkBox_SelectAll
            // 
            this.checkBox_SelectAll.AutoSize = true;
            this.checkBox_SelectAll.Location = new System.Drawing.Point(162, 68);
            this.checkBox_SelectAll.Name = "checkBox_SelectAll";
            this.checkBox_SelectAll.Size = new System.Drawing.Size(48, 16);
            this.checkBox_SelectAll.TabIndex = 6;
            this.checkBox_SelectAll.Text = "전체";
            this.checkBox_SelectAll.UseVisualStyleBackColor = true;
            this.checkBox_SelectAll.CheckedChanged += new System.EventHandler(this.checkBox_SelectAll_CheckedChanged);
            // 
            // btn_FileManagement
            // 
            this.btn_FileManagement.Location = new System.Drawing.Point(239, 12);
            this.btn_FileManagement.Name = "btn_FileManagement";
            this.btn_FileManagement.Size = new System.Drawing.Size(106, 21);
            this.btn_FileManagement.TabIndex = 7;
            this.btn_FileManagement.Text = "파일관리";
            this.btn_FileManagement.UseVisualStyleBackColor = true;
            this.btn_FileManagement.Click += new System.EventHandler(this.btn_FileManagement_Click);
            // 
            // btn_ModifyFavorite
            // 
            this.btn_ModifyFavorite.Location = new System.Drawing.Point(239, 63);
            this.btn_ModifyFavorite.Name = "btn_ModifyFavorite";
            this.btn_ModifyFavorite.Size = new System.Drawing.Size(106, 21);
            this.btn_ModifyFavorite.TabIndex = 7;
            this.btn_ModifyFavorite.Text = "관심웹툰 수정";
            this.btn_ModifyFavorite.UseVisualStyleBackColor = true;
            this.btn_ModifyFavorite.Click += new System.EventHandler(this.btn_ModifyFavorite_Click);
            // 
            // btn_AddFavorite
            // 
            this.btn_AddFavorite.Location = new System.Drawing.Point(239, 90);
            this.btn_AddFavorite.Name = "btn_AddFavorite";
            this.btn_AddFavorite.Size = new System.Drawing.Size(106, 21);
            this.btn_AddFavorite.TabIndex = 7;
            this.btn_AddFavorite.Text = "관심웹툰 다운";
            this.btn_AddFavorite.UseVisualStyleBackColor = true;
            this.btn_AddFavorite.Click += new System.EventHandler(this.btn_AddFavorite_Click);
            // 
            // btn_AddTask
            // 
            this.btn_AddTask.Location = new System.Drawing.Point(12, 90);
            this.btn_AddTask.Name = "btn_AddTask";
            this.btn_AddTask.Size = new System.Drawing.Size(198, 21);
            this.btn_AddTask.TabIndex = 7;
            this.btn_AddTask.Text = "대기목록에 추가";
            this.btn_AddTask.UseVisualStyleBackColor = true;
            this.btn_AddTask.Click += new System.EventHandler(this.btn_AddTask_Click);
            // 
            // checkBox_HTML
            // 
            this.checkBox_HTML.AutoSize = true;
            this.checkBox_HTML.Location = new System.Drawing.Point(15, 117);
            this.checkBox_HTML.Name = "checkBox_HTML";
            this.checkBox_HTML.Size = new System.Drawing.Size(75, 16);
            this.checkBox_HTML.TabIndex = 8;
            this.checkBox_HTML.Text = "html(PC)";
            this.checkBox_HTML.UseVisualStyleBackColor = true;
            // 
            // checkBox_zip
            // 
            this.checkBox_zip.AutoSize = true;
            this.checkBox_zip.Location = new System.Drawing.Point(121, 117);
            this.checkBox_zip.Name = "checkBox_zip";
            this.checkBox_zip.Size = new System.Drawing.Size(89, 16);
            this.checkBox_zip.TabIndex = 8;
            this.checkBox_zip.Text = "zip(mobile)";
            this.checkBox_zip.UseVisualStyleBackColor = true;
            // 
            // btn_Search
            // 
            this.btn_Search.Location = new System.Drawing.Point(162, 12);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(48, 21);
            this.btn_Search.TabIndex = 9;
            this.btn_Search.Text = "검색";
            this.btn_Search.UseVisualStyleBackColor = true;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // btn_TogglePause
            // 
            this.btn_TogglePause.Location = new System.Drawing.Point(121, 139);
            this.btn_TogglePause.Name = "btn_TogglePause";
            this.btn_TogglePause.Size = new System.Drawing.Size(113, 23);
            this.btn_TogglePause.TabIndex = 11;
            this.btn_TogglePause.Text = "Start";
            this.btn_TogglePause.UseVisualStyleBackColor = true;
            this.btn_TogglePause.Click += new System.EventHandler(this.btn_TogglePause_Click);
            // 
            // lBox_queue
            // 
            this.lBox_queue.FormattingEnabled = true;
            this.lBox_queue.ItemHeight = 12;
            this.lBox_queue.Location = new System.Drawing.Point(3, 17);
            this.lBox_queue.Name = "lBox_queue";
            this.lBox_queue.Size = new System.Drawing.Size(161, 268);
            this.lBox_queue.TabIndex = 12;
            // 
            // lBox_DownloadedWebtoons
            // 
            this.lBox_DownloadedWebtoons.FormattingEnabled = true;
            this.lBox_DownloadedWebtoons.ItemHeight = 12;
            this.lBox_DownloadedWebtoons.Location = new System.Drawing.Point(170, 17);
            this.lBox_DownloadedWebtoons.Name = "lBox_DownloadedWebtoons";
            this.lBox_DownloadedWebtoons.Size = new System.Drawing.Size(162, 268);
            this.lBox_DownloadedWebtoons.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "대기목록";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(170, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "완료목록";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lBox_queue, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label6, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lBox_DownloadedWebtoons, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btn_clearDownloadedList, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btn_clrQueue, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 168);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.899136F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95.10087F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(335, 317);
            this.tableLayoutPanel1.TabIndex = 14;
            // 
            // btn_clearDownloadedList
            // 
            this.btn_clearDownloadedList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_clearDownloadedList.Location = new System.Drawing.Point(170, 292);
            this.btn_clearDownloadedList.Name = "btn_clearDownloadedList";
            this.btn_clearDownloadedList.Size = new System.Drawing.Size(162, 22);
            this.btn_clearDownloadedList.TabIndex = 15;
            this.btn_clearDownloadedList.Text = "완료목록 지우기";
            this.btn_clearDownloadedList.UseVisualStyleBackColor = true;
            this.btn_clearDownloadedList.Click += new System.EventHandler(this.btn_clearDownloadedList_Click);
            // 
            // btn_clrQueue
            // 
            this.btn_clrQueue.Location = new System.Drawing.Point(3, 292);
            this.btn_clrQueue.Name = "btn_clrQueue";
            this.btn_clrQueue.Size = new System.Drawing.Size(161, 22);
            this.btn_clrQueue.TabIndex = 16;
            this.btn_clrQueue.Text = "대기목록 지우기";
            this.btn_clrQueue.UseVisualStyleBackColor = true;
            this.btn_clrQueue.Click += new System.EventHandler(this.btn_clrQueue_Click);
            // 
            // prgsBar_Webtoon
            // 
            this.prgsBar_Webtoon.Location = new System.Drawing.Point(12, 492);
            this.prgsBar_Webtoon.Maximum = 100000;
            this.prgsBar_Webtoon.Name = "prgsBar_Webtoon";
            this.prgsBar_Webtoon.Size = new System.Drawing.Size(333, 23);
            this.prgsBar_Webtoon.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(237, 117);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "쓰레드";
            // 
            // num_Thread
            // 
            this.num_Thread.Location = new System.Drawing.Point(285, 118);
            this.num_Thread.Name = "num_Thread";
            this.num_Thread.Size = new System.Drawing.Size(57, 21);
            this.num_Thread.TabIndex = 17;
            this.num_Thread.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.num_Thread.ValueChanged += new System.EventHandler(this.num_Thread_ValueChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(356, 527);
            this.Controls.Add(this.num_Thread);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.prgsBar_Webtoon);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btn_TogglePause);
            this.Controls.Add(this.btn_Search);
            this.Controls.Add(this.checkBox_zip);
            this.Controls.Add(this.checkBox_HTML);
            this.Controls.Add(this.btn_AddTask);
            this.Controls.Add(this.btn_AddFavorite);
            this.Controls.Add(this.btn_ModifyFavorite);
            this.Controls.Add(this.btn_FileManagement);
            this.Controls.Add(this.checkBox_SelectAll);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.num_endNo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.num_StartNo);
            this.Controls.Add(this.tBox_titleId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tBox_WebtoonName);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "Naver Webtoon Downloader";
            ((System.ComponentModel.ISupportInitialize)(this.num_StartNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_endNo)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_Thread)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tBox_WebtoonName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tBox_titleId;
        private System.Windows.Forms.NumericUpDown num_StartNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown num_endNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBox_SelectAll;
        private System.Windows.Forms.Button btn_FileManagement;
        private System.Windows.Forms.Button btn_ModifyFavorite;
        private System.Windows.Forms.Button btn_AddFavorite;
        private System.Windows.Forms.Button btn_AddTask;
        private System.Windows.Forms.CheckBox checkBox_HTML;
        private System.Windows.Forms.CheckBox checkBox_zip;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.Button btn_TogglePause;
        public System.Windows.Forms.ListBox lBox_queue;
        private System.Windows.Forms.ListBox lBox_DownloadedWebtoons;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ProgressBar prgsBar_Webtoon;
        private System.Windows.Forms.Button btn_clearDownloadedList;
        private System.Windows.Forms.Button btn_clrQueue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown num_Thread;
    }
}

