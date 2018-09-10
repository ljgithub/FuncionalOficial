using SQLite;
using Xamarin.Forms;

namespace FarmaEnlace.Models
{
    public class Splash
    {

        [PrimaryKey, AutoIncrement]
        public int SplashId { get; set; }
        public string ImageButton { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string ImageBase64 { get; set; }
        public override int GetHashCode()
        {
            return SplashId;
        }
    }
}
