using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;


namespace PDFEdit
{
    public class ThumbnailCreator
    {
		private int width = 64;
		private int height = 64;


		public ThumbnailCreator()
		{
		}


		public ThumbnailCreator(int w, int h)
		{
			width = w;
			height = h;
		}


		public Image Thumbnail(string folder, string file)
		{
			string path;

			try
			{
				if (folder == null)
				{
					path = Path.GetFullPath(file);
				}
				else
				{
					path = Path.GetFullPath(Path.Combine(folder, file));
				}
			}
			catch
			{
				return null;
			}
			return Thumbnail(path);
		}


		public Image Thumbnail(string path)
		{
			Image orgImage = null;
			Image thumbImage = null;

			try
			{
				FileInfo fInfo = new FileInfo(path);
				if (String.Compare(fInfo.Extension, ".jpg", true) == 0 ||
					String.Compare(fInfo.Extension, ".gif", true) == 0 ||
					String.Compare(fInfo.Extension, ".png", true) == 0 )
				{
					orgImage = Bitmap.FromFile(path);
					thumbImage = ReduceImage(orgImage, width, height);
				}
				else
				{
					ExtractImage creator = new ExtractImage(width, height); 
					thumbImage = creator.GetThumbNail(path); 
				}
				if (thumbImage == null)
				{
					if (orgImage != null)
					{
						orgImage.Dispose();
					}
					Icon appIcon = Icon.ExtractAssociatedIcon(path);
					orgImage = appIcon.ToBitmap();
					thumbImage = MakeCenterImage(orgImage, width, height);
				}
			}
			catch /*(Exception ex)*/
			{
				return null;
			}
			finally
			{
				if (orgImage != null)
				{
					orgImage.Dispose();
				}
			}
			return thumbImage;
		}


		public Image ThumbnailFromImage(Image orgImage, string path)
		{
			Image thumbImage = null;

			try
			{
				thumbImage = ReduceImage(orgImage, width, height);
				if (thumbImage == null)
				{
					if (orgImage != null)
					{
						orgImage.Dispose();
					}
					Icon appIcon = Icon.ExtractAssociatedIcon(path);
					orgImage = appIcon.ToBitmap();
					thumbImage = MakeCenterImage(orgImage, width, height);
				}
			}
			catch /*(Exception ex)*/
			{
				return null;
			}
			finally
			{
				if (orgImage != null)
				{
					orgImage.Dispose();
				}
			}
			return thumbImage;
		}


		private Image MakeCenterImage(Image image, int width, int height)
		{
			Bitmap canvas = new Bitmap(width, height);

			Graphics g = Graphics.FromImage(canvas);
			g.FillRectangle(new SolidBrush(Color.White), 0, 0, width, height);

			g.DrawImage(image, (width - image.Width) / 2, (height - image.Height) / 2, image.Width, image.Height);
			g.Dispose();

			return canvas;
		}


		private Image ReduceImage(Image image, int width, int height)
		{
			Bitmap canvas = new Bitmap(width, height);

			Graphics g = Graphics.FromImage(canvas);
			g.FillRectangle(new SolidBrush(Color.White), 0, 0, width, height);

			float fw = (float)width / (float)image.Width;
			float fh = (float)height / (float)image.Height;
			float scale = Math.Min(fw, fh);
			fw = image.Width * scale;
			fh = image.Height * scale;

			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			g.DrawImage(image, (width - fw) / 2, (height - fh) / 2, fw, fh);
			g.Dispose();

			return canvas;
		}
	}
}
