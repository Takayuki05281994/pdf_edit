using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;


namespace PDFEdit
{
	public partial class FormMain : Form
	{
		[System.Runtime.InteropServices.DllImport("ole32.dll")]
		private static extern void CoFreeUnusedLibraries();

		private string exeFolder = null;
		private string workingFolder = null;
		private List<Image> allThumbNailImageList = null;
		private List<PDFPageEntry> pdfPageEntryList = null;
		private PDFPageEntry currPage = null;
		private List<int> undoPageList = null;
		private List<int> pageList = null;
		private FileInfo fileInfo = null;
		private bool editFlag = false;
		private bool saveFlag = false;
		private bool inProcFlag = false;
		private bool pauseWebViewFlag = false;

		private const int SIZEX = 128;
		private const int SIZEY = 128;


		public FormMain()
		{
			Assembly asm = Assembly.GetEntryAssembly();
			string exePath = asm.Location;
			exeFolder = Path.GetDirectoryName(exePath);
			workingFolder = Path.Combine(exeFolder, "WorkingFolder");
			if (Directory.Exists(workingFolder))
			{
				DeleteAll(workingFolder, true);
			}
			else
			{
				Directory.CreateDirectory(workingFolder);
			}

			InitializeComponent();
			imageList_cvPDFPageList.ImageSize = new Size(SIZEX, SIZEY);
			allThumbNailImageList = new List<Image>();
			pageList = new List<int>();
			undoPageList = new List<int>();
			InitWebView();
		}


		// WebView2コントロール初期化
		async private void InitWebView()
		{
			await webView2_cvPDFView.EnsureCoreWebView2Async();
		}


		// PDFファイルロード&ページ分割
		public bool LoadFile(FileInfo fi)
		{
			lab_cvMessage.Text = "";
			fileInfo = fi;
			if (!File.Exists(fileInfo.FullName))
			{
				lab_cvMessage.Text = "ファイルが見つかりません。";
				return false;
			}
			PdfDocument inputDocument;
			try
			{
				inputDocument = PdfReader.Open(fileInfo.FullName, PdfDocumentOpenMode.Import);
			}
			catch /*(Exception ex)*/
			{
				lab_cvMessage.Text = "PDFファイルの読み込みに失敗しました。";
				return false;
			}

			pdfPageEntryList = new List<PDFPageEntry>();
			string name = Path.GetFileNameWithoutExtension(fileInfo.FullName);
			for (int idx = 0; idx < inputDocument.PageCount; idx++)
			{ 
				PDFPageEntry entry = new PDFPageEntry(idx + 1, inputDocument.PageCount, name,
					String.Format("{0}_P{1}.pdf", name, idx + 1), Path.Combine(workingFolder, String.Format("{0}_P{1}.pdf", name, idx + 1)));
				PdfDocument outputDocument = new PdfDocument();
				outputDocument.Version = inputDocument.Version;
				outputDocument.Info.Title = String.Format("Page {0} of {1}", idx + 1, inputDocument.Info.Title);
				outputDocument.Info.Creator = inputDocument.Info.Creator;

				outputDocument.AddPage(inputDocument.Pages[idx]); 
				outputDocument.Save(entry.FilePath);
				pdfPageEntryList.Add(entry);
				pageList.Add(idx + 1);
			}
			btn_cvPDFRead.Enabled = false;
			btn_cvPDFFileRef.Enabled = false;

			MakeSrcPDFList();
			if (pdfPageEntryList.Count > 0)
			{
				lView_cvPDFPageList.Items[0].Selected = true;
			}
			return true;
		}


		private void FormMain_Load(object sender, EventArgs e)
		{
			ActiveControl = btn_cvPDFFileRef;
		}


		// ListViewに分割したPDFファイルを展開
		private void MakeSrcPDFList()
		{
			lView_cvPDFPageList.SuspendLayout();
			for (int i1 = 0; i1 < pdfPageEntryList.Count; i1++)
			{
				PDFPageEntry entry = pdfPageEntryList[i1];
				if (entry.PageState != PDFPageEntry.PS_NONE)
				{
					continue;
				}
				string file = entry.FilePath;
				ThumbnailCreator thumbCreator = new ThumbnailCreator(SIZEX, SIZEY);
				Image thumbImage = thumbCreator.Thumbnail(null, file);
				allThumbNailImageList.Add(thumbImage);
				imageList_cvPDFPageList.Images.Add(thumbImage);

				ListViewItem item = null;
				item = new ListViewItem();
				item.Text = entry.Page.ToString();
				item.ImageIndex = i1;
				item.Tag = entry.Page;
				lView_cvPDFPageList.Items.Add(item);
			}
			lView_cvPDFPageList.ResumeLayout();
		}


		// ListViewの再描画
		private void RefreshPDFList()
		{
			lView_cvPDFPageList.SuspendLayout();
			lView_cvPDFPageList.Items.Clear();
			for (int i1 = 0; i1 < pageList.Count; i1++)
			{
				int page = pageList[i1];
				ListViewItem item = null;
				item = new ListViewItem();
				item.Text = page.ToString();
				item.ImageIndex = page - 1;
				item.Tag = page;
				lView_cvPDFPageList.Items.Add(item);
			}
			lView_cvPDFPageList.ResumeLayout();
		}


		// PDF削除処理
		private void btn_cvDelete_Click(object sender, EventArgs e)
		{
			lab_cvMessage.Text = "";
			if (currPage == null)
			{
				lab_cvMessage.Text = "ページが指定されていません。";
				return;
			}

			if (inProcFlag)
			{
				return;
			}
			inProcFlag = true;
			currPage.PageState = PDFPageEntry.PS_DELETE;
			int page = currPage.Page;
			AddDeletePage(page);

			pauseWebViewFlag = true;
			for (int i1 = 0; i1 < pageList.Count; i1++)
			{
				if (pageList[i1] == page)
				{
					pageList.RemoveAt(i1);
					break;
				}
			}

			int sel = lView_cvPDFPageList.SelectedIndices[0];
			lView_cvPDFPageList.SelectedItems[0].Remove();
			currPage = null;
			pauseWebViewFlag = false;

			SetLViewSel(sel);
			tBox_cvPages.Text = PageListStr;
			btn_cvSave.Enabled = true;
			btn_cvNamedSave.Enabled = true;
			editFlag = true;
			inProcFlag = false;
		}


		// 削除ページを削除ページ一覧に追加
		private void AddDeletePage(int page)
		{
			undoPageList.Add(page);
			RefreshDeleteList();
			btn_cvDelUndo.Enabled = true;
		}


		// 削除ページ一覧更新
		private void RefreshDeleteList()
		{
			string str = "";
			for (int i1 = 0; i1 < undoPageList.Count; i1++)
			{
				if (str != "")
				{
					str += "、";
				}
				str += undoPageList[i1].ToString();
			}
			tBox_cvDeletePages.Text = str;
		}


		// ListView選択時の処理
		private void SetLViewSel(int sel)
		{
			if (sel == -1)
			{
				return;
			}
			if (lView_cvPDFPageList.Items.Count == 0)
			{
				for (int i1 = 0; i1 < 10; i1++)
				{
					webView2_cvPDFView.CoreWebView2.Navigate("about:blank");
					Application.DoEvents();
					CoFreeUnusedLibraries();
					Thread.Sleep(10);
				}
				return;
			}
			if (sel >= lView_cvPDFPageList.Items.Count)
			{
				sel = lView_cvPDFPageList.Items.Count - 1;
			}
			lView_cvPDFPageList.Items[sel].Selected = true;
			lView_cvPDFPageList.EnsureVisible(/*0*/sel);
		}


		// PDFページ左回転の処理(PDFRorateメソッド呼び出し)
		private void btn_cvRotateLeft_Click(object sender, EventArgs e)
		{
			if (currPage == null)
			{
				return;
			}
			if (inProcFlag)
			{
				return;
			}
			inProcFlag = true;
			int page = currPage.Page;
			PDFRorate(page, lView_cvPDFPageList.SelectedIndices[0], false);
			inProcFlag = false;
		}


		// PDFページ右回転の処理(PDFRorateメソッド呼び出し)
		private void btn_cvRotateRight_Click(object sender, EventArgs e)
		{
			if (currPage == null)
			{
				return;
			}
			if (inProcFlag)
			{
				return;
			}
			inProcFlag = true;
			int page = currPage.Page;
			PDFRorate(page, lView_cvPDFPageList.SelectedIndices[0], true);
			inProcFlag = false;
		}


		// PDFページ回転の処理
		private void PDFRorate(int page, int pos, bool rightFlag)
		{
			PDFPageEntry entry = pdfPageEntryList[page - 1];

			PdfDocument document = PdfReader.Open(entry.FilePath, PdfDocumentOpenMode.Import);
			var pdfPage = document.Pages[0];
			pdfPage.Rotate = (pdfPage.Rotate + (rightFlag?90:-90)) % 360;
			document.Close();

			for (int i1 = 0; i1 < 10; i1++)
			{
				webView2_cvPDFView.CoreWebView2.Navigate("about:blank");
				Application.DoEvents();
				CoFreeUnusedLibraries();
				Thread.Sleep(10);
				try
				{
					document.Save(entry.FilePath);
					break;
				}
				catch
				{
				}
			}
			webView2_cvPDFView.CoreWebView2.Navigate(entry.FilePath);

			string file = entry.FilePath;
			ThumbnailCreator thumbCreator = new ThumbnailCreator(SIZEX, SIZEY);
			Image thumbImage = thumbCreator.Thumbnail(null, file);

			allThumbNailImageList[page - 1].Dispose();
			allThumbNailImageList[page - 1] = thumbImage;
			imageList_cvPDFPageList.Images[page - 1] = thumbImage;

			// ImageListを変更するだけでは、ListViewが更新されない
			int value = lView_cvPDFPageList.Items[pos].ImageIndex;
			lView_cvPDFPageList.Items[pos].ImageIndex = -1;
			lView_cvPDFPageList.Items[pos].ImageIndex = value;

			// こうするとListViewをクリックすると更新される。クリックが必要な不具合を以下で回避
			string str = lView_cvPDFPageList.Items[pos].SubItems[0].Text;
			lView_cvPDFPageList.Items[pos].SubItems[0].Text = str + "R";
			lView_cvPDFPageList.Items[pos].SubItems[0].Text = str;
			btn_cvSave.Enabled = true;
			btn_cvNamedSave.Enabled = true;
			editFlag = true;
		}


		// PDFページ上移動
		private void btn_cvUp_Click(object sender, EventArgs e)
		{
			if (currPage == null)
			{
				return;
			}
			if (lView_cvPDFPageList.SelectedItems.Count != 1)
			{
				return;
			}
			int sel = lView_cvPDFPageList.SelectedIndices[0];
			if (sel == -1)
			{
				return;
			}
			if (sel <= 0)
			{
				return;
			}
			if (inProcFlag)
			{
				return;
			}
			inProcFlag = true;
			pauseWebViewFlag = true;
			SwapOpeLViewItem(sel-1, sel);
			pauseWebViewFlag = false;
			lView_cvPDFPageList.Items[sel-1].Selected = true;
			lView_cvPDFPageList.EnsureVisible(sel-1);
			inProcFlag = false;
		}


		// PDFページ下移動
		private void btn_cvDown_Click(object sender, EventArgs e)
		{
			if (currPage == null)
			{
				return;
			}
			if (lView_cvPDFPageList.SelectedItems.Count != 1)
			{
				return;
			}
			int sel = lView_cvPDFPageList.SelectedIndices[0];
			if (sel == -1)
			{
				return;
			}
			if (sel >= lView_cvPDFPageList.Items.Count - 1)
			{
				return;
			}
			if (inProcFlag)
			{
				return;
			}
			inProcFlag = true;
			pauseWebViewFlag = true;
			SwapOpeLViewItem(sel, sel+1);
			pauseWebViewFlag = false;
			lView_cvPDFPageList.Items[sel+1].Selected = true;
			lView_cvPDFPageList.EnsureVisible(sel+1);
			inProcFlag = false;
		}



		// PDFページ位置移動に使用するListViewの項目swap
		private void SwapOpeLViewItem(int pos1, int pos2)
		{
			if (pos1 < 0 || pos1 >= lView_cvPDFPageList.Items.Count ||
				pos2 < 0 || pos2 >= lView_cvPDFPageList.Items.Count)
			{
				return;
			}

			int value = pageList[pos1];
			pageList[pos1] = pageList[pos2];
			pageList[pos2] = value;
			tBox_cvPages.Text = PageListStr;
			RefreshPDFList();
			btn_cvSave.Enabled = true;
			btn_cvNamedSave.Enabled = true;
			editFlag = true;
		}


		// ListView選択項目取得
		private int GetLViewTag()
		{
			if (lView_cvPDFPageList.SelectedItems.Count != 1)
			{
				return -1;
			}
			int tag = (int)lView_cvPDFPageList.SelectedItems[0].Tag;
			return tag;
		}


		// PDFファイル指定時の参照ボタン処理
		private void btn_cvPDFFileRef_Click(object sender, EventArgs e)
		{
			lab_cvMessage.Text = "";
			OpenFileDialog ofd = new OpenFileDialog();
			if (ofd.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			tBox_cvPDFFile.Text = ofd.FileName;
			btn_cvPDFRead.Focus();
		}


		// PDFファイル読み込み
		private void btn_cvPDFRead_Click(object sender, EventArgs e)
		{
			lab_cvMessage.Text = "";
			if (tBox_cvPDFFile.Text.Trim() == "")
			{
				lab_cvMessage.Text = "PDFファイルを指定してください。";
				return;
			}

			FileInfo fi = new FileInfo(tBox_cvPDFFile.Text.Trim());
			LoadFile(fi);
			tBox_cvPages.Text = PageListStr;
			RefreshPDFList();
			if (pageList.Count > 0)
			{
				lView_cvPDFPageList.Items[0].Selected = true;
			}
		}


		// ListView選択処理
		private void lView_cvPDFPageList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (pauseWebViewFlag)
			{
				return;
			}

			lView_cvPDFPageList.Enabled = false;
			lab_cvMessage.Text = "";
			currPage = null;
			int page = GetLViewTag();
			if (page == -1)
			{
				EnableButtons(false);
				for (int i1 = 0; i1 < 10; i1++)
				{
					webView2_cvPDFView.CoreWebView2.Navigate("about:blank");
					Application.DoEvents();
					CoFreeUnusedLibraries();
					Thread.Sleep(10);
				}
				return;
			}
			PDFPageEntry entry = pdfPageEntryList[page - 1];
			currPage = entry;
			webView2_cvPDFView.CoreWebView2.Navigate(entry.FilePath);
			lView_cvPDFPageList.Enabled = true;
			EnableButtons(true);
			lView_cvPDFPageList.Enabled = true;
		}


		// ボタン有効/無効変更
		private void EnableButtons(bool enable)
		{
			btn_cvDelete.Enabled = enable;
			btn_cvRotateLeft.Enabled = enable;
			btn_cvRotateRight.Enabled = enable;
			btn_cvUp.Enabled = enable;
			btn_cvDown.Enabled = enable;
		}


		// PDF上書き保存処理
		private void btn_cvSave_Click(object sender, EventArgs e)
		{
			lab_cvMessage.Text = "";
			if (DialogResult.Cancel == MessageBox.Show("ファイルを上書きしてよろしいですか?", "PDF編集",
				MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
			{
				return;
			}
				for (int i1 = 0; i1 < 10; i1++)
			{
				webView2_cvPDFView.CoreWebView2.Navigate("about:blank");
				Application.DoEvents();
				CoFreeUnusedLibraries();
				Thread.Sleep(10);
			}
			try
			{
				if (DoSave(fileInfo.FullName) == false)
				{
					lab_cvMessage.Text = "保存に失敗しました。";
					return;
				}
				saveFlag = true;
			}
			catch
			{
				lab_cvMessage.Text = "保存に失敗しました。";
				return;
			}

			Close();
		}


		// PDF名前をつけて保存処理
		private void btn_cvNamedSave_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.FileName = "新しいファイル.PDF";
			sfd.InitialDirectory = fileInfo.DirectoryName;
			sfd.Filter = "PDFファイル(*.PDF)|*.PDF|すべてのファイル(*.*)|*.*";
			sfd.FilterIndex = 1;
			sfd.Title = "保存先のファイル名を指定してください";
			sfd.RestoreDirectory = true;

			if (sfd.ShowDialog() == DialogResult.OK)
			{
				try
				{
					if (DoSave(sfd.FileName) == false)
					{
						lab_cvMessage.Text = "保存に失敗しました。";
						return;
					}
					saveFlag = true;
				}
				catch
				{
					lab_cvMessage.Text = "保存に失敗しました。";
					return;
				}
			}
			Close();
		}


		// ウィンドウを閉じる
		private void btn_paClose_Click(object sender, EventArgs e)
		{
			Close();
		}


		// 保存処理本体
		private bool DoSave(string file)
		{
			if (File.Exists(file))
			{
				try
				{
					string bakFile = Path.Combine(fileInfo.DirectoryName, Path.GetFileNameWithoutExtension(file) + "_BAK.PDF");
					File.Delete(bakFile);
					File.Move(file, bakFile);
				}
				catch
				{
					return false;
				}
			}
			PdfDocument outputDocument = new PdfDocument();
			for (int i1 = 0; i1 < pageList.Count; i1++)
			{ 
				PDFPageEntry entry = pdfPageEntryList[pageList[i1] - 1];
				PdfDocument inputDocument = PdfReader.Open(entry.FilePath, PdfDocumentOpenMode.Import);
				PdfPage page = inputDocument.Pages[0];
				outputDocument.AddPage(page);
			}
			outputDocument.Save(file); 
			return true;
		}


		public List<int> UndoPageList
		{
			get
			{
				return undoPageList;
			}
		}


		public string PageListStr
		{
			get
			{
				string str = "";
				for (int i1 = 0; i1 < pageList.Count; i1++)
				{
					if (str != "")
					{
						str += "、";
					}
					str += pageList[i1].ToString();
				}
				return str;
			}
		}


		public bool SaveFlag
		{
			get
			{
				return saveFlag;
			}
		}


		// フォルダ内ファイル削除処理
		private static bool DeleteAll(string targetDirectoryPath, bool subFolderOnly)
		{
			if (!Directory.Exists (targetDirectoryPath))
			{
				return false;
			}

			try
			{
				string[] filePaths = Directory.GetFiles(targetDirectoryPath);
				foreach (string filePath in filePaths)
				{
					File.SetAttributes(filePath, FileAttributes.Normal);
					File.Delete(filePath);
				}

				string[] directoryPaths = Directory.GetDirectories(targetDirectoryPath);
				foreach (string directoryPath in directoryPaths)
				{
					if (DeleteAll(directoryPath, false) == false)
					{
						return false;
					}
				}

				if (subFolderOnly)
				{
					return true;
				}
				Directory.Delete(targetDirectoryPath, false);
			}
			catch
			{
				return false;
			}
			return true;
		}


		// 編集されているかのプロパティ
		private bool IsEdit
		{
			get
			{
				if (pdfPageEntryList == null)
				{
					return false;
				}
				for (int i1 = 0; i1 < pdfPageEntryList.Count; i1++)
				{
					PDFPageEntry entry = pdfPageEntryList[i1];
					if (entry.PageState != PDFPageEntry.PS_NONE)
					{
						return true;
					}
				}
				if (editFlag)
				{
					return true;
				}
				return false;
			}
		}


		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (saveFlag)
			{
				return;
			}
			if (IsEdit)
			{
				if (DialogResult.Cancel == MessageBox.Show("編集を破棄して終了します。終了してよろしいですか?", "PDF編集",
					MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
				{
					e.Cancel = true;
				}
			}
		}


		// Undo処理
		private void btn_cvDelUndo_Click(object sender, EventArgs e)
		{
			if (undoPageList.Count == 0)
			{
				return;
			}

			if (inProcFlag)
			{
				return;
			}
			inProcFlag = true;
			int idx = undoPageList.Count - 1;
			int page = undoPageList[idx];
			PDFPageEntry entry = pdfPageEntryList[page - 1];
			if (entry.PageState == PDFPageEntry.PS_DELETE)
			{
				RemoveFromDeleteList(page);
				RefreshDeleteList();
			}
			entry.PageState = PDFPageEntry.PS_NONE;

			for (int i1 = 0; i1 < pageList.Count; i1++)
			{
				if (pageList[i1] > entry.Page)
				{
					pageList.Insert(i1, entry.Page);
					break;
				}
			}

			imageList_cvPDFPageList.Images.Clear();
			for (int i1 = 0; i1 < allThumbNailImageList.Count; i1++)
			{
				Image thumbImage = allThumbNailImageList[i1];
				imageList_cvPDFPageList.Images.Add(thumbImage);
			}
			tBox_cvPages.Text = PageListStr;
			RefreshPDFList();
			SetLViewPage(page + 1);
			if (undoPageList.Count <= 0)
			{
				btn_cvDelUndo.Enabled = false;
			}
			inProcFlag = false;
		}


		// ListViewを指定ページに移動
		private void SetLViewPage(int page)
		{
			if (lView_cvPDFPageList.Items.Count == 0)
			{
				for (int i1 = 0; i1 < 10; i1++)
				{
					webView2_cvPDFView.CoreWebView2.Navigate("about:blank");
					Application.DoEvents();
					CoFreeUnusedLibraries();
					Thread.Sleep(10);
				}
				return;
			}
			for (int i1 = lView_cvPDFPageList.Items.Count - 1; i1 >= 0; i1--)
			{
				int tag = (int)lView_cvPDFPageList.Items[i1].Tag;
				PDFPageEntry entry = GetPageEntry(tag);
				if (entry == null)
				{
					continue;
				}
				if (entry.Page < page)
				{
					lView_cvPDFPageList.Items[i1].Selected = true;
					lView_cvPDFPageList.EnsureVisible(/*0*/i1);
					return;
				}
			}
			if (lView_cvPDFPageList.Items.Count > 0)
			{
				int lastPage = lView_cvPDFPageList.Items.Count - 1;
				lView_cvPDFPageList.Items[lastPage].Selected = true;
				lView_cvPDFPageList.EnsureVisible(lastPage);
			}
		}


		// 削除一覧からページ除去
		private void RemoveFromDeleteList(int page)
		{
			for (int i1 = 0; i1 < undoPageList.Count; i1++)
			{
				if (undoPageList[i1] == page)
				{
					undoPageList.RemoveAt(i1);
				}
			}
		}


		// ページ番号からPDF情報のエントリ取得
		private PDFPageEntry GetPageEntry(int page)
		{
			for (int i1 = 0; i1 < pdfPageEntryList.Count; i1++)
			{
				PDFPageEntry entry = pdfPageEntryList[i1];
				if (entry.Page == page)
				{
					return entry;
				}
			}
			return null;
		}


		private void FormMain_DragEnter(object sender, DragEventArgs e)
		{
			if (tBox_cvPDFFile.Enabled == false)
			{
				return;
			}
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] drags = (string[])e.Data.GetData(DataFormats.FileDrop);
				if (drags.Length != 1)
				{
					return;
				}
				if (!File.Exists(drags[0]))
				{
					return;
				}
				e.Effect = DragDropEffects.Copy;
			}
		}


		private void FormMain_DragDrop(object sender, DragEventArgs e)
		{
			lab_cvMessage.Text = "";
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (files.Length != 1)
			{
				lab_cvMessage.Text = "複数のファイルが選択されています。";
				return;
			}
			tBox_cvPDFFile.Text = files[0];
		}
	}



	public class PDFPageEntry
	{
		private int page = -1;
		private int totalPage = -1;
		private string parentFile = null;
		private string fileName = null;
		private string filePath = null;
		private int pageState = PS_NONE;
		private int categoryNo = -1;

		public const int PS_NONE = 0;
		public const int PS_DELETE = 10;
		public const int PS_CATEGORIZED = 20;


		public PDFPageEntry(int pg, int tpg, string pFile, string fName, string fPath)
		{
			page = pg;
			totalPage = tpg;
			parentFile = pFile;
			fileName = fName;
			filePath = fPath;
		}


		public int Page
		{
			get
			{
				return page;
			}
		}


		public int TotalPage
		{
			get
			{
				return totalPage;
			}
		}


		public string ParentFile
		{
			get
			{
				return parentFile;
			}
		}


		public string FileName
		{
			get
			{
				return fileName;
			}
		}


		public string FilePath
		{
			get
			{
				return filePath;
			}
		}


		public int PageState
		{
			get
			{
				return pageState;
			}

			set
			{
				pageState = value;
			}
		}


		public int CategoryNo
		{
			get
			{
				return categoryNo;
			}

			set
			{
				categoryNo = value;
			}
		}
	}
}
