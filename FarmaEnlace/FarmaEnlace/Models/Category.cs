using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;
using Xamarin.Forms;

namespace FarmaEnlace.Models
{
    public class Category
    {
		#region Properties
        [PrimaryKey]
        public int CategoryId { get; set; }

        public int BrandId { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }
        
        public int Order { get; set; }

        public bool SearchProduct { get; set; }

        public string Image { get; set; }

        public string SearchCode { get; set; }
        //[ManyToOne]
        //public Category Parent { get; set; }

        public bool IsActive { get; set; }

        public int FarmaEnlaceId { get; set; }

        public string ImageLine { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(Image))
                {
                    return "noimage";
                }

                return string.Format(
                    Application.Current.Resources["AdministradorAppFarmaEnlace"].ToString(),
                    Image.Substring(1));
            }
        }

        public string ImageLineFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImageLine))
                {
                    return "noimage";
                }

                return string.Format(
                    Application.Current.Resources["AdministradorAppFarmaEnlace"].ToString(),
                    ImageLine.Substring(1));
            }
        }
        public override int GetHashCode()
        {
            return CategoryId;
        }

        #endregion

    }
}
