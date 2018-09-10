using FarmaEnlace.Interfaces;

namespace FarmaEnlace.iOS.Implementations
{
	public class FileStore : IFileStore
	{
		public string GetFilePath()
		{
			return "image.png";
		}
	}
}
