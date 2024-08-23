// Decompiled with JetBrains decompiler
// Type: Swapper.Program
// Assembly: Swapper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84DD08F2-3DCF-4093-9E68-169A51C5CF63
// Assembly location: C:\Users\WDAGUtilityAccount\Downloads\Swapper.exe

using System;
using System.IO;
using System.Linq;

#nullable disable
namespace Swapper
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      if (args.Length != 3)
      {
        Console.WriteLine("Usage: Swapper.exe <ucas-file> <hex-to-search> <hex-to-replace>");
      }
      else
      {
        string path = args[0];
        string hex1 = args[1].Replace(" ", "");
        string hex2 = args[2].Replace(" ", "");
        if (hex1.Length != hex2.Length)
        {
          Console.WriteLine("Error: The lengths of the hex values should be the same.");
        }
        else
        {
          try
          {
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            {
              byte[] numArray = new byte[4096];
              byte[] byteArray1 = Program.StringToByteArray(hex1);
              byte[] byteArray2 = Program.StringToByteArray(hex2);
              int num;
              while ((num = fileStream.Read(numArray, 0, 4096)) > 0)
              {
                Program.ReplaceHexValue(numArray, byteArray1, byteArray2, num);
                fileStream.Seek((long) -num, SeekOrigin.Current);
                fileStream.Write(numArray, 0, num);
              }
            }
            Console.WriteLine("Hex value replaced successfully.");
          }
          catch (Exception ex)
          {
            Console.WriteLine("An error occurred: " + ex.Message);
          }
        }
      }
    }

    private static void ReplaceHexValue(
      byte[] input,
      byte[] hexToReplace,
      byte[] newHexValue,
      int bytesRead)
    {
      for (int index1 = 0; index1 <= bytesRead - hexToReplace.Length; ++index1)
      {
        bool flag = true;
        for (int index2 = 0; index2 < hexToReplace.Length; ++index2)
        {
          if ((int) input[index1 + index2] != (int) hexToReplace[index2])
          {
            flag = false;
            break;
          }
        }
        if (flag)
        {
          for (int index3 = 0; index3 < hexToReplace.Length; ++index3)
            input[index1 + index3] = newHexValue[index3];
        }
      }
    }

    private static byte[] StringToByteArray(string hex)
    {
      return Enumerable.Range(0, hex.Length).Where<int>((Func<int, bool>) (x => x % 2 == 0)).Select<int, byte>((Func<int, byte>) (x => Convert.ToByte(hex.Substring(x, 2), 16))).ToArray<byte>();
    }
  }
}
