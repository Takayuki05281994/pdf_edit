
namespace PDFEdit
{
    partial class FormMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.btn_cvDelUndo = new System.Windows.Forms.Button();
			this.btn_cvDelete = new System.Windows.Forms.Button();
			this.tBox_cvDeletePages = new System.Windows.Forms.TextBox();
			this.btn_cvRotateLeft = new System.Windows.Forms.Button();
			this.btn_cvRotateRight = new System.Windows.Forms.Button();
			this.tBox_cvPages = new System.Windows.Forms.TextBox();
			this.panel = new System.Windows.Forms.Panel();
			this.webView2_cvPDFView = new Microsoft.Web.WebView2.WinForms.WebView2();
			this.btn_cvPDFFileRef = new System.Windows.Forms.Button();
			this.tBox_cvPDFFile = new System.Windows.Forms.TextBox();
			this.btn_cvPDFRead = new System.Windows.Forms.Button();
			this.btn_cvUp = new System.Windows.Forms.Button();
			this.btn_cvDown = new System.Windows.Forms.Button();
			this.lView_cvPDFPageList = new System.Windows.Forms.ListView();
			this.imageList_cvPDFPageList = new System.Windows.Forms.ImageList(this.components);
			this.btn_cvSave = new System.Windows.Forms.Button();
			this.lab_cvMessage = new System.Windows.Forms.Label();
			this.btn_cvClose = new System.Windows.Forms.Button();
			this.btn_cvNamedSave = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.panel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.webView2_cvPDFView)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer
			// 
			this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer.Location = new System.Drawing.Point(12, 9);
			this.splitContainer.Name = "splitContainer";
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.btn_cvDelUndo);
			this.splitContainer.Panel1.Controls.Add(this.btn_cvDelete);
			this.splitContainer.Panel1.Controls.Add(this.tBox_cvDeletePages);
			this.splitContainer.Panel1.Controls.Add(this.btn_cvRotateLeft);
			this.splitContainer.Panel1.Controls.Add(this.btn_cvRotateRight);
			this.splitContainer.Panel1.Controls.Add(this.tBox_cvPages);
			this.splitContainer.Panel1.Controls.Add(this.panel);
			this.splitContainer.Panel1.Controls.Add(this.btn_cvPDFFileRef);
			this.splitContainer.Panel1.Controls.Add(this.tBox_cvPDFFile);
			this.splitContainer.Panel1.Controls.Add(this.btn_cvPDFRead);
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.btn_cvUp);
			this.splitContainer.Panel2.Controls.Add(this.btn_cvDown);
			this.splitContainer.Panel2.Controls.Add(this.lView_cvPDFPageList);
			this.splitContainer.Size = new System.Drawing.Size(887, 690);
			this.splitContainer.SplitterDistance = 676;
			this.splitContainer.TabIndex = 153;
			// 
			// btn_cvDelUndo
			// 
			this.btn_cvDelUndo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btn_cvDelUndo.Enabled = false;
			this.btn_cvDelUndo.Location = new System.Drawing.Point(4, 661);
			this.btn_cvDelUndo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.btn_cvDelUndo.Name = "btn_cvDelUndo";
			this.btn_cvDelUndo.Size = new System.Drawing.Size(69, 25);
			this.btn_cvDelUndo.TabIndex = 157;
			this.btn_cvDelUndo.Text = "削除を戻す";
			this.btn_cvDelUndo.UseVisualStyleBackColor = false;
			this.btn_cvDelUndo.Click += new System.EventHandler(this.btn_cvDelUndo_Click);
			// 
			// btn_cvDelete
			// 
			this.btn_cvDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_cvDelete.Enabled = false;
			this.btn_cvDelete.Location = new System.Drawing.Point(622, 661);
			this.btn_cvDelete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.btn_cvDelete.Name = "btn_cvDelete";
			this.btn_cvDelete.Size = new System.Drawing.Size(49, 25);
			this.btn_cvDelete.TabIndex = 156;
			this.btn_cvDelete.Text = "削除";
			this.btn_cvDelete.UseVisualStyleBackColor = false;
			this.btn_cvDelete.Click += new System.EventHandler(this.btn_cvDelete_Click);
			// 
			// tBox_cvDeletePages
			// 
			this.tBox_cvDeletePages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tBox_cvDeletePages.BackColor = System.Drawing.Color.Ivory;
			this.tBox_cvDeletePages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tBox_cvDeletePages.Location = new System.Drawing.Point(81, 665);
			this.tBox_cvDeletePages.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tBox_cvDeletePages.Name = "tBox_cvDeletePages";
			this.tBox_cvDeletePages.ReadOnly = true;
			this.tBox_cvDeletePages.Size = new System.Drawing.Size(533, 19);
			this.tBox_cvDeletePages.TabIndex = 155;
			// 
			// btn_cvRotateLeft
			// 
			this.btn_cvRotateLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_cvRotateLeft.Enabled = false;
			this.btn_cvRotateLeft.Location = new System.Drawing.Point(594, 34);
			this.btn_cvRotateLeft.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.btn_cvRotateLeft.Name = "btn_cvRotateLeft";
			this.btn_cvRotateLeft.Size = new System.Drawing.Size(37, 25);
			this.btn_cvRotateLeft.TabIndex = 149;
			this.btn_cvRotateLeft.Text = "左";
			this.btn_cvRotateLeft.UseVisualStyleBackColor = false;
			this.btn_cvRotateLeft.Click += new System.EventHandler(this.btn_cvRotateLeft_Click);
			// 
			// btn_cvRotateRight
			// 
			this.btn_cvRotateRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_cvRotateRight.Enabled = false;
			this.btn_cvRotateRight.Location = new System.Drawing.Point(634, 34);
			this.btn_cvRotateRight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.btn_cvRotateRight.Name = "btn_cvRotateRight";
			this.btn_cvRotateRight.Size = new System.Drawing.Size(37, 25);
			this.btn_cvRotateRight.TabIndex = 148;
			this.btn_cvRotateRight.Text = "右";
			this.btn_cvRotateRight.UseVisualStyleBackColor = false;
			this.btn_cvRotateRight.Click += new System.EventHandler(this.btn_cvRotateRight_Click);
			// 
			// tBox_cvPages
			// 
			this.tBox_cvPages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tBox_cvPages.BackColor = System.Drawing.Color.Ivory;
			this.tBox_cvPages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tBox_cvPages.Location = new System.Drawing.Point(3, 38);
			this.tBox_cvPages.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tBox_cvPages.Name = "tBox_cvPages";
			this.tBox_cvPages.ReadOnly = true;
			this.tBox_cvPages.Size = new System.Drawing.Size(583, 19);
			this.tBox_cvPages.TabIndex = 147;
			// 
			// panel
			// 
			this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel.Controls.Add(this.webView2_cvPDFView);
			this.panel.Location = new System.Drawing.Point(2, 68);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(670, 587);
			this.panel.TabIndex = 2;
			// 
			// webView2_cvPDFView
			// 
			this.webView2_cvPDFView.AllowExternalDrop = true;
			this.webView2_cvPDFView.CreationProperties = null;
			this.webView2_cvPDFView.DefaultBackgroundColor = System.Drawing.Color.White;
			this.webView2_cvPDFView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webView2_cvPDFView.Location = new System.Drawing.Point(0, 0);
			this.webView2_cvPDFView.Name = "webView2_cvPDFView";
			this.webView2_cvPDFView.Size = new System.Drawing.Size(668, 585);
			this.webView2_cvPDFView.TabIndex = 2;
			this.webView2_cvPDFView.ZoomFactor = 1D;
			// 
			// btn_cvPDFFileRef
			// 
			this.btn_cvPDFFileRef.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_cvPDFFileRef.Location = new System.Drawing.Point(485, 7);
			this.btn_cvPDFFileRef.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.btn_cvPDFFileRef.Name = "btn_cvPDFFileRef";
			this.btn_cvPDFFileRef.Size = new System.Drawing.Size(76, 25);
			this.btn_cvPDFFileRef.TabIndex = 152;
			this.btn_cvPDFFileRef.Text = "参照";
			this.btn_cvPDFFileRef.UseVisualStyleBackColor = false;
			this.btn_cvPDFFileRef.Click += new System.EventHandler(this.btn_cvPDFFileRef_Click);
			// 
			// tBox_cvPDFFile
			// 
			this.tBox_cvPDFFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tBox_cvPDFFile.BackColor = System.Drawing.SystemColors.Window;
			this.tBox_cvPDFFile.Location = new System.Drawing.Point(4, 11);
			this.tBox_cvPDFFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tBox_cvPDFFile.Name = "tBox_cvPDFFile";
			this.tBox_cvPDFFile.Size = new System.Drawing.Size(473, 19);
			this.tBox_cvPDFFile.TabIndex = 151;
			// 
			// btn_cvPDFRead
			// 
			this.btn_cvPDFRead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_cvPDFRead.Location = new System.Drawing.Point(568, 7);
			this.btn_cvPDFRead.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.btn_cvPDFRead.Name = "btn_cvPDFRead";
			this.btn_cvPDFRead.Size = new System.Drawing.Size(103, 25);
			this.btn_cvPDFRead.TabIndex = 153;
			this.btn_cvPDFRead.Text = "PDF読み込み";
			this.btn_cvPDFRead.UseVisualStyleBackColor = false;
			this.btn_cvPDFRead.Click += new System.EventHandler(this.btn_cvPDFRead_Click);
			// 
			// btn_cvUp
			// 
			this.btn_cvUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_cvUp.Enabled = false;
			this.btn_cvUp.Location = new System.Drawing.Point(126, 3);
			this.btn_cvUp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.btn_cvUp.Name = "btn_cvUp";
			this.btn_cvUp.Size = new System.Drawing.Size(37, 25);
			this.btn_cvUp.TabIndex = 151;
			this.btn_cvUp.Text = "▲";
			this.btn_cvUp.UseVisualStyleBackColor = false;
			this.btn_cvUp.Click += new System.EventHandler(this.btn_cvUp_Click);
			// 
			// btn_cvDown
			// 
			this.btn_cvDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_cvDown.Enabled = false;
			this.btn_cvDown.Location = new System.Drawing.Point(166, 3);
			this.btn_cvDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.btn_cvDown.Name = "btn_cvDown";
			this.btn_cvDown.Size = new System.Drawing.Size(37, 25);
			this.btn_cvDown.TabIndex = 150;
			this.btn_cvDown.Text = "▼";
			this.btn_cvDown.UseVisualStyleBackColor = false;
			this.btn_cvDown.Click += new System.EventHandler(this.btn_cvDown_Click);
			// 
			// lView_cvPDFPageList
			// 
			this.lView_cvPDFPageList.AllowDrop = true;
			this.lView_cvPDFPageList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lView_cvPDFPageList.Enabled = false;
			this.lView_cvPDFPageList.HideSelection = false;
			this.lView_cvPDFPageList.LargeImageList = this.imageList_cvPDFPageList;
			this.lView_cvPDFPageList.Location = new System.Drawing.Point(4, 34);
			this.lView_cvPDFPageList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.lView_cvPDFPageList.MultiSelect = false;
			this.lView_cvPDFPageList.Name = "lView_cvPDFPageList";
			this.lView_cvPDFPageList.Size = new System.Drawing.Size(199, 652);
			this.lView_cvPDFPageList.TabIndex = 144;
			this.lView_cvPDFPageList.UseCompatibleStateImageBehavior = false;
			this.lView_cvPDFPageList.SelectedIndexChanged += new System.EventHandler(this.lView_cvPDFPageList_SelectedIndexChanged);
			// 
			// imageList_cvPDFPageList
			// 
			this.imageList_cvPDFPageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList_cvPDFPageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList_cvPDFPageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// btn_cvSave
			// 
			this.btn_cvSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_cvSave.Enabled = false;
			this.btn_cvSave.Location = new System.Drawing.Point(696, 706);
			this.btn_cvSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.btn_cvSave.Name = "btn_cvSave";
			this.btn_cvSave.Size = new System.Drawing.Size(105, 25);
			this.btn_cvSave.TabIndex = 155;
			this.btn_cvSave.Text = "上書き保存/終了";
			this.btn_cvSave.UseVisualStyleBackColor = false;
			this.btn_cvSave.Click += new System.EventHandler(this.btn_cvSave_Click);
			// 
			// lab_cvMessage
			// 
			this.lab_cvMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lab_cvMessage.BackColor = System.Drawing.Color.White;
			this.lab_cvMessage.ForeColor = System.Drawing.Color.Red;
			this.lab_cvMessage.Location = new System.Drawing.Point(13, 710);
			this.lab_cvMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lab_cvMessage.Name = "lab_cvMessage";
			this.lab_cvMessage.Size = new System.Drawing.Size(535, 16);
			this.lab_cvMessage.TabIndex = 154;
			this.lab_cvMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btn_cvClose
			// 
			this.btn_cvClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_cvClose.Location = new System.Drawing.Point(809, 706);
			this.btn_cvClose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.btn_cvClose.Name = "btn_cvClose";
			this.btn_cvClose.Size = new System.Drawing.Size(90, 25);
			this.btn_cvClose.TabIndex = 156;
			this.btn_cvClose.Text = "破棄して終了";
			this.btn_cvClose.UseVisualStyleBackColor = false;
			this.btn_cvClose.Click += new System.EventHandler(this.btn_paClose_Click);
			// 
			// btn_cvNamedSave
			// 
			this.btn_cvNamedSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_cvNamedSave.Enabled = false;
			this.btn_cvNamedSave.Location = new System.Drawing.Point(556, 706);
			this.btn_cvNamedSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.btn_cvNamedSave.Name = "btn_cvNamedSave";
			this.btn_cvNamedSave.Size = new System.Drawing.Size(132, 25);
			this.btn_cvNamedSave.TabIndex = 157;
			this.btn_cvNamedSave.Text = "名前を付けて保存/終了";
			this.btn_cvNamedSave.UseVisualStyleBackColor = false;
			this.btn_cvNamedSave.Click += new System.EventHandler(this.btn_cvNamedSave_Click);
			// 
			// FormMain
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(911, 738);
			this.Controls.Add(this.btn_cvNamedSave);
			this.Controls.Add(this.splitContainer);
			this.Controls.Add(this.btn_cvSave);
			this.Controls.Add(this.lab_cvMessage);
			this.Controls.Add(this.btn_cvClose);
			this.Name = "FormMain";
			this.Text = "ＰDF編集 V1.1";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormMain_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormMain_DragEnter);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel1.PerformLayout();
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.panel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.webView2_cvPDFView)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Button btn_cvRotateLeft;
        private System.Windows.Forms.Button btn_cvRotateRight;
        private System.Windows.Forms.TextBox tBox_cvPages;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Button btn_cvPDFFileRef;
        private System.Windows.Forms.TextBox tBox_cvPDFFile;
        private System.Windows.Forms.Button btn_cvPDFRead;
        private System.Windows.Forms.Button btn_cvUp;
        private System.Windows.Forms.Button btn_cvDown;
        private System.Windows.Forms.ListView lView_cvPDFPageList;
        private System.Windows.Forms.ImageList imageList_cvPDFPageList;
        private System.Windows.Forms.Button btn_cvSave;
        private System.Windows.Forms.Label lab_cvMessage;
        private System.Windows.Forms.Button btn_cvClose;
        private System.Windows.Forms.Button btn_cvDelUndo;
        private System.Windows.Forms.Button btn_cvDelete;
        private System.Windows.Forms.TextBox tBox_cvDeletePages;
        private System.Windows.Forms.Button btn_cvNamedSave;
		private Microsoft.Web.WebView2.WinForms.WebView2 webView2_cvPDFView;
	}
}

