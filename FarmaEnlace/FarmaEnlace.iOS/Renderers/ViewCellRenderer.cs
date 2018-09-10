using UIKit;

using FarmaEnlace.iOS.Renderers;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(ViewCell), typeof(ViewCellRenderer))]

namespace FarmaEnlace.iOS.Renderers
{
    public class ViewCellRenderer : Xamarin.Forms.Platform.iOS.ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            reusableCell = base.GetCell(item, reusableCell, tv);
            reusableCell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return reusableCell;
        }
    }
}