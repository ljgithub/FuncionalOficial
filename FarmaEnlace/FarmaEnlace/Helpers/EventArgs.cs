/// <summary>
/// Clase que define un argumento tipo evento para los radio buttons.
/// Autor: LogicStudio S.A.
/// Referencia: Xlabs-Xamarin-Forms
/// Fecha Creación: julio 2017
/// </summary>

using System;

namespace FarmaEnlace.Helpers
{
    public class EventArgs<T> : EventArgs
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public EventArgs(T value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Estabelce u obtiene el valor del arguetno tipo evento.
        /// </summary>
        public T Value { get; private set; }
    }
}
