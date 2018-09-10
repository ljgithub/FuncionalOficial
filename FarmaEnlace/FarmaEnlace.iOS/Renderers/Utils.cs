
using Xamarin.Forms;

namespace FarmaEnlace.iOS.Renderers
{
    public static class Utils
    {
        /// <summary>
        /// Devuelve el nombre del logo resposive de acuerdo al ancho de la pantalla y tamanio
        /// </summary>
        /// <param name="name">Nombre del logo qe se desea responsive</param>
        /// <returns>nombre del logo completo de acuerdo al dispositivo</returns>
        public static string GetLogoUIResponsive(string nameImg)
        {
            string logo = "";

            if(Device.Idiom == TargetIdiom.Phone)
            {
                logo = GetNameLogoiPhone(nameImg);
            }
            else if (Device.Idiom == TargetIdiom.Tablet)
            {
                logo = GetNameLogoiPad(nameImg);
            }
            
            return logo;
        }

        public static string GetNameLogoiPhone(string nameImg)
        {
            string logo = nameImg + ".png";

            if (App.ScreenWidth == 375 && App.ScreenHeight == 812)
            {
                //iphone X
                return nameImg + "_X.png";
            }
            
            if(App.ScreenWidth >= 414)
            {
                return nameImg + "414.png";
            }

            if(App.ScreenWidth >= 375)
            {
                return nameImg + "375.png";
            }

            if(App.ScreenWidth >= 320)
            {
                return nameImg + "320.png";
            }

            return logo;
        }

        public static string GetNameLogoiPad(string nameImg)
        {
            string logo = nameImg + "_ipad.png"; 
            if (App.ScreenWidth >= 1024)
            {
                //logo en el iPad Pro de 12.9
                return nameImg + "_ipad1024.png";
            }

            if (App.ScreenWidth >= 834)
            {
                return nameImg + "_ipad834.png";
            }

            if (App.ScreenWidth >= 768)
            {
                //iPad Air 2
                return nameImg + "_ipad768.png";
            }

            return logo;
        }
    }
}