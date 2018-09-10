using SQLite;
using Xamarin.Forms;

namespace FarmaEnlace.Models
{
    public class Brand
    {
        [PrimaryKey]
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string TextColor { get; set; }
        public bool AllowCall { get; set; }
        public string SearchCode { get; set; }
        public string ImageButton { get; set; }
        public string ImageBase64 { get; set; }
        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImageButton))
                {
                    return "noimage";
                }

                return string.Format(
                    Application.Current.Resources["AdministradorAppFarmaEnlace"].ToString(),
                    ImageButton.Substring(1));
            }
        }
        public override int GetHashCode()
        {
            return BrandId;
        }
    }
}
