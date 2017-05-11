﻿using System;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;
using PropertyChanged;
using sub100DemoApp.ViewModels.Bases;

namespace sub100DemoApp.ViewModels
{
    [ImplementPropertyChanged]
    public class ImageGalleryPreviewPageViewModel : BaseViewModel, INavigationAware
    {
        readonly INavigationService _navigationService;

        public int ImovelId { get; set; }
        public string Source { get; set; }
        public DelegateCommand OnCloseButtonCommand { get; set; }

        public ImageGalleryPreviewPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            OnCloseButtonCommand = new DelegateCommand(OnCloseButton);
        }

        Action OnCloseButton
        {
            get
            {
                return new Action(async () =>
                {
                    if (ImovelId > 0)
                    {
                        var navParameters = new NavigationParameters
                        {
                            {"ImovelId", ImovelId}
                        };
                        await GoBackTo(navParameters);
                    }
                });
            }
        }

        async Task GoBackTo(NavigationParameters navParameters)
        {
            await _navigationService.GoBackAsync(navParameters ?? navParameters);
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("ImovelId") && parameters.ContainsKey("ImageSource"))
            {
                ImovelId = (int)parameters["ImovelId"];
                Source = (string)parameters["ImageSource"];
            }
        }
    }
}