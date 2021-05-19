using BiofeebackDrivingSimulator.Datos;
using BiofeebackDrivingSimulator.ViewModels.Base;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace BiofeebackDrivingSimulator.ViewModels
{
    public class ComparacionViewModel : BaseViewModel
    {
        #region Propiedades
        public Sesion SesionRelajacion { get; set; }
        public Sesion SesionConduccion { get; set; }
        public int[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public SeriesCollection FrecuenciaCardiacasSc { get; set; }
        public SeriesCollection TemperaturasSc { get; set; }
        public SeriesCollection EegAtencionSc { get; set; }
        #endregion

        #region Constructor
        public ComparacionViewModel()
        {

        }
        #endregion

        #region Metodos
        public void Init(int idSesion1, int idSession2)
        {
            try
            {
                using (var entidades = new Entidades())
                {
                    var relajacion = entidades.Sesiones
                                                    .Where(s => s.Id == idSesion1)
                                                    .Include(s => s.FrecuenciaCardiacas)
                                                    .Include(s => s.Eegs)
                                                    .Include(s => s.Temperaturas)
                                                    .FirstOrDefault();

                    var atencion = entidades.Sesiones
                                                    .Where(s => s.Id == idSession2)
                                                    .Include(s => s.FrecuenciaCardiacas)
                                                    .Include(s => s.Eegs)
                                                    .Include(s => s.Temperaturas)
                                                    .FirstOrDefault();

                    if (relajacion.Comentarios == "Relajación")
                    {
                        this.SesionRelajacion = relajacion;
                        this.SesionConduccion = atencion;
                    }
                    else
                    {
                        this.SesionRelajacion = atencion;
                        this.SesionConduccion = relajacion;
                    }
                }

                //Frecuencia Cardiaca
                this.FrecuenciaCardiacasSc = new SeriesCollection();

                var frecuenciasCardiacasR = new List<int>();

                foreach (var item in this.SesionRelajacion.FrecuenciaCardiacas)
                {
                    if (item.Valor > 60) 
                    {
                        frecuenciasCardiacasR.Add((int)item.Valor);
                    }
                }

                var lineSeriesHRRelajacion = new ColumnSeries
                {
                    Title = "Relajación",
                    Values = new ChartValues<int> { (int)frecuenciasCardiacasR.Average() },
                    Stroke = System.Windows.Media.Brushes.White,
                    Fill = System.Windows.Media.Brushes.White,
                };

                var frecuenciasCardiacasC = new List<int>();

                foreach (var item in this.SesionConduccion.FrecuenciaCardiacas)
                {
                    if (item.Valor > 60)
                    {
                        frecuenciasCardiacasC.Add((int)item.Valor);
                    }
                }

                var lineSeriesHRConduccion = new ColumnSeries
                {
                    Title = "Conducción",
                    Values = new ChartValues<int> { (int)frecuenciasCardiacasC.Average() },
                    Stroke = System.Windows.Media.Brushes.Red,
                    Fill = System.Windows.Media.Brushes.Red,
                };

                FrecuenciaCardiacasSc.Add(lineSeriesHRRelajacion);
                FrecuenciaCardiacasSc.Add(lineSeriesHRConduccion);

                //Temperatura
                this.TemperaturasSc = new SeriesCollection();

                var temperaturasR = new List<double>();

                foreach (var item in this.SesionRelajacion.Temperaturas)
                {
                    temperaturasR.Add((double)item.Valor);
                }

                var lineSeriesTempRelajacion = new ColumnSeries
                {
                    Title = "Relajación",
                    Values = new ChartValues<double> { (double)temperaturasR.Average() },
                    Stroke = System.Windows.Media.Brushes.White,
                    Fill = System.Windows.Media.Brushes.White,
                };

                var temperaturasC = new List<double>();

                foreach (var item in this.SesionConduccion.Temperaturas)
                {
                    temperaturasC.Add((double)item.Valor);
                }

                var lineSeriesTempConduccion = new ColumnSeries
                {
                    Title = "Conducción",
                    Values = new ChartValues<double> { (double)temperaturasC.Average() },
                    Stroke = System.Windows.Media.Brushes.Red,
                    Fill = System.Windows.Media.Brushes.Red,
                };

                TemperaturasSc.Add(lineSeriesTempRelajacion);
                TemperaturasSc.Add(lineSeriesTempConduccion);

                //Atencion EEG
                this.EegAtencionSc = new SeriesCollection();

                var atencionR = new List<int>();

                foreach (var item in this.SesionRelajacion.Eegs)
                {
                    atencionR.Add((int)item.Attention);
                }

                var lineSeriesEegRelajacion = new ColumnSeries
                {
                    Title = "Relajación",
                    Values = new ChartValues<int> { (int)atencionR.Average() },
                    Stroke = System.Windows.Media.Brushes.White,
                    Fill = System.Windows.Media.Brushes.White,
                };

                var atencionC = new List<int>();

                foreach (var item in this.SesionConduccion.Eegs)
                {
                    atencionC.Add((int)item.Attention);
                }

                var lineSeriesEegConduccion = new ColumnSeries
                {
                    Title = "Conducción",
                    Values = new ChartValues<int> { (int)atencionC.Average() },
                    Stroke = System.Windows.Media.Brushes.Red,
                    Fill = System.Windows.Media.Brushes.Red,
                };

                EegAtencionSc.Add(lineSeriesEegRelajacion);
                EegAtencionSc.Add(lineSeriesEegConduccion);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                MessageBox.Show(
                    "Ocurrió un problema al leer los datos registrados o no hay datos registrados",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Debug.WriteLine(ex.Message);
                Logger.Log.Error("Mensaje: ", ex);
            }
        }
        #endregion
    }
}
