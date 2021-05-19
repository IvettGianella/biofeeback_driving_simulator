using BiofeebackDrivingSimulator.Interface;
using BiofeebackDrivingSimulator.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Windows;

namespace BiofeebackDrivingSimulator.ViewModels
{
    public class PuertosViewModel : BaseViewModel
    {
        #region Variables Locales
        private string modelPuertoUno;
        private string modelPuertoNano;
        private ObservableCollection<string> comPorts;
        private string selectedComPortUno;
        private string selectedComPortNano;
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

        public ObservableCollection<string> ComPorts
        {
            get { return this.comPorts; }
            set { SetProperty(ref this.comPorts, value); }
        }

        public string SelectedComPortUno
        {
            get { return this.selectedComPortUno; }
            set { SetProperty(ref this.selectedComPortUno, value); }
        }

        public string SelectedComPortNano
        {
            get { return this.selectedComPortNano; }
            set { SetProperty(ref this.selectedComPortNano, value); }
        }
        #endregion

        #region Constructor
        public PuertosViewModel()
        {
            this.GuardarCommand = new RelayCommand<ICloseable>(this.Guardar);
            this.CancelarCommand = new RelayCommand<ICloseable>(this.Cancelar);
        }
        #endregion

        #region Commands
        public RelayCommand<ICloseable> GuardarCommand { get; private set; }
        public RelayCommand<ICloseable> CancelarCommand { get; private set; }
        #endregion

        #region Metodos
        public void Init()
        {
            this.ComPorts = new ObservableCollection<string>();
            LlenarComPorts();

            this.SelectedComPortUno = ComPorts.FirstOrDefault(c => c == Properties.Settings.Default.ComPortUno);
            this.SelectedComPortNano = ComPorts.FirstOrDefault(c => c == Properties.Settings.Default.ComPortNano);
        }

        private void Guardar(ICloseable window)
        {
            try
            {
                if (SelectedComPortUno == null || SelectedComPortNano == null)
                {
                    MessageBox.Show(
                        "No puede dejar los puertos vacíos, verifique que tenga los dos Arduinos conectados.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                if (SelectedComPortUno.Equals(SelectedComPortNano)) 
                {
                    MessageBox.Show(
                        "No puede elegir el mismo puerto para ambos dispositivos.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                if (SelectedComPortUno.Equals(string.Empty) || SelectedComPortNano.Equals(string.Empty)) 
                {
                    MessageBox.Show(
                        "No puede dejar los puertos vacíos, verifique que tenga los dos Arduinos conectados.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                Properties.Settings.Default.ComPortUno = SelectedComPortUno;
                Properties.Settings.Default.ComPortNano = SelectedComPortNano;
                Properties.Settings.Default.Save();

                if (window != null)
                {
                    window.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void LlenarComPorts()
        {
            foreach (var item in SerialPort.GetPortNames())
            {
                this.comPorts.Add(item);
            }
        }

        private void Cancelar(ICloseable window) 
        {
            if (window != null)
            {
                window.Close();
            }
        }
        #endregion
    }
}
