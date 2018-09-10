using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using FarmaEnlace.Services;
using GalaSoft.MvvmLight.Command;
using SQLite;
using Xamarin.Forms;

namespace FarmaEnlace.Models
{
    public class ImageBrand
    {
        [PrimaryKey]
        public int ImageId { get; set; }
        public int BrandId { get; set; }
        public string ImageName { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public string Url { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DisplayOrder { get; set; }
        public int DisplayTime { get; set; }
        public string Remarks { get; set; }
        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(Image))
                {
                    return "noimage";
                }

                return string.Format(
                    Application.Current.Resources["AdministradorAppFarmaEnlace"].ToString(),
                    Image.Substring(1));
            }
        }


        #region Services
        NavigationService navigationService;
        #endregion

        #region Constructor
        public ImageBrand()
        {
            navigationService = new NavigationService();
        }
        #endregion
        public ICommand ReturnCommand
        {
            get
            {
                return new RelayCommand(Return);
            }
        }

        async void Return()
        {
            await navigationService.Back();
        }

        public ICommand ReturnMenuCommand
        {
            get
            {
                return new RelayCommand(ReturnMenu);
            }
        }

        async void ReturnMenu()
        {
            await navigationService.BackOn();
        }


  



    }
}
