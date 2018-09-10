/// <summary>
/// Clase que define RadioButtons.
/// Referencia: Xlabs-Xamarin-Forms
/// </summary>

using FarmaEnlace.Helpers;
using System;
using Xamarin.Forms;


namespace FarmaEnlace.Renderers
{  

    public class CustomRadioButton : View
    {
        /// <summary>
        /// Propiedad de item seleccioando.
        /// </summary>
        public static readonly BindableProperty CheckedProperty = BindableProperty.Create<CustomRadioButton, bool>(p => p.Checked, false);

        /// <summary>
        /// Propiedad de texto.
        /// </summary>
        public static readonly BindableProperty TextProperty = BindableProperty.Create<CustomRadioButton, string>(p => p.Text, string.Empty);

        /// <summary>
        /// Propiedad de selecciòn de item.
        /// </summary>
        public EventHandler<EventArgs<bool>> CheckedChanged;


        /// <summary>
        /// Propiedad vinculable que identifica el color de texto.
        /// </summary>
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create<CustomRadioButton, Color>(p => p.TextColor, Color.Black);

        /// <summary>
        /// Establece u obtiene el estado de selección del item.
        /// </summary>
        /// <value>Estado de selección del item.</value>
        public bool Checked
        {
            get
            {
                return this.GetValue<bool>(CheckedProperty);
            }

            set
            {
                this.SetValue(CheckedProperty, value);
                EventHandler<EventArgs<bool>> eventHandler = this.CheckedChanged;
                if (eventHandler != null)
                {
                    eventHandler.Invoke(this, value);
                }
            }
        }

        /// <summary>
        /// Establece u obtiene el texto para cada item.
        /// </summary>
        /// <value>Valor del texto de cada item.</value>
        public string Text
        {
            get
            {
                return this.GetValue<string>(TextProperty);
            }

            set
            {
                this.SetValue(TextProperty, value);
            }
        }

        /// <summary>
        /// Establece u obtiene el color de texto del radio button.
        /// </summary>
        /// <value>Valor del color de texto.</value>
        public Color TextColor
        {
            get
            {
                return this.GetValue<Color>(TextColorProperty);
            }

            set
            {
                this.SetValue(TextColorProperty, value);
            }
        }

        /// <summary>
        /// Establece u obtiene le valor numérico del índice de cada item.
        /// </summary>
        /// <value>Valor del índice.</value>
        public new int Id { get; set; }

    }
}
