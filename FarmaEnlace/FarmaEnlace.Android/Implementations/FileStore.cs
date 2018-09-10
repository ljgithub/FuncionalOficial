using System.IO;
using FarmaEnlace.Interfaces;

namespace FarmaEnlace.Android.Implementations
{
	public class FileStore: IFileStore
	{
		public string GetFilePath()
		{
            return Path.Combine(global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "Compartir.png");
		}
        
    }
}