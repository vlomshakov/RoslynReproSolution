using System;
using System.IO;
using System.Runtime.InteropServices;

namespace TestRoslynCodgenBlittableTypes
{
  public static class Test
  {
    private static unsafe void Main(string[] args)
    {
      var data = new WIN32_FIND_DATA();
      using (var handle = Kernel32Dll.FindFirstFileW(@"C:\work\*", &data))
      {
        if (handle.IsInvalid)
        {
          var error = Marshal.GetLastWin32Error();
          throw new IOException("IO error with code {0}.", error);
        }
      }
      Console.WriteLine("Roslyn vs Csc: test - calling of P/Invoke with blittable stucture as argument.");
    }
  }
}
