using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace FarmaEnlace.Renderers
{
    public class CustomMap : Map
    {
        public List<CustomPin> CustomPins { get; set; }
    }
}
