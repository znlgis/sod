using System;
using System.Runtime.InteropServices;

namespace PWMIS.EnterpriseFramework.Common
{
	public sealed class MemoryMappedFile : IDisposable
	{
		private const int FILE_MAP_WRITE = 0x2;
		private const int FILE_MAP_READ = 0x0004;
		
		
		[DllImport("kernel32.dll",EntryPoint="OpenFileMapping",SetLastError=true, CharSet=CharSet.Auto) ]
		private static extern IntPtr OpenFileMapping (int dwDesiredAccess, bool bInheritHandle,String lpName );

		[DllImport("Kernel32.dll",EntryPoint="CreateFileMapping",SetLastError=true,CharSet=CharSet.Auto)]
		private static extern IntPtr CreateFileMapping(uint hFile, IntPtr lpAttributes, uint flProtect,uint dwMaximumSizeHigh, uint dwMaximumSizeLow, string lpName);
    
		[DllImport("Kernel32.dll")]
		private static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject,uint dwDesiredAccess, uint dwFileOffsetHigh,uint dwFileOffsetLow, uint dwNumberOfBytesToMap);
    
		[DllImport("Kernel32.dll",EntryPoint="UnmapViewOfFile",SetLastError=true,CharSet=CharSet.Auto)]
		private static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);
		
		[DllImport("kernel32.dll",EntryPoint="CloseHandle",SetLastError=true,CharSet=CharSet.Auto)]
		private static extern bool CloseHandle(uint hHandle);

		[DllImport("kernel32.dll",EntryPoint="GetLastError",SetLastError=true,CharSet=CharSet.Auto)]
		private static extern uint GetLastError();

		private IntPtr memoryFileHandle;
		
		public enum FileAccess : int
		{
			ReadOnly = 2,
			ReadWrite = 4
		}

		private MemoryMappedFile(IntPtr memoryFileHandle)
		{
			this.memoryFileHandle = memoryFileHandle;
		}
		
		public static MemoryMappedFile CreateMMF(string fileName, FileAccess access, int size)
		{
			if(size < 0)
				throw new ArgumentException("The size parameter should be a number greater than Zero.");

			IntPtr memoryFileHandle = CreateFileMapping(0xFFFFFFFF,IntPtr.Zero,(uint)access,0,(uint)size,fileName);
			if(memoryFileHandle == IntPtr.Zero)
				throw new SharedMemoryException("Creating Shared Memory failed.");

			return new MemoryMappedFile(memoryFileHandle);
		}

		public static IntPtr ReadHandle(string fileName)
		{
			IntPtr mappedFileHandle = OpenFileMapping((int)FileAccess.ReadWrite ,false,fileName);
			if(mappedFileHandle == IntPtr.Zero)
				throw new SharedMemoryException("Opening the Shared Memory for Read failed.");
			
			IntPtr mappedViewHandle = MapViewOfFile(mappedFileHandle,(uint)FILE_MAP_READ,0,0,8);
			if(mappedViewHandle == IntPtr.Zero)
				throw new SharedMemoryException("Creating a view of Shared Memory failed.");
			
			IntPtr windowHandle = Marshal.ReadIntPtr(mappedViewHandle);
			if(windowHandle == IntPtr.Zero)
				throw new ArgumentException ("Reading from the specified address in  Shared Memory failed.");

			UnmapViewOfFile(mappedViewHandle);
			CloseHandle((uint)mappedFileHandle);
			return windowHandle;
		}

		public void WriteHandle(IntPtr windowHandle)
		{
			IntPtr mappedViewHandle = MapViewOfFile(memoryFileHandle,(uint)FILE_MAP_WRITE,0,0,8);
			if(mappedViewHandle == IntPtr.Zero)
				throw new SharedMemoryException("Creating a view of Shared Memory failed.");
			
			Marshal.WriteIntPtr(mappedViewHandle,windowHandle );

			UnmapViewOfFile(mappedViewHandle);
			CloseHandle((uint)mappedViewHandle);
		}


		#region IDisposable Member
		public void Dispose()
		{
			if(memoryFileHandle != IntPtr.Zero)
			{
				if(CloseHandle((uint)memoryFileHandle))
					memoryFileHandle = IntPtr.Zero;
			}
		}

		#endregion
	}

	[Serializable]
	public class SharedMemoryException: ApplicationException 
	{
		public SharedMemoryException()
		{
			
		}
		public SharedMemoryException(string message): base(message)
		{
		}
		public SharedMemoryException(string message, Exception inner): base(message, inner)
		{
		}

	}
}
