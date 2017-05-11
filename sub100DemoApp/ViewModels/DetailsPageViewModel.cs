using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using PropertyChanged;
using sub100DemoApp.ViewModels.Bases;
using Xamarin.Forms;

namespace sub100DemoApp.ViewModels
{
    [ImplementPropertyChanged]
    public class DetailsPageViewModel : BaseViewModel, INavigationAware
    {
        public Imovel Imovel { get; set; }
        public List<string> Fotos { get; set; }
        public List<string> Caracteristicas { get; set; }
        public List<string> CaracteristicasComum { get; set; }
        public string BackgroundImage { get; set; }
        public bool IsDataPresented { get; set; } = true;
        public DelegateCommand<string> ImageTappedCommand { get; set; }
        public DelegateCommand SendCommand { get; set; }

        int ImovelId { get; set; }

        readonly IRootObjectService<Imovel> _rootObjectService;
        readonly IApplicationService<Imovel> _imovelService;
        readonly INavigationService _navigationService;
        readonly IDialogsFunctions _dialogService;

        public DetailsPageViewModel(IRootObjectService<Imovel> rootObjectService,
                                    IDialogsFunctions dialogService,
                                    IApplicationService<Imovel> imovelService,
                                    INavigationService navigationService)
        {
            _navigationService = navigationService;
            _rootObjectService = rootObjectService;
            _imovelService = imovelService;
            _dialogService = dialogService;

            ImageTappedCommand = new DelegateCommand<string>(ImageTapped);
            SendCommand = new DelegateCommand(Send);
        }

        Action Send
        {
            get
            {
                return new Action(async () =>
                {
                    var content = new FormUrlEncodedContent(new[]
                                    {
                                        new KeyValuePair<string, string>("Nome", "Rodrigo"),
                                                new KeyValuePair<string, string>("Telefone", "21975519377"),
                                                new KeyValuePair<string, string>("Email","jbravo.br@gmail.com"),
                                                new KeyValuePair<string, string>("CodOferta",Imovel.CodImovel.ToString())
                                    });
                    var result = await App.AppBaseHttpClient.PostAsync($"{EnumAPIPath.imoveis}/{EnumAPIPath.contato}", content);
                    if (result.IsSuccessStatusCode)
                        _dialogService.ShowToast(EnumToastType.Success,
                                                 "Mensagem enviada com sucesso");
                    else
                        _dialogService.ShowToast(EnumToastType.Error,
                                                 "Erro ao enviar mensagem, por favor tente novamente",
                                                 8000);
                });
            }
        }

        Action<string> ImageTapped
        {
            get
            {
                return new Action<string>(async (source) =>
                {
                    await NavigateTo("ImageGalleryPreviewPage", source);
                });
            }
        }

        async Task NavigateTo(string page, string source)
        {
            var navparameters = new NavigationParameters
            {
                {"ImovelId", Imovel.CodImovel},
                {"ImageSource", source}
            };
            await _navigationService.NavigateAsync(page, navparameters, true);
        }

        public async void OnNavigatedFrom(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("imovelId"))
                ImovelId = (int)parameters["imovelId"];

            if (ImovelId > 0)
                await SetData();
            else
                _dialogService.ShowToast(EnumToastType.Error, Constants.ErrorFetchAPIData, 8000);
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("imovelId"))
                ImovelId = (int)parameters["imovelId"];

            if (ImovelId > 0)
                await SetData();
            else
                _dialogService.ShowToast(EnumToastType.Error, Constants.ErrorFetchAPIData, 8000);
        }

        async Task SetData()
        {
            _dialogService.ShowLoading("Carregando detalhes");

            var data = await _rootObjectService.Get(EnumAPIPath.details, ImovelId);

            if (data != null)
            {
                var imoveis = await _imovelService.GetAll();
                if (imoveis != null)
                {
                    Imovel = imoveis.FirstOrDefault(x => x.CodImovel == ImovelId);
                    if (Imovel != null)
                        ConfigureImovelForPresentation();
                }
            }
            else
                IsDataPresented = false;

            _dialogService.HideLoading();
        }

        void ConfigureImovelForPresentation()
        {
            if (!string.IsNullOrEmpty(Imovel.FotosPath))
            {
                Fotos = new List<string>();
                Imovel.FotosPath
                      .Replace("[", "")
                      .Replace("]", "")
                      .Replace("\"", "")
                      .Split(',')
                      .ToList()
                      .ForEach((nome) => Fotos.Add(nome));

                BackgroundImage = Imovel.Fotos[0];
            }

            if (!string.IsNullOrEmpty(Imovel.CaracteristicasPath))
            {
                Caracteristicas = new List<string>();
                Imovel.CaracteristicasPath
                      .Replace("[", "")
                      .Replace("]", "")
                      .Replace("\"", "")
                      .Split(',')
                      .ToList()
                      .ForEach((nome) => Caracteristicas.Add(nome));
            }

            if (!string.IsNullOrEmpty(Imovel.CaracteristicasComumPath))
            {
                CaracteristicasComum = new List<string>();
                Imovel.CaracteristicasComumPath
                      .Replace("[", "")
                      .Replace("]", "")
                      .Replace("\"", "")
                      .Split(',')
                      .ToList()
                      .ForEach((nome) => CaracteristicasComum.Add(nome));
            }

        }
    }
}