using System;
using System.ComponentModel;
using Acr.UserDialogs;
using FarmaEnlace.Helpers;
using FarmaEnlace.Models;
using FarmaEnlace.Renderers;
using FarmaEnlace.Services;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace FarmaEnlace.Views
{
    public class PopUpView : PopupPage
    {
        private int optionId = 0;
        private Customer customer = new Customer();

        public PopUpView(string textTitle, string textMessage, string imageIcon)
        {
            TapGestureRecognizer profileTapRecognizer = new TapGestureRecognizer
            {
                Command = new Command(async (o) => {
                    await PopupNavigation.PopAsync();
                }),
                NumberOfTapsRequired = 1
            };
            Image image = new Image()
            {
                Source = "btnaceptar.png",
                VerticalOptions = LayoutOptions.End,
                Scale = 0.7,
                Margin = new Thickness(0, 20, 0, 0),

            };
            image.GestureRecognizers.Add(profileTapRecognizer);
            Content = new StackLayout()
            {
                Children =
                {
                   new Frame()
                   {
                        CornerRadius = 15,
                        Padding = 2,
                        BackgroundColor = Color.FromHex("#FFFFFF"),
                        Content = new StackLayout()
                        {
                            Children =
                            {
                                new Frame()
                                {
                                    CornerRadius = 15,
                                    Padding = new Thickness(20, 40, 20, 40),
                                    BackgroundColor = Color.FromHex("#001689"),
                                    Content = new StackLayout()
                                    {
                                        Padding = new Thickness(0, 10, 0, 10),
                                        Children =
                                        {
                                            new Image()
                                            {
                                                HeightRequest = 100,
                                                Scale = 0.7,
                                                Source = imageIcon,
                                                VerticalOptions = LayoutOptions.Start
                                            },
                                            new Label()
                                            {
                                                Text = textMessage,
                                                TextColor = Color.White,
                                                FontSize = 20,
                                                HorizontalOptions = LayoutOptions.Center,
                                                HorizontalTextAlignment = TextAlignment.Center,
                                                VerticalOptions = LayoutOptions.End
                                            },
                                            image
                                        }
                                    }
                                }
                            }
                        }
                   }
                },
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(20, 20, 20, 20),
                Margin = new Thickness(25, 0, 25, 0)
            };
                     
        }

        public PopUpView(string textTitle, string textMessage, string imageIcon, string brand)
        {
            TapGestureRecognizer profileTapRecognizer = null;
            Image image = null;
            switch (brand)
            {
                case "":
                    profileTapRecognizer = new TapGestureRecognizer
                    {
                        Command = new Command(async (o) => {
                            await PopupNavigation.PopAsync();
                        }),
                        NumberOfTapsRequired = 1
                    };
                    image = new Image()
                    {
                        Source = "btnaceptar.png",
                        VerticalOptions = LayoutOptions.End,
                        Scale = 0.7,
                        Margin = new Thickness(0, 20, 0, 0),

                    };
                    image.GestureRecognizers.Add(profileTapRecognizer);
                    Content = new StackLayout()
                    {
                        Children =
                            {
                               new Frame()
                               {
                                    CornerRadius = 15,
                                    Padding = 2,
                                    BackgroundColor = Color.FromHex("#FFFFFF"),
                                    Content = new StackLayout()
                                    {
                                        Children =
                                        {
                                            new Frame()
                                            {
                                                CornerRadius = 15,
                                                Padding = new Thickness(20, 40, 20, 40),
                                                BackgroundColor = Color.FromHex("#001689"),
                                                Content = new StackLayout()
                                                {
                                                    Padding = new Thickness(0, 10, 0, 10),
                                                    Children =
                                                    {
                                                        new Image()
                                                        {
                                                            HeightRequest = 100,
                                                            Scale = 0.7,
                                                            Source = imageIcon,
                                                            VerticalOptions = LayoutOptions.Start
                                                        },
                                                        new Label()
                                                        {
                                                            Text = textMessage,
                                                            TextColor = Color.White,
                                                            FontSize = 20,
                                                            HorizontalOptions = LayoutOptions.Center,
                                                            HorizontalTextAlignment = TextAlignment.Center,
                                                            VerticalOptions = LayoutOptions.End
                                                        },
                                                        image
                                                    }
                                                }
                                            }
                                        }
                                    }
                               }
                            },
                        VerticalOptions = LayoutOptions.Center,
                        Padding = new Thickness(20, 20, 20, 20),
                        Margin = new Thickness(25, 0, 25, 0)
                    };
                    break;
                case "003":
                    profileTapRecognizer = new TapGestureRecognizer
                    {
                        Command = new Command(async (o) => {
                            await PopupNavigation.PopAsync();
                        }),
                        NumberOfTapsRequired = 1
                    };
                    image = new Image()
                    {
                        Source = "btnaceptarEco.png",
                        VerticalOptions = LayoutOptions.End,
                        Scale = 0.7,
                        Margin = new Thickness(0, 20, 0, 0),

                    };
                    image.GestureRecognizers.Add(profileTapRecognizer);
                    Content = new StackLayout()
                    {
                        Children =
                        {
                           new Frame()
                           {
                                CornerRadius = 15,
                                Padding = 2,
                                BackgroundColor = Color.FromHex("#000000"),
                                Content = new StackLayout()
                                {
                                    Children =
                                    {
                                        new Frame()
                                        {
                                            CornerRadius = 15,
                                            Padding = new Thickness(20, 40, 20, 40),
                                            BackgroundColor = Color.FromHex("#808080"),
                                            Content = new StackLayout()
                                            {
                                                Padding = new Thickness(0, 10, 0, 10),
                                                Children =
                                                {
                                                    new Image()
                                                    {
                                                        HeightRequest = 100,
                                                        Scale = 0.7,
                                                        Source = imageIcon,
                                                        VerticalOptions = LayoutOptions.Start
                                                    },
                                                    new Label()
                                                    {
                                                        Text = textMessage,
                                                        TextColor = Color.White,
                                                        FontSize = 20,
                                                        HorizontalOptions = LayoutOptions.Center,
                                                        HorizontalTextAlignment = TextAlignment.Center,
                                                        VerticalOptions = LayoutOptions.End
                                                    },
                                                    image
                                                }
                                            }
                                        }
                                    }
                                }
                           }
                        },
                        VerticalOptions = LayoutOptions.Center,
                        Padding = new Thickness(20, 20, 20, 20),
                        Margin = new Thickness(25, 0, 25, 0)
                    };
                    break;
                case "002":
                    profileTapRecognizer = new TapGestureRecognizer
                    {
                        Command = new Command(async (o) => {
                            await PopupNavigation.PopAsync();
                        }),
                        NumberOfTapsRequired = 1
                    };
                    image = new Image()
                    {
                        Source = "med_btnaceptar.png",
                        VerticalOptions = LayoutOptions.End,
                        Scale = 0.7,
                        Margin = new Thickness(0, 20, 0, 0),

                    };
                    image.GestureRecognizers.Add(profileTapRecognizer);
                    Content = new StackLayout()
                    {
                        Children =
                        {
                           new Frame()
                           {
                                CornerRadius = 15,
                                Padding = 2,
                                BackgroundColor = Color.FromHex("#FFFFFF"),
                                Content = new StackLayout()
                                {
                                    Children =
                                    {
                                        new Frame()
                                        {
                                            CornerRadius = 15,
                                            Padding = new Thickness(20, 40, 20, 40),
                                            BackgroundColor = Color.FromHex("#80ba27"),
                                            Content = new StackLayout()
                                            {
                                                Padding = new Thickness(0, 10, 0, 10),
                                                Children =
                                                {
                                                    new Image()
                                                    {
                                                        HeightRequest = 100,
                                                        Scale = 0.7,
                                                        Source = imageIcon,
                                                        VerticalOptions = LayoutOptions.Start
                                                    },
                                                    new Label()
                                                    {
                                                        Text = textMessage,
                                                        TextColor = Color.White,
                                                        FontSize = 20,
                                                        HorizontalOptions = LayoutOptions.Center,
                                                        HorizontalTextAlignment = TextAlignment.Center,
                                                        VerticalOptions = LayoutOptions.End
                                                    },
                                                    image
                                                }
                                            }
                                        }
                                    }
                                }
                           }
                        },
                        VerticalOptions = LayoutOptions.Center,
                        Padding = new Thickness(20, 20, 20, 20),
                        Margin = new Thickness(25, 0, 25, 0)
                    };
                    break;
                case "010":
                    profileTapRecognizer = new TapGestureRecognizer
                    {
                        Command = new Command(async (o) => {
                            await PopupNavigation.PopAsync();
                        }),
                        NumberOfTapsRequired = 1
                    };
                    image = new Image()
                    {
                        Source = "pun_btnaceptar.png",
                        VerticalOptions = LayoutOptions.End,
                        Scale = 0.7,
                        Margin = new Thickness(0, 20, 0, 0),

                    };
                    image.GestureRecognizers.Add(profileTapRecognizer);
                    Content = new StackLayout()
                    {
                        Children =
                        {
                           new Frame()
                           {
                                CornerRadius = 15,
                                Padding = 2,
                                BackgroundColor = Color.FromHex("#FFFFFF"),
                                Content = new StackLayout()
                                {
                                    Children =
                                    {
                                        new Frame()
                                        {
                                            CornerRadius = 15,
                                            Padding = new Thickness(20, 40, 20, 40),
                                            BackgroundColor = Color.FromHex("#95d600"),
                                            Content = new StackLayout()
                                            {
                                                Padding = new Thickness(0, 10, 0, 10),
                                                Children =
                                                {
                                                    new Image()
                                                    {
                                                        HeightRequest = 100,
                                                        Scale = 0.7,
                                                        Source = imageIcon,
                                                        VerticalOptions = LayoutOptions.Start
                                                    },
                                                    new Label()
                                                    {
                                                        Text = textMessage,
                                                        TextColor = Color.White,
                                                        FontSize = 20,
                                                        HorizontalOptions = LayoutOptions.Center,
                                                        HorizontalTextAlignment = TextAlignment.Center,
                                                        VerticalOptions = LayoutOptions.End
                                                    },
                                                    image
                                                }
                                            }
                                        }
                                    }
                                }
                           }
                        },
                        VerticalOptions = LayoutOptions.Center,
                        Padding = new Thickness(20, 20, 20, 20),
                        Margin = new Thickness(25, 0, 25, 0)
                    };
                    break;
                default:
                    profileTapRecognizer = new TapGestureRecognizer
                    {
                        Command = new Command(async (o) => {
                            await PopupNavigation.PopAsync();
                        }),
                        NumberOfTapsRequired = 1
                    };
                    image = new Image()
                    {
                        Source = "btnaceptarEco.png",
                        VerticalOptions = LayoutOptions.End,
                        Scale = 0.7,
                        Margin = new Thickness(0, 20, 0, 0),

                    };
                    image.GestureRecognizers.Add(profileTapRecognizer);
                    Content = new StackLayout()
                    {
                        Children =
                        {
                           new Frame()
                           {
                                CornerRadius = 15,
                                Padding = 2,
                                BackgroundColor = Color.FromHex("#000000"),
                                Content = new StackLayout()
                                {
                                    Children =
                                    {
                                        new Frame()
                                        {
                                            CornerRadius = 15,
                                            Padding = new Thickness(20, 40, 20, 40),
                                            BackgroundColor = Color.FromHex("#808080"),
                                            Content = new StackLayout()
                                            {
                                                Padding = new Thickness(0, 10, 0, 10),
                                                Children =
                                                {
                                                    new Image()
                                                    {
                                                        HeightRequest = 100,
                                                        Scale = 0.7,
                                                        Source = imageIcon,
                                                        VerticalOptions = LayoutOptions.Start
                                                    },
                                                    new Label()
                                                    {
                                                        Text = textMessage,
                                                        TextColor = Color.White,
                                                        FontSize = 20,
                                                        HorizontalOptions = LayoutOptions.Center,
                                                        HorizontalTextAlignment = TextAlignment.Center,
                                                        VerticalOptions = LayoutOptions.End
                                                    },
                                                    image
                                                }
                                            }
                                        }
                                    }
                                }
                           }
                        },
                        VerticalOptions = LayoutOptions.Center,
                        Padding = new Thickness(20, 20, 20, 20),
                        Margin = new Thickness(25, 0, 25, 0)
                    };
                break;
            }
        }

        public PopUpView(Customer customer)
        {
            this.optionId = 0;
            this.customer = customer;

            TapGestureRecognizer profileTapRecognizer = new TapGestureRecognizer
            {
                Command = new Command(async (o) => {
                    //enviar a la api el update de la autorizacion....

                    var apiService = new ApiService();
                    var dialogService = new DialogService();
                    UserDialogs.Instance.ShowLoading(string.Empty, MaskType.Black);
                    var connection = await apiService.CheckConnection();
                    if (!connection.IsSuccess)
                    {
                        await dialogService.ShowMessage(
                            FarmaEnlace.Resources.Resource.Error,
                            connection.Message);
                    }
                    else
                    {
                        if(optionId == 0)
                        {//SI
                            customer.AuthorizationPromotions = true;
                        }
                        else
                        {//NO
                            customer.AuthorizationPromotions = false;
                        }

                        var urlAPI = Application.Current.Resources["URLAPI"].ToString();
                        var response = await apiService.Post(
                            urlAPI,
                            Application.Current.Resources["PrefixAPI"].ToString(),
                            "/Customers/UpdateAuthorizationPromotion",
                            customer);

                        UserDialogs.Instance.HideLoading();
                        await PopupNavigation.PopAsync();

                        if (!response.IsSuccess)
                        {                           

                            await dialogService.ShowMessage(
                                "No se ha podido Guardar su elección",
                                response.Message);                           
                            
                        }
                        else
                        {
                            await dialogService.ShowMessage(
                               FarmaEnlace.Resources.Resource.Info,
                               "Gracias por preferirnos",
                               "iconperfactualic");
                        }
                    }

                    
                }),
                NumberOfTapsRequired = 1
            };
            Image image = new Image()
            {
                Source = "btnaceptarAutorization.png",
                VerticalOptions = LayoutOptions.End,
                Scale = 0.7,
                Margin = new Thickness(0, 20, 0, 0),

            };

            var options = new BindableRadioGroup
            {
                ItemsSource = new[] { "De acuerdo", "No gracias" },
                SelectedIndex = 0
            };

            options.CheckedChanged += RadiouGroup_CheckedChanged;

            image.GestureRecognizers.Add(profileTapRecognizer);
            Content = new StackLayout()
            {
                Children =
                {
                   new Frame()
                   {
                        CornerRadius = 15,
                        Padding = 2,
                        BackgroundColor = Color.FromHex("#001689"),
                        Content = new StackLayout()
                        {
                            Children =
                            {
                                new Frame()
                                {
                                    CornerRadius = 15,
                                    Padding = new Thickness(20, 40, 20, 40),
                                    Margin = new Thickness(5, 5, 5, 5),
                                    BackgroundColor = Color.FromHex("#FFFFFF"),
                                    Content = new StackLayout()
                                    {
                                        Padding = new Thickness(0, 10, 0, 10),
                                        Children =
                                        {
                                            new Label()
                                            {
                                                Text = "Gracias por preferirnos, en el futuro enviaremos más información sobre nuestras promociones y ofertas, pensadas especialmente para ti",
                                                TextColor = Color.FromHex("#001689"),
                                                FontSize = 20,
                                                HorizontalOptions = LayoutOptions.Center,
                                                HorizontalTextAlignment = TextAlignment.Center,
                                                VerticalOptions = LayoutOptions.End
                                            },
                                            options,
                                            image
                                        }
                                    }
                                }
                            }
                        }
                   }
                },
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(20, 20, 20, 20),
                Margin = new Thickness(25, 0, 25, 0)
            };

        }

        private void RadiouGroup_CheckedChanged(object sender, int e)
        {            
            CustomRadioButton radio = sender as CustomRadioButton;
            if (radio == null || radio.Id == -1) return;
            else
            {
                optionId = radio.Id;
                radio.TextColor = Color.White;
            }
        }
                
    }
}
