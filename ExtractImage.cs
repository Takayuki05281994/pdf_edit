using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;

using System.Diagnostics;
using System.Reflection;
using System.Threading;


namespace PDFEdit
{
    public class ExtractImage : IDisposable
	{
		private IMalloc alloc;
		private bool disposed;
		private Bitmap thumbnail;
		private int width = 64;
		private int height = 64;


		public ExtractImage()
		{
		}


		public ExtractImage(int w, int h)
		{
			width = w;
			height = h;
		}


		public void Dispose()
		{
			if (!disposed)
			{
				if (alloc != null)
				{
					Marshal.ReleaseComObject(alloc);
				}
				alloc = null;
				thumbnail = null;
				disposed = true;
			}
		}


		~ExtractImage()
		{
			Dispose();
		}


		private bool GetThumbNailSub(string file, IntPtr pidl, IShellFolder item)
		{
			bool ret;
			IntPtr hBmp = IntPtr.Zero;
			IExtractImage extractImage = null;
			try
			{
				if (Path.GetFileName(PathFromPidl(pidl)).ToUpper().Equals(Path.GetFileName(file).ToUpper()))
				{
					int prgf;
					IUnknown iunk = null;
					Guid iidExtractImage = new Guid("BB2E617C-0920-11d1-9A0B-00C04FC2D6C1");
					item.GetUIObjectOf(IntPtr.Zero, 1, ref pidl, ref iidExtractImage, out prgf, ref iunk);
					extractImage = (IExtractImage) iunk;
					if (extractImage != null)
					{
						SIZE sz = new SIZE { cx = width, cy = height };
						StringBuilder location = new StringBuilder(2048);
						int priority = 0;
						int requestedColourDepth = 0x20;
						EIEIFLAG flags = EIEIFLAG.IEIFLAG_SCREEN | EIEIFLAG.IEIFLAG_ASPECT;
						int uFlags = (int)flags;
						extractImage.GetLocation(location, location.Capacity, ref priority, ref sz, requestedColourDepth, ref uFlags);
						extractImage.Extract(out hBmp);

						if (hBmp != IntPtr.Zero)
						{
							thumbnail = Image.FromHbitmap(hBmp);
						}
						Marshal.ReleaseComObject(extractImage);
						extractImage = null;
					}
					return true;
				}
				ret = false;
			}
			catch (Exception /*ex*/)
			{
				if (hBmp != IntPtr.Zero)
				{
					UnManagedMethods.DeleteObject(hBmp);
				}
				if (extractImage != null)
				{
					Marshal.ReleaseComObject(extractImage);
				}
				return false;
			}
			return ret;
		}


		public Bitmap GetThumbNail(string file)
		{
			if (!File.Exists(file) && !Directory.Exists(file))
			{
				return null;
			}
			thumbnail = null;

			IShellFolder folder = getDesktopFolder;
			if (folder == null)
			{
				return null;
			}

			IntPtr pidlMain;
			try
			{
				int cParsed;
				int pdwAttrib;
				string filePath = Path.GetDirectoryName(file);
				folder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, filePath, out cParsed, out pidlMain, out pdwAttrib);
			}
			catch (Exception)
			{
				Marshal.ReleaseComObject(folder);
				return null;
			}
			if (pidlMain == IntPtr.Zero)
			{
				Marshal.ReleaseComObject(folder);
				return null;
			}

			Guid iidShellFolder = new Guid("000214E6-0000-0000-C000-000000000046");
			IShellFolder item = null;
			try
			{
				folder.BindToObject(pidlMain, IntPtr.Zero, ref iidShellFolder, ref item);
			}
			catch (Exception)
			{
				Marshal.ReleaseComObject(folder);
				Allocator.Free(pidlMain);
				return null;
			}
			if (item != null)
			{
				IEnumIDList idEnum = null;
				try
				{
					item.EnumObjects(IntPtr.Zero, ESHCONTF.SHCONTF_NONFOLDERS | ESHCONTF.SHCONTF_FOLDERS, ref idEnum);
				}
				catch (Exception)
				{
					Marshal.ReleaseComObject(folder);
					Allocator.Free(pidlMain);
					return null;
				}
				if (idEnum != null)
				{
					IntPtr pidl = IntPtr.Zero;
					bool complete = false;
					while (!complete)
					{
						int fetched;
						if (idEnum.Next(1, ref pidl, out fetched) != 0)
						{
							pidl = IntPtr.Zero;
							complete = true;
						}
						else if (GetThumbNailSub(file, pidl, item))
						{
							complete = true;
						}
						if (pidl != IntPtr.Zero)
						{
							Allocator.Free(pidl);
						}
					}
					Marshal.ReleaseComObject(idEnum);
				}
				Marshal.ReleaseComObject(item);
			}
			Allocator.Free(pidlMain);
			Marshal.ReleaseComObject(folder);

			return thumbnail;
		}


		private static string PathFromPidl(IntPtr pidl)
		{
			StringBuilder path = new StringBuilder(2048);
			if (UnManagedMethods.SHGetPathFromIDList(pidl, path) != 0)
			{
				return path.ToString();
			}
			return string.Empty;
		}


		private IMalloc Allocator
		{
			get
			{
				if (!disposed && (alloc == null))
				{
					UnManagedMethods.SHGetMalloc(out alloc);
				}
				return alloc;
			}
		}


		private static IShellFolder getDesktopFolder
		{
			get
			{
				IShellFolder ppshf;
				UnManagedMethods.SHGetDesktopFolder(out ppshf);
				return ppshf;
			}
		}


		public Bitmap ThumbNail
		{
			get
			{
				return thumbnail;
			}
		}


		public int Width
		{
			get
			{
				return width;
			}

			set
			{
				width = value;
			}
		}


		public int Height
		{
			get
			{
				return height;
			}

			set
			{
				height = value;
			}
		}


		[Flags]
		private enum EIEIFLAG
		{
			IEIFLAG_ASPECT = 4,
			IEIFLAG_ASYNC = 1,
			IEIFLAG_CACHE = 2,
			IEIFLAG_GLEAM = 0x10,
			IEIFLAG_NOBORDER = 0x100,
			IEIFLAG_NOSTAMP = 0x80,
			IEIFLAG_OFFLINE = 8,
			IEIFLAG_ORIGSIZE = 0x40,
			IEIFLAG_QUALITY = 0x200,
			IEIFLAG_SCREEN = 0x20
		}


		[Flags]
		private enum ESFGAO
		{
			SFGAO_CANCOPY = 1,
			SFGAO_CANDELETE = 0x20,
			SFGAO_CANLINK = 4,
			SFGAO_CANMOVE = 2,
			SFGAO_CANRENAME = 0x10,
			SFGAO_CAPABILITYMASK = 0x177,
			SFGAO_COMPRESSED = 0x4000000,
			SFGAO_CONTENTSMASK = -2147483648,
			SFGAO_DISPLAYATTRMASK = 0xf0000,
			SFGAO_DROPTARGET = 0x100,
			SFGAO_FILESYSANCESTOR = 0x10000000,
			SFGAO_FILESYSTEM = 0x40000000,
			SFGAO_FOLDER = 0x20000000,
			SFGAO_GHOSTED = 0x80000,
			SFGAO_HASPROPSHEET = 0x40,
			SFGAO_HASSUBFOLDER = -2147483648,
			SFGAO_LINK = 0x10000,
			SFGAO_READONLY = 0x40000,
			SFGAO_REMOVABLE = 0x2000000,
			SFGAO_SHARE = 0x20000,
			SFGAO_VALIDATE = 0x1000000
		}


		[Flags]
		private enum ESHCONTF
		{
			SHCONTF_FOLDERS = 0x20,
			SHCONTF_INCLUDEHIDDEN = 0x80,
			SHCONTF_NONFOLDERS = 0x40
		}


		[Flags]
		private enum ESHGDN
		{
			SHGDN_FORADDRESSBAR = 0x4000,
			SHGDN_FORPARSING = 0x8000,
			SHGDN_INFOLDER = 1,
			SHGDN_NORMAL = 0
		}


		[Flags]
		private enum ESTRRET
		{
			STRRET_WSTR,
			STRRET_OFFSET,
			STRRET_CSTR
		}


		[ComImport, Guid("000214F2-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		private interface IEnumIDList
		{
			[PreserveSig]
			int Next(int celt, ref IntPtr rgelt, out int pceltFetched);
			void Skip(int celt);
			void Reset();
			void Clone(ref ExtractImage.IEnumIDList ppenum);
		}


		[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("BB2E617C-0920-11d1-9A0B-00C04FC2D6C1")]
		private interface IExtractImage
		{
			[PreserveSig]
			int GetLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszPathBuffer, int cch, ref int pdwPriority, ref ExtractImage.SIZE prgSize, int dwRecClrDepth, ref int pdwFlags);
			[PreserveSig]
			int Extract(out IntPtr phBmpThumbnail);
		}


		[ComImport, Guid("00000002-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		private interface IMalloc
		{
			[PreserveSig]
			IntPtr Alloc(int cb);
			[PreserveSig]
			IntPtr Realloc(IntPtr pv, int cb);
			[PreserveSig]
			void Free(IntPtr pv);
			[PreserveSig]
			int GetSize(IntPtr pv);
			[PreserveSig]
			int DidAlloc(IntPtr pv);
			[PreserveSig]
			void HeapMinimize();
		}


		[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214E6-0000-0000-C000-000000000046")]
		private interface IShellFolder
		{
			void ParseDisplayName(IntPtr hwndOwner, IntPtr pbcReserved, [MarshalAs(UnmanagedType.LPWStr)] string lpszDisplayName, out int pchEaten, out IntPtr ppidl, out int pdwAttributes);
			void EnumObjects(IntPtr hwndOwner, [MarshalAs(UnmanagedType.U4)] ExtractImage.ESHCONTF grfFlags, ref ExtractImage.IEnumIDList ppenumIDList);
			void BindToObject(IntPtr pidl, IntPtr pbcReserved, ref Guid riid, ref ExtractImage.IShellFolder ppvOut);
			void BindToStorage(IntPtr pidl, IntPtr pbcReserved, ref Guid riid, IntPtr ppvObj);
			[PreserveSig]
			int CompareIDs(IntPtr lParam, IntPtr pidl1, IntPtr pidl2);
			void CreateViewObject(IntPtr hwndOwner, ref Guid riid, IntPtr ppvOut);
			void GetAttributesOf(int cidl, IntPtr apidl, [MarshalAs(UnmanagedType.U4)] ref ExtractImage.ESFGAO rgfInOut);
			void GetUIObjectOf(IntPtr hwndOwner, int cidl, ref IntPtr apidl, ref Guid riid, out int prgfInOut, ref ExtractImage.IUnknown ppvOut);
			void GetDisplayNameOf(IntPtr pidl, [MarshalAs(UnmanagedType.U4)] ExtractImage.ESHGDN uFlags, ref ExtractImage.STRRET_CSTR lpName);
			void SetNameOf(IntPtr hwndOwner, IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] string lpszName, [MarshalAs(UnmanagedType.U4)] ExtractImage.ESHCONTF uFlags, ref IntPtr ppidlOut);
		}


		[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00000000-0000-0000-C000-000000000046")]
		private interface IUnknown
		{
			[PreserveSig]
			IntPtr QueryInterface(ref Guid riid, out IntPtr pVoid);
			[PreserveSig]
			IntPtr AddRef();
			[PreserveSig]
			IntPtr Release();
		}


		[StructLayout(LayoutKind.Sequential)]
		private struct SIZE
		{
			public int cx;
			public int cy;
		}


		[StructLayout(LayoutKind.Explicit, CharSet=CharSet.Auto)]
		private struct STRRET_ANY
		{
			[FieldOffset(4)]
			public IntPtr pOLEString;
			[FieldOffset(0)]
			public ExtractImage.ESTRRET uType;
		}


		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto, Pack=4)]
		private struct STRRET_CSTR
		{
			public ExtractImage.ESTRRET uType;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst=520)]
			public byte[] cStr;
		}


		private class UnManagedMethods
		{
			[DllImport("gdi32", CharSet=CharSet.Auto)]
			internal static extern int DeleteObject(IntPtr hObject);
			[DllImport("shell32", CharSet=CharSet.Auto)]
			internal static extern int SHGetDesktopFolder(out ExtractImage.IShellFolder ppshf);
			[DllImport("shell32", CharSet=CharSet.Auto)]
			internal static extern int SHGetMalloc(out ExtractImage.IMalloc ppMalloc);
			[DllImport("shell32", CharSet=CharSet.Auto)]
			internal static extern int SHGetPathFromIDList(IntPtr pidl, StringBuilder pszPath);
		}
    }
}
