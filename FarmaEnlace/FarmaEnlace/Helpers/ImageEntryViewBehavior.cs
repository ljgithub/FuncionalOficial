using FarmaEnlace.Renderers;
using Xamarin.Forms;

namespace FarmaEnlace.Helpers
{
    public class ImageEntryViewBehavior : Behavior<ImageEntry>
    {
        public int MaxLength { get; set; }

        protected override void OnAttachedTo(ImageEntry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(ImageEntry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (ImageEntry)sender;

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
