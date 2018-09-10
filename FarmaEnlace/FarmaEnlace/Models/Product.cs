using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;
using Xamarin.Forms;

namespace FarmaEnlace.Models
{
    public class Product
    {
		#region Properties
        [PrimaryKey]
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Barcode { get; set; }
        public string InternalCode { get; set; }
        public string TypeSale { get; set; }
        public decimal Price { get; set; }
        public string Remarks { get; set; }
        public string TypeArt { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(Image))
                {
                    return "noimage";    
                }

                if(!string.IsNullOrEmpty(TypeArt) && TypeArt == "CONTROLADO")
                {
                    return "iconotc";
                }

                if (!string.IsNullOrEmpty(TypeArt) && TypeArt == "PRESCRIPCION")
                {
                    return "iconprescripcion";
                }

                return string.Format(
                    Application.Current.Resources["ApiFarmaenlace"].ToString(), 
                    Image.Substring(1));
            }
        }  
        
        public string DescripctionControl
        {
            get
            {
                if (!string.IsNullOrEmpty(TypeArt) && TypeArt == "CONTROLADO")
                {
                    return "Medicamento controlado, venta bajo receta médica especial";
                }

                if (!string.IsNullOrEmpty(TypeArt) && TypeArt == "PRESCRIPCION")
                {
                    return "Medicamento de prescripción, venta con receta médica";
                }

                return "";
            }
        }

        public string TextColorDescripctionControl
        {
            get
            {
                if (!string.IsNullOrEmpty(TypeArt) && TypeArt == "CONTROLADO")
                {
                    return "#e60000"; //rojo mas intenso
                }

                if (!string.IsNullOrEmpty(TypeArt) && TypeArt == "PRESCRIPCION")
                {
                    return "#000000"; //azul intenso (tirando a marino)
                }

                return "#000000"; //negro
            }
        }

        public bool IsProductWithControl
        {
            get
            {
                if (!string.IsNullOrEmpty(TypeArt) && ( TypeArt == "CONTROLADO") || (TypeArt == "PRESCRIPCION") )
                {
                    return true;
                }

                return false; 
            }
        }
                        
        #endregion

        #region Methods
        public override int GetHashCode()
        {
            return ProductId;
        }
        #endregion
    }
}
