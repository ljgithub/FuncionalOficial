﻿using System.IO;

namespace FarmaEnlace.Helpers
{
    public class FilesHelper
	{
		public static byte[] ReadFully(Stream input)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				input.CopyTo(ms);
				return ms.ToArray();
			}
		}
	}
}
