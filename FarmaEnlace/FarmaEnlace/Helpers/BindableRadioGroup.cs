/// <summary>
/// Clase que define grupos de RadioButtons.
/// Referencia: Xlabs-Xamarin-Forms
/// </summary>

using FarmaEnlace.Renderers;
using System;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;

namespace FarmaEnlace.Helpers
{
    public class BindableRadioGroup : StackLayout
    {
        /// <summary>
        /// Lista de radio buttons por defecto.
        /// </summary>
        public List<CustomRadioButton> rads;

        /// <summary>
        /// Constructor.
        /// </summary>
        public BindableRadioGroup()
        {

            rads = new List<CustomRadioButton>();
        }


        /// <summary>
        /// Propiedad de la fuente de items.
        /// </summary>
        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create<BindableRadioGroup, IEnumerable>(o => o.ItemsSource, default(IEnumerable), propertyChanged: OnItemsSourceChanged);

        /// <summary>
        /// Propiedad de item seleccionado.
        /// </summary>
        public static BindableProperty SelectedIndexProperty =
            BindableProperty.Create<BindableRadioGroup, int>(o => o.SelectedIndex, -1, BindingMode.TwoWay, propertyChanged: OnSelectedIndexChanged);

        /// <summary>
        /// Establece u obtiene la fuente de los items.
        /// </summary>
        /// <value>Fuente de los items.</value>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Establece u obtiene el item seleccionado.
        /// </summary>
        /// <value>Item seleccionado.</value>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Evento de cambio de selección de item.
        /// </summary>
        public event EventHandler<int> CheckedChanged;

        /// <summary>
        /// Maneja el cambio de la fuente de items.
        /// </summary>
        /// <value>Fuente de items.</value>
        private static void OnItemsSourceChanged(BindableObject bindable, IEnumerable oldvalue, IEnumerable newvalue)
        {
            BindableRadioGroup radButtons = bindable as BindableRadioGroup;

            radButtons.rads.Clear();
            radButtons.Children.Clear();
            if (newvalue != null)
            {

                int radIndex = 0;
                foreach (object item in newvalue)
                {
                    CustomRadioButton rad = new CustomRadioButton();
                    rad.Text = item.ToString();
                    rad.Id = radIndex;

                    rad.CheckedChanged += radButtons.OnCheckedChanged;

                    radButtons.rads.Add(rad);

                    radButtons.Children.Add(rad);
                    radIndex++;
                }
            }
        }

        /// <summary>
        /// Maneja el evento de selección de item.
        /// </summary>
        /// <value>Evento de selección de item.</value>
        private void OnCheckedChanged(object sender, EventArgs<bool> e)
        {
            if (e.Value == false) return;

            CustomRadioButton selectedRad = sender as CustomRadioButton;

            foreach (CustomRadioButton rad in rads)
            {
                if (!selectedRad.Id.Equals(rad.Id))
                {
                    rad.Checked = false;
                }
                else
                {
                    if (CheckedChanged != null)
                        CheckedChanged.Invoke(sender, rad.Id);
                }
            }
        }

        /// <summary>
        /// Maneja el evento de cambio de selección de item.
        /// </summary>
        /// <value>Evento de cambio de selección.</value>
        private static void OnSelectedIndexChanged(BindableObject bindable, int oldvalue, int newvalue)
        {
            if (newvalue == -1) return;

            BindableRadioGroup bindableRadioGroup = bindable as BindableRadioGroup;

            foreach (CustomRadioButton rad in bindableRadioGroup.rads)
            {
                if (rad.Id == bindableRadioGroup.SelectedIndex)
                {
                    rad.Checked = true;
                }
            }
        }
    }


    public static class ExtensionObjetoEnlazado
    {
        /// <summary>
        /// Permite obtener los parametros de las propiedades de los radio buttons customizados.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bindableObject">Objeto enlazado</param>
        /// <param name="property"> Propiedad del objeto</param>
        /// <returns>Regresa la propiedad del objeto</returns>
        public static T GetValue<T>(this BindableObject bindableObject, BindableProperty property)
        {
            return (T)bindableObject.GetValue(property);
        }
    }


    public static class EventExtensions
    {
        /// <summary>
        /// Activa el evento especificado.
        /// </summary>
        public static void Invoke<T>(this EventHandler<EventArgs<T>> handler, object sender, T value)
        {
            handler?.Invoke(sender, new EventArgs<T>(value));
        }

        public static bool TryInvoke<T>(this EventHandler<T> handler, object sender, T args) where T : EventArgs
        {
            EventHandler<T> handle = handler;
            if (handle != null)
            {
                handle(sender, args);
                return true;
            }

            return false;
        }
    }
}
