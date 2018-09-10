using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FarmaEnlace.Helpers
{
    
  /// <summary>
  /// This is a hideable toolbar item for xaml views. It operates by setting the
  /// text for the toolbar item to "" when it is invisible and removing the
  /// command so that it can't be clicked on.
  /// 
  /// When the toolbar item is made visible, the original settings are applied.
  /// </summary>
  public class HideableToolbarItem : ToolbarItem
    {
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public static readonly BindableProperty IsVisibleProperty =
          BindableProperty.Create(nameof(IsVisible),
            typeof(bool),
            typeof(HideableToolbarItem),
            true,
            propertyChanged: OnIsVisibleChanged);

        private string oldText = "";
        private System.Windows.Input.ICommand oldCommand = null;
        private FileImageSource oldIcon = null;

        private static void OnIsVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var item = bindable as HideableToolbarItem;

            var newValueBool = (bool)newValue;
            var oldValueBool = (bool)oldValue;

            if (!newValueBool && oldValueBool)
            {
                item.oldText = item.Text;
                item.oldCommand = item.Command;
                item.oldIcon = item.Icon;
                item.Text = "";
                item.Icon = null;
                item.Command = null;
            }

            if (newValueBool && !oldValueBool)
            {
                item.Text = item.oldText;
                item.Command = item.oldCommand;
                item.Icon = item.oldIcon;
            }
        }
    }
}
