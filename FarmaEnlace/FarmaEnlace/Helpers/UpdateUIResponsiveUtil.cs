using System;
using Xamarin.Forms;

namespace FarmaEnlace.Helpers
{
    public static class UpdateUIResponsiveUtil
    {
        public static void ChangeUIThisPage(View content, int numLabelsNormal = 0)
        {
            if (content != null)
            {
                Label textTitle = content.FindByName<Label>("LabelFontSizeTitleDynamic");
                if (textTitle != null)
                {
                    ChangeFontSizeTitle(textTitle);
                }

                //cuando la page tiene varios labels.... 
                //e intentado buscar una forma de conseguir todos los de una page... pero nada por lo tanto
                //se deja de esta forma
                for (int i = 1; i <= numLabelsNormal; i++)
                {
                    Label textNormal = content.FindByName<Label>("LabelFontSizeDymanic" + i);
                    if (textNormal != null)
                    {
                        ChangeFontSizeNormal(textNormal);
                    }
                }                                                             
            }
        }

        public static void ChangeFontSizeTitle(Label textTitle)
        {
            if (App.ScreenHeight >= 1024)
            {     //este seria el tamanio en un Ipad           
                if(textTitle.Text.Length > 12)
                {
                    textTitle.FontSize = 40;
                }
                else
                {
                    textTitle.FontSize = 50;
                }
            }
            else if (App.ScreenHeight >= 600)
            {
                //telefono
                if(textTitle.Text.Length > 12)
                {
                    textTitle.FontSize = 20;
                }
                else
                {
                    textTitle.FontSize = 22;
                }                
            }
        }

        public static void ChangeFontSizeNormal(Label textNormal)
        {
            if (App.ScreenHeight >= 1024)
            {
                textNormal.FontSize = 30;
            }
            else if (App.ScreenHeight >= 600)
            {
                //telefono
                textNormal.FontSize = 18;
            }
        }        
    }
}
