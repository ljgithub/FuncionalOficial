using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using FarmaEnlace.Interfaces;
using FarmaEnlace.Models;
using FarmaEnlace.Services;
using FarmaEnlace.Views;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;


namespace FarmaEnlace.ViewModels
{
    public class MainViewModel
    {


        #region Services
        NavigationService navigationService;
        #endregion

        #region Properties
        public string PhoneNumber
        {
            get;
            set;
        }
        
        public bool IsVisibleMyMenu
        {
            get;
            set;
        }
        public bool IsVisibleMyMenuUser
        {
            get;
            set;
        }
        
        public Brand Brand
        {
            get;
            set;
        }

        public ObservableCollection<FarmaEnlace.Models.Menu> MyMenu
        {
            get;
            set;
        }
        public ObservableCollection<FarmaEnlace.Models.Menu> MyMenuUser
        {
            get;
            set;
        }
        public NewCustomerViewModel NewCustomer
        {
            get;
            set;
        }

        public Product Product
        {
            get;
            set;
        }
        public ImageBrand ImageBrand
        {
            get;
            set;
        }
        public DetailPromotionViewModel DetailPromotion
        {
            get;
            set;
        }
        
        public LoginViewModel Login
        {
            get;
            set;
        }

        public BrandsViewModel Brands
        {
            get;
            set;
        }

        public DetailBrandsViewModel DetailBrands
        {
            get;
            set;
        }

        public CategoriesViewModel Categories
        {
            get;
            set;
        }
        public CategoriesSub1ViewModel CategoriesSub1
        {
            get;
            set;
        }
        public CategoriesSub2ViewModel CategoriesSub2
        {
            get;
            set;
        }
        public CategoriesSub3ViewModel CategoriesSub3
        {
            get;
            set;
        }
        public CategoriesSub4ViewModel CategoriesSub4
        {
            get;
            set;
        }
        
        public  TokenResponse Token
        {
            get;
            set;
   
        }

        public ProductsViewModel Products
        {
            get;
            set;
        }

        public DetailProductViewModel DetailProduct
        {
            get;
            set;
        }


        public Category Category
        {
            get;
            set;
        }
        
        public DetailCommerceViewModel DetailCommerces
        {
            get;
            set;
        }

        public CommercesListViewModel CommercesList
        {
            get;
            set;
        }

        public CommerceMapViewModel CommerceMap
        {
            get;
            set;
        }
        
        public CommercesViewModel Commerces
        {
            get;
            set;
        }

        public CommercesSearchViewModel CommercesSearch
        {
            get;
            set;
        }

        public SyncViewModel Sync
        {
            get;
            set;
        }

        public MyProfileViewModel MyProfile
        {
            get;
            set;
        }
        public ChangeUserViewModel ChangeUser
        {
            get;
            set;
        }

        public PasswordRecoveryViewModel PasswordRecovery
        {
            get;
            set;
        }

        public VirtualCardViewModel VirtualCard
        {
            get;
            set;
        }

        public CommercesListMapViewModel CommercesListMap
        {
            get;
            set;
        }


        #endregion

        #region Constructors
        public MainViewModel()
        {
            navigationService = new NavigationService();            
            instance = this;
            //Brands = new BrandsViewModel();
            LoadMenu();
            LoadMenuUser();
        }
        #endregion

        #region Sigleton
        static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        #endregion

        #region Methods
        public void RegisterDevice()
        {
            var register = DependencyService.Get<IRegisterDevice>();
            register.RegisterDevice();
        }

        void LoadMenu()
        {
            MyMenu = new ObservableCollection<FarmaEnlace.Models.Menu>();
            MyMenu.Add(new FarmaEnlace.Models.Menu
            {
                Icon = "ic_map",
                PageName = "CategoriesLineView",
                Title = "Productos",
            });
            MyMenu.Add(new FarmaEnlace.Models.Menu
            {
                Icon = "ic_map",
                PageName = "CommercesView",
                Title = "Encuéntramos",
            });
        }
        void LoadMenuUser()
        {
            MyMenuUser = new ObservableCollection<FarmaEnlace.Models.Menu>();

           
            MyMenuUser.Add(new FarmaEnlace.Models.Menu
            {
                Icon = "iconusuario",
                PageName = "ChangeUserView",
                Title = "Editar perfil",
            });
            MyMenuUser.Add(new FarmaEnlace.Models.Menu
            {
                Icon = "iconclave",
                PageName = "MyProfileView",
                Title = "Cambiar clave",
            });
            MyMenuUser.Add(new FarmaEnlace.Models.Menu
            {
                Icon = "iconcerrar",
                PageName = "LoginView",
                Title = "Cerrar",
            });
        }
        #endregion

        #region Commands

        public ICommand CallCommand
        {
            get
            {
                return new RelayCommand(Call);
            }
        }

        public void Call()
        {

            Device.OpenUri(new Uri("tel://" + Brand.Phone));

        }



         public ICommand ReturnHomeCommand
        {
            get
            {
                return new RelayCommand(ReturnHome);
            }
        }

        async public void ReturnHome()
        {
            await navigationService.BackOnNavigation();
        }
        public ICommand ReturnBrandCommand
        {
            get
            {
                return new RelayCommand(ReturnBrand);
            }
        }

        async public void ReturnBrand()
        {
            await navigationService.BackOnBrand();
        }
        #endregion


    }
}
