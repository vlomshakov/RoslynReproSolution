using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace TestRoslynCodgenBlittableTypes
{
  [StructLayout(LayoutKind.Sequential)]
  public struct FILE_TIME
  {
    public FILE_TIME(long fileTime)
    {
      ftTimeLow = (uint) fileTime;
      ftTimeHigh = (uint) (fileTime >> 32);
    }

    [MethodImpl(0x100)]
    public long ToTicks()
    {
      return ((long) ftTimeHigh << 32) + ftTimeLow;
    }

    [MethodImpl(0x100)]
    public DateTime ToDateTimeUtc()
    {
      return DateTime.FromFileTimeUtc(ToTicks());
    }

    public uint ftTimeLow;
    public uint ftTimeHigh;
  }

  public sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    public SafeFindHandle() : base(true)
    {
    }

    protected override unsafe bool ReleaseHandle()
    {
      return Kernel32Dll.FindClose((void*) handle) != 0;
    }
  }


  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public unsafe struct WIN32_FIND_DATA
  {
    public uint dwFileAttributes;
    public FILE_TIME ftCreationTime;
    public FILE_TIME ftLastAccessTime;
    public FILE_TIME ftLastWriteTime;
    public uint nFileSizeHigh;
    public uint nFileSizeLow;
    public uint dwReserved0;
    public uint dwReserved1;
    public fixed char cFileName [260];
    public fixed char cAlternateFileName [14];
  }

  public static unsafe class Kernel32Dll
  {
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true, ExactSpelling = true)
    ]
    public static extern SafeFindHandle FindFirstFileW(string lpFileName, WIN32_FIND_DATA* lpFindFileData);


    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true, ExactSpelling = true)
    ]
    public static extern int FindClose(void* handle);
  }
}