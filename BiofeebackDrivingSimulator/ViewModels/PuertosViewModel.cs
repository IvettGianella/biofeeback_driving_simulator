using BiofeebackDrivingSimulator.Interface;
using BiofeebackDrivingSimulator.ViewModels.Base;
using GalaSoft.MvvmLight.Command;

namespace BiofeebackDrivingSimulator.ViewModels
{
    public class PuertosViewModel : BaseViewModel
    {
        #region Variables Locales
        private string modelPuertoUno;
        private string modelPuertoNano;
        #endregion

        #region Propieades
        public string ModelPuertoUno
        {
            get { return this.modelPuertoUno; }
            set { SetProperty(ref this.modelPuertoUno, value); }
        }
        public string ModelPuertoNano
        {
            get { return this.modelPuertoNano; }
            set { SetProperty(ref this.modelPuertoNano, value); }
        }
        #endregion

        #region Constructor
        public PuertosViewModel()
        {
            this.GuardarCommand = new RelayCommand<ICloseable>(this.Guardar);
        }
        #endregion

        #region Commands
        public RelayCommand<ICloseable> GuardarCommand { get; private set; }
        #endregion

        #region Metodos
        public void Init()
        {

        }

        public void Guardar(ICloseable window)
        {

        }
        #endregion
    }
}
