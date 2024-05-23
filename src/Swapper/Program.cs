using System;
using System.IO;
using System.Linq;

namespace Swapper
{
	// Token: 0x02000002 RID: 2
	internal class Program
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static void Main(string[] args)
		{
			bool flag = args.Length != 3;
			if (flag)
			{
				Console.WriteLine("Usage: Swapper.exe <ucas-file> <hex-to-search> <hex-to-replace>");
			}
			else
			{
				string path = args[0];
				string text = args[1].Replace(" ", "");
				string text2 = args[2].Replace(" ", "");
				bool flag2 = text.Length != text2.Length;
				if (flag2)
				{
					Console.WriteLine("Error: The lengths of the hex values should be the same.");
				}
				else
				{
					try
					{
						using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
						{
							byte[] array = new byte[4096];
							byte[] hexToReplace = Program.StringToByteArray(text);
							byte[] newHexValue = Program.StringToByteArray(text2);
							int num;
							while ((num = fileStream.Read(array, 0, 4096)) > 0)
							{
								Program.ReplaceHexValue(array, hexToReplace, newHexValue, num);
								fileStream.Seek((long)(-(long)num), SeekOrigin.Current);
								fileStream.Write(array, 0, num);
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

		// Token: 0x06000002 RID: 2 RVA: 0x00002198 File Offset: 0x00000398
		private static void ReplaceHexValue(byte[] input, byte[] hexToReplace, byte[] newHexValue, int bytesRead)
		{
			for (int i = 0; i <= bytesRead - hexToReplace.Length; i++)
			{
				bool flag = true;
				for (int j = 0; j < hexToReplace.Length; j++)
				{
					bool flag2 = input[i + j] != hexToReplace[j];
					if (flag2)
					{
						flag = false;
						break;
					}
				}
				bool flag3 = flag;
				if (flag3)
				{
					for (int k = 0; k < hexToReplace.Length; k++)
					{
						input[i + k] = newHexValue[k];
					}
				}
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000221C File Offset: 0x0000041C
		private static byte[] StringToByteArray(string hex)
		{
			return (from x in Enumerable.Range(0, hex.Length)
			where x % 2 == 0
			select Convert.ToByte(hex.Substring(x, 2), 16)).ToArray<byte>();
		}
	}
}
