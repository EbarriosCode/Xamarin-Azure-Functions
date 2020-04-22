using ServerlessApp.Models;
using ServerlessApp.Resources;
using ServerlessApp.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ServerlessApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Services
        private readonly AzureFunctionService azureFunctionService;
        #endregion Services

        #region Attributes
        private string nombre;
        private double pesoKg;
        private double alturaMts;
        private bool isVisible;
        private bool isRunning;
        private string result;
        private bool errorIsVisible;
        private string error;
        #endregion Attributes

        #region Properties
        public string Nombre
        {
            get { return this.nombre; }
            set { SetValue(ref this.nombre, value); }
        }

        public double PesoKg
        {
            get { return this.pesoKg; }
            set { SetValue(ref this.pesoKg, value); }
        }

        public double AlturaMts
        {
            get { return this.alturaMts; }
            set { SetValue(ref this.alturaMts, value); }
        }

        public bool IsVisible
        {
            get { return this.isVisible; }
            set { SetValue(ref this.isVisible, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public string Result
        {
            get { return this.result; }
            set { SetValue(ref this.result, value); }
        }

        public bool ErrorIsVisible
        {
            get { return this.errorIsVisible; }
            set { SetValue(ref this.errorIsVisible, value); }
        }

        public string Error
        {
            get { return this.error; }
            set { SetValue(ref this.error, value); }
        }
        #endregion Properties

        #region Constructor
        public MainViewModel() => this.azureFunctionService = new AzureFunctionService();
        #endregion Constructor

        #region Commands
        public ICommand PesoIdealCommand
        {
            get { return new Command(async () => await PesoIdealMethod()); }
        }
        #endregion Commands        

        #region Methods
        private async Task PesoIdealMethod()
        {
            var current = Connectivity.NetworkAccess;
            
            // Si no hay Acceso a Internet
            if(current == NetworkAccess.None)
            {
                await Application.Current.MainPage.DisplayAlert("ServerlessApp", "Comprueba su conexión a Internet", "Aceptar");
                return; 
            }

            if (string.IsNullOrEmpty(this.Nombre))
            {
                this.ErrorIsVisible = true;
                this.Error = $"El campo {nameof(this.Nombre)} es obligatorio";
                return;
            }

            if(this.PesoKg <= 0)
            {
                this.ErrorIsVisible = true;
                this.Error = $"El campo {nameof(this.PesoKg)} es obligatorio";
                return;
            }

            if (this.AlturaMts <= 0)
            {
                this.ErrorIsVisible = true;
                this.Error = $"El campo {nameof(AlturaMts)} es obligatorio";
                return;
            }

            var sendParams = new PesoIdealParams() 
            { 
                Nombre = this.Nombre,
                PesoKg = this.PesoKg,
                AlturaMts = this.AlturaMts
            };

            this.ErrorIsVisible = false;
            this.Error = string.Empty;
            this.IsVisible = true;
            this.IsRunning = true;

            var pesoIdealRequest = await this.azureFunctionService.PesoIdealAzureFunctionAsync<PesoIdealResult>(Literals.UrlAzureFunction, sendParams);

            if(pesoIdealRequest != null)
            {                
                double pesoIdealResult = (pesoIdealRequest.DataResult as PesoIdealResult).Result;
                this.Result = $"Indice de Masa Corporal (IMC): {pesoIdealResult}";                
                this.IsRunning = false;

                if (pesoIdealResult >= 18.5 && pesoIdealResult <= 24.9)                
                    await Application.Current.MainPage.DisplayAlert("ServerlessApp",
                                                                    $"Hola {this.Nombre}: Usted tiene un peso saludable.",
                                                                    "Aceptar");                

                else if (pesoIdealResult < 18.5)                
                    await Application.Current.MainPage.DisplayAlert("ServerlessApp",
                                                                    $"Hola {this.Nombre}: Usted tiene un peso demasiado bajo.",
                                                                    "Aceptar");                

                else if (pesoIdealResult >= 25 && pesoIdealResult <= 30)                
                    await Application.Current.MainPage.DisplayAlert("ServerlessApp",
                                                                    $"Hola {this.Nombre}: Usted tiene sobrepeso.",
                                                                    "Aceptar");                

                else if (pesoIdealResult > 30)                
                    await Application.Current.MainPage.DisplayAlert("ServerlessApp",
                                                                    $"Hola {this.Nombre}: Usted tiene obesidad :( ",
                                                                    "Aceptar");                
            }
        }
        #endregion Methods
    }
}