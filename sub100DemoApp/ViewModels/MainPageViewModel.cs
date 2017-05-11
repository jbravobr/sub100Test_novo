using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using PropertyChanged;
using sub100DemoApp.ViewModels.Bases;

namespace sub100DemoApp.ViewModels
{
    [ImplementPropertyChanged]
    public class MainPageViewModel : BaseViewModel
    {
        readonly INavigationService _navigationService;
        readonly IApplicationService<Imovei> _imovelService;
        readonly IRootObjectService<Imovei> _rootObjectService;
        readonly IDialogsFunctions _dialogService;
        readonly IPageDialogService _pageService;

        public ObservableCollection<Imovei> Imoveis { get; set; }
        public DelegateCommand<Imovei> ItemTappedCommand { get; set; }
        public DelegateCommand OrderByCommand { get; set; }
        public bool IsDataPresented { get; set; } = true;

        public MainPageViewModel(IDialogsFunctions dialogService,
                                 IApplicationService<Imovei> imovelService,
                                 INavigationService navigationService,
                                 IRootObjectService<Imovei> rootObjectService,
                                 IPageDialogService pageService)
        {
            try
            {
                _navigationService = navigationService;
                _dialogService = dialogService;
                _imovelService = imovelService;
                _rootObjectService = rootObjectService;
                _pageService = pageService;

                ItemTappedCommand = new DelegateCommand<Imovei>(ItemTapped);
                OrderByCommand = new DelegateCommand(OrderBy);
                SetData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Action OrderBy
        {
            get
            {
                return new Action(async () =>
                {
                    try
                    {
                        var orderSelection = await _pageService.DisplayActionSheetAsync("Ordenar por",
                                                                   "Cancelar",
                                                                   "Limpar",
                                                                   new string[]
                        {
                            EnumOrderType.Preco.GetDescription(),
                            EnumOrderType.Dormitorios.GetDescription(),
                            EnumOrderType.Suites.GetDescription(),
                            EnumOrderType.Vagas.GetDescription(),
                            EnumOrderType.AreaTotal.GetDescription()
                        });

                        if (orderSelection != null && !orderSelection.Equals("Limpar"))
                            OrderList(orderSelection);
                        else if (orderSelection.Equals("Limpar"))
                            Imoveis = new ObservableCollection<Imovei>(Imoveis.OrderBy(x => x.Id));
                    }
                    catch (Exception ex)
                    {
                        _dialogService.ShowToast(EnumToastType.Error, ex.Message);
                    }
                });
            }
        }

        void OrderList(string orderBy)
        {
            _dialogService.ShowLoading("Reordenando");

            try
            {
                if (orderBy.Equals(EnumOrderType.Preco.GetDescription()))
                    Imoveis = new ObservableCollection<Imovei>(Imoveis.OrderByDescending(x => x.PrecoVenda));
                if (orderBy.Equals(EnumOrderType.Dormitorios.GetDescription()))
                    Imoveis = new ObservableCollection<Imovei>(Imoveis.OrderByDescending(x => x.Dormitorios));
                if (orderBy.Equals(EnumOrderType.Suites.GetDescription()))
                    Imoveis = new ObservableCollection<Imovei>(Imoveis.OrderByDescending(x => x.Suites));
                if (orderBy.Equals(EnumOrderType.Vagas.GetDescription()))
                    Imoveis = new ObservableCollection<Imovei>(Imoveis.OrderByDescending(x => x.Vagas));
                if (orderBy.Equals(EnumOrderType.AreaTotal.GetDescription()))
                    Imoveis = new ObservableCollection<Imovei>(Imoveis.OrderByDescending(x => x.AreaTotal));

                _dialogService.HideLoading();
            }
            catch (Exception ex)
            {
                _dialogService.ShowToast(EnumToastType.Error, ex.Message);
                _dialogService.HideLoading();
            }
        }

        Action<Imovei> ItemTapped
        {
            get
            {
                return new Action<Imovei>(async (imovel) =>
                {
                    await NavigateTo(EnumMenuPages.DetailsPage.GetDescription(), imovel.CodImovel);
                });
            }
        }

        async Task NavigateTo(string page, int imovelId)
        {
            try
            {
                var parameters = new NavigationParameters
                {
                    { "imovelId", imovelId }
                };
                await _navigationService.NavigateAsync($"{page}", parameters);
            }
            catch (Exception ex)
            {
                _dialogService.ShowToast(EnumToastType.Error, ex.Message);
            }
        }

        async void SetData()
        {
            try
            {
                _dialogService.ShowLoading("Analisando imóveis");
                var data = await _rootObjectService.Get(EnumAPIPath.imoveis);

                if (data != null)
                {
                    var imoveis = await _imovelService.GetAll();
                    if (imoveis != null && imoveis.Any())
                        Imoveis = new ObservableCollection<Imovei>(imoveis.OrderBy((x => x.Id)));
                }
                else
                    IsDataPresented = false;

                _dialogService.HideLoading();
            }
            catch (Exception ex)
            {
                _dialogService.ShowToast(EnumToastType.Error, ex.Message);
                _dialogService.HideLoading();
            }
        }
    }
}