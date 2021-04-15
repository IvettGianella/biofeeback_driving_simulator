using BiofeebackDrivingSimulator.Datos;
using BiofeebackDrivingSimulator.ViewModels.Base;
using System.Collections.Generic;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Linq;
using System.Data.Entity;
using System.Windows;
using System.Windows.Input;
using BiofeebackDrivingSimulator.Views;

namespace BiofeebackDrivingSimulator.ViewModels
{
    public class ResultadoViewModel : BaseViewModel
    {
        #region Variables Locales
        private List<FrecuenciaCardiaca> frecuenciaCardiacas;
        private string fecha;
        private string comentario;
        #endregion

        #region Propiedades
        public List<FrecuenciaCardiaca> FrecuenciaCardiacas
        {
            get { return frecuenciaCardiacas; }
            set { SetProperty(ref this.frecuenciaCardiacas, value); }
        }
        public string Fecha 
        {
            get { return this.fecha; }
            set { SetProperty(ref this.fecha, value); } 
        }
        public string Comentario
        {
            get { return this.comentario; }
            set { SetProperty(ref this.comentario, value); }
        }

        public Sesion Sesion { get; set; }
        public int[] Labels { get; set; }
        public string[] LabelsTimeHR { get; set; }
        public string[] LabelsTimeTemp { get; set; }
        public string[] LabelsTimeAplha { get; set; }
        public string[] LabelsTimeBeta { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public SeriesCollection FrecuenciaCardiacasSc { get; set; }
        public SeriesCollection TemperaturasSc { get; set; }
        public SeriesCollection EegAlphaSc { get; set; }
        public SeriesCollection EegBetaSc { get; set; }
        #endregion

        #region Commands
        public ICommand CommandVerMas => new DelegateCommand(ResultadoProcesado, null);
        public ICommand CommandEstadarizacionEeg => new DelegateCommand(EstadarizacionEeg, null);
        #endregion

        #region Constructor
        public ResultadoViewModel()
        {
            FrecuenciaCardiacas = new List<FrecuenciaCardiaca>();
            YFormatter = value => value.ToString("C");
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Método de inicialización de la clase en la cual se 
        /// trae de la base de datos el registro de los sensores
        /// y se crean SeriesCollection para el dibujado de los graficos 
        /// </summary>
        /// <param name="idSesion"></param>
        public void Init(int idSesion)
        {
            try
            {
                Labels = new[] { 0, 50, 100 };

                using (var entidades = new Entidades())
                {
                    this.Sesion = entidades.Sesiones
                                           .Where(s => s.Id == idSesion)
                                           .Include(s => s.FrecuenciaCardiacas)
                                           .Include(s => s.Eegs)
                                           .Include(s => s.Temperaturas)
                                           .FirstOrDefault();
                }

                this.Fecha = String.Format("{0:dd/MM/yyyy - hh:mm t}", this.Sesion.Fecha);
                this.Comentario = this.Sesion.Comentarios;

                //Frecuencia Cardiaca
                FrecuenciaCardiacasSc = new SeriesCollection();

                LabelsTimeHR = new string[this.Sesion.FrecuenciaCardiacas.Count];

                int countH = 0;

                var lineSeriesHr = new LineSeries
                {
                    Title = "BPM",
                    Values = new ChartValues<int>(),
                    Stroke = System.Windows.Media.Brushes.Yellow,
                    PointGeometry = null,
                };

                var avgHr = new List<int>();

                foreach (var item in this.Sesion.FrecuenciaCardiacas)
                {
                    lineSeriesHr.Values.Add((int)item.Valor);
                    if (item.Valor > 60) 
                    {
                        avgHr.Add((int)item.Valor);
                    }
                    LabelsTimeHR[countH] = item.FechaHora.TimeOfDay.ToString(@"hh\:mm\:ss");
                    countH++;
                }

                FrecuenciaCardiacasSc.Add(lineSeriesHr);

                //Temperaturas
                this.TemperaturasSc = new SeriesCollection();

                LabelsTimeTemp = new string[this.Sesion.Temperaturas.Count];

                int countT = 0;

                var lineSeriesTemp = new LineSeries
                {
                    Title = "Centigrados",
                    Values = new ChartValues<double>(),
                    PointGeometry = null,
                };

                var avgTemp = new List<double>();

                foreach (var item in this.Sesion.Temperaturas)
                {
                    lineSeriesTemp.Values.Add(Math.Round((double)item.Valor, 2));
                    avgTemp.Add((double)item.Valor);
                    LabelsTimeTemp[countT] = item.FechaHora.TimeOfDay.ToString(@"hh\:mm\:ss");
                    countT++;
                }

                TemperaturasSc.Add(lineSeriesTemp);

                var mediaHr = Math.Round((double)avgHr.Average(), 2);
                var mediaTemp = Math.Round((double)avgTemp.Average(), 2);

                //Eeg ondas Alpha
                this.EegAlphaSc = new SeriesCollection();

                var lineSeriesAlpha = new LineSeries
                {
                    Title = "Alpha",
                    Values = new ChartValues<float>(),
                    PointGeometry = null,
                    Stroke = System.Windows.Media.Brushes.Green,
                };

                var maxAlpha = this.Sesion.Eegs.Max(s => s.LowAlpha);
                //var maxDelta = this.Sesion.Eegs.Max(s => s.Delta);

                List<decimal> datosAlpha = new List<decimal>();

                LabelsTimeAplha = new string[this.Sesion.Eegs.Count];

                int countA = 0;

                foreach (var item in this.Sesion.Eegs)
                {
                    float valAlpha = ((item.LowAlpha * 100) / maxAlpha);
                    lineSeriesAlpha.Values.Add(valAlpha);
                    datosAlpha.Add((decimal)valAlpha);
                    LabelsTimeAplha[countA] = item.FechaHora.TimeOfDay.ToString(@"hh\:mm\:ss");
                    countA++;
                }

                decimal resultA = datosAlpha.Average();

                EegAlphaSc.Add(lineSeriesAlpha);

                //Eeg ondas Beta
                this.EegBetaSc = new SeriesCollection();

                var lineSeriesBeta = new LineSeries
                {
                    Title = "Beta ",
                    Values = new ChartValues<float>(),
                    PointGeometry = null,
                    Stroke = System.Windows.Media.Brushes.Red
                };

                var maxBeta = this.Sesion.Eegs.Max(s => s.LowBeta);

                List<decimal> datosBeta = new List<decimal>();

                LabelsTimeBeta = new string[this.Sesion.Eegs.Count];

                int countB = 0;

                foreach (var item in this.Sesion.Eegs)
                {
                    float valBeta = ((item.LowBeta * 100) / maxBeta);
                    lineSeriesBeta.Values.Add(valBeta);
                    datosBeta.Add((decimal)valBeta);
                    LabelsTimeBeta[countB] = item.FechaHora.TimeOfDay.ToString(@"hh\:mm\:ss");
                    countB++;
                }

                decimal resultB = datosBeta.Average();

                EegBetaSc.Add(lineSeriesBeta);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                MessageBox.Show(
                    "Ocurrió un problema al leer los datos registrados o no hay datos registrados",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void ResultadoProcesado(Object obj)
        {
            try
            {
                MainViewModel.GetInstance().ResultadoProcesadoVm.Init(this.Sesion);
                ResultadoProcesadoView resultadoProcesadoView = new ResultadoProcesadoView();
                resultadoProcesadoView.Show();
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        }

        public void EstadarizacionEeg(Object obj) 
        {
            try
            {
                MainViewModel.GetInstance().EstandarizacionEegVm.Init(this.Sesion);
                EstadarizacionEegView estadarizacionEegView = new EstadarizacionEegView();
                estadarizacionEegView.Show();
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        }

        #endregion
    }
}
