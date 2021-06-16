using BiofeebackDrivingSimulator.Datos;
using BiofeebackDrivingSimulator.Interface;
using BiofeebackDrivingSimulator.Services;
using BiofeebackDrivingSimulator.ViewModels.Base;
using BiofeebackDrivingSimulator.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BiofeebackDrivingSimulator.ViewModels
{
    public class RegistroSimulacionViewModel : BaseViewModel
    {
        #region Variables Locales
        //Variable SerialPort
        private SerialPort serialPort1;
        private SerialPort serialPort2;
        private string comentarios;
        private string heartRate = "";
        private string temperatura = "";
        private string eeg = "";
        private string senal = "";
        private string concentracion = "0";
        private string meditacion = "0";
        private string alpha = "0";
        private int maxAlpha = 0;
        private string beta = "0";
        private int maxBeta = 0;
        private string delta = "0";
        private int maxDelta = 0;
        private string oldHeartRate = "0";
        private string registrarTerminar = "";
        private float oldHr = 0;
        private int altoAncho = 25;
        //private int contador = 0;
        #endregion

        #region Propiedades
        public string Comentarios
        {
            get { return this.comentarios; }
            set { SetProperty(ref this.comentarios, value); }
        }
        public string HeartRate
        {
            get { return this.heartRate; }
            set { SetProperty(ref this.heartRate, value); }
        }
        public string Temperatura
        {
            get { return this.temperatura; }
            set { SetProperty(ref this.temperatura, value); }
        }
        public string Eeg
        {
            get { return this.eeg; }
            set { SetProperty(ref this.eeg, value); }
        }
        public string Senal
        {
            get { return this.senal; }
            set { SetProperty(ref this.senal, value); }
        }
        public string Concentracion
        {
            get { return this.concentracion; }
            set { SetProperty(ref this.concentracion, value); }
        }
        public string Meditacion
        {
            get { return this.meditacion; }
            set { SetProperty(ref this.meditacion, value); }
        }
        public string Alpha
        {
            get { return this.alpha; }
            set { SetProperty(ref this.alpha, value); }
        }
        public string Beta
        {
            get { return this.beta; }
            set { SetProperty(ref this.beta, value); }
        }
        public string Delta
        {
            get { return this.delta; }
            set { SetProperty(ref this.delta, value); }
        }
        public int AltoAncho
        {
            get { return this.altoAncho; }
            set { SetProperty(ref this.altoAncho, value); }
        }
        public string RegistrarTerminar
        {
            get { return this.registrarTerminar; }
            set { SetProperty(ref this.registrarTerminar, value); }
        }
        public Sesion Sesion { get; set; }
        public bool Save { get; set; }
        public Eeg NewEgg { get; set; }
        public Temperatura NewTemperatura { get; set; }
        public FrecuenciaCardiaca NewHeartRate { get; set; }
        public List<FrecuenciaCardiaca> FrecuenciaCardiacas { get; set; }
        #endregion

        #region Constructor
        public RegistroSimulacionViewModel()
        {
            try
            {
                this.RegresarCommand = new RelayCommand<ICloseable>(this.Regresar);
                this.Save = false;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                MessageBox.Show(
                    "Ocurrió un problema cargar la pagina de registro de datos",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        #endregion

        #region Command
        public ICommand CommandRegistrar => new DelegateCommand(this.Registrar, null);
        public RelayCommand<ICloseable> RegresarCommand { get; private set; }
        #endregion

        #region Metodos
        /// <summary>
        /// Metodo para inicializar puertos de los sensores
        /// y demás componentes necesarios para el registro 
        /// de los datos enviados por los sensores
        /// </summary>
        /// <param name="sesion">Objeto de tipo sesión para 
        /// tener acceso al Id de dicha sesión</param>
        public void Init(Sesion sesion)
        {
            try
            {
                this.RegistrarTerminar = "Registrar";

                this.Sesion = sesion;
                this.NewEgg = new Eeg();
                this.NewTemperatura = new Temperatura();
                this.NewHeartRate = new FrecuenciaCardiaca();
                this.FrecuenciaCardiacas = new List<FrecuenciaCardiaca>();

                serialPort1 = new SerialPort();
                serialPort1.PortName = Properties.Settings.Default.ComPortUno;
                serialPort1.BaudRate = 9600;
                serialPort1.DtrEnable = true;
                serialPort1.Open();

                serialPort1.DataReceived += serialPort_DataRecived;

                serialPort2 = new SerialPort();
                serialPort2.PortName = Properties.Settings.Default.ComPortNano;
                serialPort2.BaudRate = 9600;
                serialPort2.DtrEnable = true;
                serialPort2.Open();

                serialPort2.DataReceived += serialPort2_DataRecived;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                MessageBox.Show(
                    "Ocurrió un problema al leer los datos de la arduino",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                RemoverSesion(this.Sesion.Id);

                Debug.WriteLine(ex.Message);
                Logger.Log.Error("Mensaje: ", ex);
            }
        }

        /// <summary>
        /// Método el cual se activa cuando se da clic en el botón Registrar/Terminar
        /// Si el botón esta en Registrar se cambia la propiedad save a true, si no
        /// se cambia la propiedad save a false, al terminar los datos se guardan
        /// en la base de datos
        /// </summary>
        /// <param name="obj"></param>
        public async void Registrar(Object obj)
        {
            try
            {
                if (!this.Save)
                {
                    if (string.IsNullOrEmpty(this.Comentarios))
                    {
                        MessageBox.Show(
                        "Debe llenar el espacio de comentarios",
                        "Atención",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                    }
                    else
                    {
                        this.Save = true;
                        this.RegistrarTerminar = "Terminar";
                    }
                }
                else
                {
                    var rpta = MessageBox.Show(
                                "¿Desea dejar de registrar los datos de la sesión?",
                                "Atención",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Exclamation);

                    if (rpta.ToString().Equals("Yes"))
                    {
                        this.Save = false;
                        this.RegistrarTerminar = "Registrar";

                        using (var context = new Entidades())
                        {
                            SesionServices sesionServices = new SesionServices(context);
                            sesionServices.AgregarComentarioSesion(this.Sesion.Id, this.comentarios);

                            FrecuenciaCardiacaService frecuenciaCardiacaService = new FrecuenciaCardiacaService(context);
                            await frecuenciaCardiacaService.AgregarFrecuenciaCardiaca(this.Sesion.Id, this.FrecuenciaCardiacas);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log.Error("Mensaje: ", ex);
            }
        }

        /// <summary>
        /// Métodos para regresar a la vista de Usuarios
        /// Si ya se dio clic en este botón anteriormente
        /// no se permite que se ejecute la acción dos veces
        /// </summary>
        /// <param name="window"></param>
        public void Regresar(ICloseable window)
        {
            MainViewModel.GetInstance().UsuariosVm.Init();
            if (!Application.Current.Windows.OfType<UsuariosView>().Any())
            {
                UsuariosView usuariosView = new UsuariosView();
                usuariosView.Show();
            }

            if (window != null)
            {
                window.Close();
            }
        }

        /// <summary>
        /// Método que esta escuchando los datos que envian los sensores
        /// de temperatura y de electroencefalograma
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void serialPort_DataRecived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string read1 = serialPort1.ReadLine();
                if (read1.Contains("T,"))
                {
                    //Temperatura = read1;
                    String source = read1;
                    String[] result = source.Split(new char[] { ',', ';' });

                    this.Temperatura = result[1].Replace("\r\n", "") + "°";
                    this.NewTemperatura.FechaHora = System.DateTime.Now;
                    var tem = result[1].Trim();
                    this.NewTemperatura.Valor = float.Parse(tem, CultureInfo.InvariantCulture);

                    if (this.Save)
                    {
                        using (var contexto = new Entidades())
                        {
                            TemperaturaService temperaturaService = new TemperaturaService(contexto);
                            await temperaturaService.AgregarTemperatura(this.Sesion.Id, this.NewTemperatura);
                        }
                    }
                }
                else
                {
                    Eeg = read1;

                    String source = Eeg;
                    String[] result = source.Split(new char[] { ',', ';' });

                    this.NewEgg.FechaHora = System.DateTime.Now;
                    this.NewEgg.Senal = Int32.Parse(result[1]);
                    this.NewEgg.Attention = Int32.Parse(result[2]);
                    this.NewEgg.Meditation = Int32.Parse(result[3]);
                    this.NewEgg.Delta = Int32.Parse(result[4]);
                    this.NewEgg.Theta = Int32.Parse(result[5]);
                    this.NewEgg.LowAlpha = Int32.Parse(result[6]);
                    this.NewEgg.HighAlpha = Int32.Parse(result[7]);
                    this.NewEgg.LowBeta = Int32.Parse(result[8]);
                    this.NewEgg.HighBeta = Int32.Parse(result[9]);
                    this.NewEgg.LowGamma = Int32.Parse(result[10]);
                    this.NewEgg.HighGamma = Int32.Parse(result[11]);

                    this.Concentracion = this.NewEgg.Attention.ToString();
                    this.Meditacion = this.NewEgg.Meditation.ToString();

                    if (maxAlpha < this.NewEgg.LowAlpha || maxAlpha == 0)
                    {
                        maxAlpha = this.NewEgg.LowAlpha;
                    }

                    if (maxBeta < this.NewEgg.LowBeta || maxBeta == 0)
                    {
                        maxBeta = this.NewEgg.LowBeta;
                    }

                    if (maxDelta < this.NewEgg.Delta || maxDelta == 0)
                    {
                        maxDelta = this.NewEgg.Delta;
                    }

                    if (this.NewEgg.Senal == 200)
                    {
                        this.Senal = "Red";
                        this.Concentracion = "0";
                        this.Meditacion = "0";
                        this.Alpha = "0";
                        this.Beta = "0";
                        this.Delta = "0";
                    }

                    else if (this.NewEgg.Senal == 0)
                    {
                        this.Senal = "YellowGreen";
                        this.Concentracion = this.NewEgg.Attention.ToString();
                        this.Meditacion = this.NewEgg.Meditation.ToString();
                    }
                    else if (this.NewEgg.Senal > 0 && this.NewEgg.Senal < 200)
                    {
                        this.Senal = "Yellow";
                        this.Concentracion = "0";
                        this.Meditacion = "0";
                        this.Alpha = ((this.NewEgg.LowAlpha * 100) / maxAlpha).ToString();
                        this.Beta = ((this.NewEgg.LowBeta * 100) / maxBeta).ToString();
                        this.Delta = ((this.NewEgg.Delta * 100) / maxDelta).ToString();
                    }


                    if (this.Save)
                    {
                        using (var contexto = new Entidades())
                        {
                            EegService eegService = new EegService(contexto);
                            await eegService.AgregarEeg(this.Sesion.Id, this.NewEgg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log.Error("Mensaje: ", ex);
            }
        }

        /// <summary>
        /// Método que esta escuchando los datos que envian
        /// el sensor de ritmo cardiaco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serialPort2_DataRecived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //Separa la cadena por comas ","
                string heartRate = serialPort2.ReadLine();

                String source = heartRate;
                String[] result = source.Split(new char[] { ',', ';' });

                this.NewHeartRate.Valor = Int32.Parse(result[2]);
                this.NewHeartRate.FechaHora = System.DateTime.Now;

                if (result.Length == 4)
                {
                    //Mensaje para pedir que coloque el dedo en el sensor
                    if (result[3].ToString().Contains("NoFinger"))
                    {
                        this.AltoAncho = 25;
                        this.HeartRate = "Por favor coloque el dedo en el sensor";
                    }
                }
                else
                {
                    if (this.NewHeartRate.Valor < 60)
                    {
                        //Evitar mostrar dato error
                        this.HeartRate = "60";
                    }
                    else
                    {
                        this.HeartRate = this.NewHeartRate.Valor.ToString();
                    }

                    if (!result[1].Equals(this.oldHeartRate.ToString()))
                    {
                        if (this.AltoAncho == 25)
                        {
                            this.AltoAncho = 35;
                        }
                        else
                        {
                            this.AltoAncho = 25;
                        }
                    }

                    if (this.Save)
                    {
                        if (oldHr != this.NewHeartRate.Valor)
                        {
                            this.FrecuenciaCardiacas.Add(new FrecuenciaCardiaca
                            {
                                FechaHora = this.NewHeartRate.FechaHora,
                                Valor = this.NewHeartRate.Valor
                            });
                            oldHr = this.NewHeartRate.Valor;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log.Error("Mensaje: ", ex);
            }
        }

        private void RemoverSesion(int idSesion) 
        {
            try
            {
                using (var contexto = new Entidades())
                {
                    SesionServices sesion = new SesionServices(contexto);
                    sesion.BorrarSesion(idSesion);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log.Error("Mensaje: ", ex);
            }
        }
        #endregion
    }
}
