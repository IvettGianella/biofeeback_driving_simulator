using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiofeebackDrivingSimulator.ViewModels
{
    public class MainViewModel
    {
        private static MainViewModel instance;

        #region Constructor
        private MainViewModel(){ }
        #endregion

        #region Singleton
        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MainViewModel();
            }

            return instance;
        }
        #endregion

        #region ViewModels Properties
        public UsuariosViewModel UsuariosVm { get; set; }
        public RegistroUsuarioViewModel RegistroUsuarioVm { get; set; }
        public RegistroSimulacionViewModel RegistroSimulacionVm { get; set; }
        public PuertosViewModel PuertosVm { get; set; }
        public ResultadoViewModel ResultadoVm { get; set; }
        public ResultadoProcesadoViewModel ResultadoProcesadoVm { get; set; }
        public ComparacionViewModel ComparacionVm { get; set; }
        public EstandarizacionEegViewModel EstandarizacionEegVm { get; set; }
        #endregion
    }
}
