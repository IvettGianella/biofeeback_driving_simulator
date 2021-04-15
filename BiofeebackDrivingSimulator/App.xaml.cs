using BiofeebackDrivingSimulator.ViewModels;
using BiofeebackDrivingSimulator.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BiofeebackDrivingSimulator
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            MainViewModel.GetInstance().UsuariosVm = new UsuariosViewModel();
            MainViewModel.GetInstance().RegistroUsuarioVm = new RegistroUsuarioViewModel();
            MainViewModel.GetInstance().RegistroSimulacionVm = new RegistroSimulacionViewModel();
            MainViewModel.GetInstance().ResultadoVm = new ResultadoViewModel();
            MainViewModel.GetInstance().ResultadoProcesadoVm = new ResultadoProcesadoViewModel();
            MainViewModel.GetInstance().PuertosVm = new PuertosViewModel();
            MainViewModel.GetInstance().ComparacionVm = new ComparacionViewModel();
            MainViewModel.GetInstance().EstandarizacionEegVm = new EstandarizacionEegViewModel();
            //MainWindow = new UsuariosView();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            base.OnStartup(e);
        }
    }
}
