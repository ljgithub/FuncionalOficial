using FarmaEnlace.Renderers;
using Xamarin.Forms;

namespace FarmaEnlace.Helpers
{
    public class ImageEntrySquareViewBehavior : Behavior<ImageEntrySquare>
    {
        public int MaxLength { get; set; }
        protected override void OnAttachedTo(ImageEntrySquare bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(ImageEntrySquare bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (ImageEntrySquare)sender;

            // if Entry text is longer then valid length
            if (!string.IsNullOrEmpty(entry.Text))
            {
                if (entry.Text.Length > this.MaxLength)
                {
                    string entryText = entry.Text;

                    entryText = entryText.Remove(entryText.Length - 1); // remove last char

                    entry.Text = entryText;
                }
            }
           
        }
    }
}
