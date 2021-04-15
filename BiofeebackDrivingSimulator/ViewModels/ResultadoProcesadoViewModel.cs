using BiofeebackDrivingSimulator.Datos;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BiofeebackDrivingSimulator.ViewModels
{
    public class ResultadoProcesadoViewModel
    {
        #region Variables Locales

        #endregion

        #region Propiedades
        public Sesion Sesion { get; set; }
        public int[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public SeriesCollection EegAtencionMeditacionLinelSc { get; set; }
        public SeriesCollection EegAtencionMeditacionBarraSc { get; set; }
        #endregion

        #region Contructor
        public ResultadoProcesadoViewModel()
        {
            YFormatter = value => value.ToString("C");
        }
        #endregion

        #region Metodos
        public void Init(Sesion sesion)
        {
            try
            {
                this.Sesion = sesion;

                //Eeg Atencion y Meditacion Lineas
                this.EegAtencionMeditacionLinelSc = new SeriesCollection();

                var lineSeriesAtencion = new LineSeries
                {
                    Title = "Atención",
                    Values = new ChartValues<int>(),
                    PointGeometry = null,
                    Stroke = System.Windows.Media.Brushes.Red,
                };

                var lineSeriesMeditacion = new LineSeries
                {
                    Title = "Meditacion",
                    Values = new ChartValues<int>(),
                    PointGeometry = null,
                    Stroke = System.Windows.Media.Brushes.White,
                };

                List<int> datosAtencion = new List<int>();
                List<int> datosMeditacion = new List<int>();

                foreach (var item in this.Sesion.Eegs)
                {
                    lineSeriesAtencion.Values.Add(item.Attention);
                    lineSeriesMeditacion.Values.Add(item.Meditation);
                    datosAtencion.Add(item.Attention);
                    datosMeditacion.Add(item.Meditation);
                }

                EegAtencionMeditacionLinelSc.Add(lineSeriesAtencion);
                EegAtencionMeditacionLinelSc.Add(lineSeriesMeditacion);

                //Eeg Atencion y Meditacion Barras
                this.EegAtencionMeditacionBarraSc = new SeriesCollection();

                int atencionAvg = (int)datosAtencion.Average();
                int meditacionAvg = (int)datosMeditacion.Average();

                var lineSeriesAtencionB = new ColumnSeries
                {
                    Title = "Atención",
                    Values = new ChartValues<int> { atencionAvg },
                    Stroke = System.Windows.Media.Brushes.Red,
                    Fill = System.Windows.Media.Brushes.Red
                };

                var lineSeriesMeditacionB = new ColumnSeries
                {
                    Title = "Meditación",
                    Values = new ChartValues<int> { meditacionAvg },
                    Stroke = System.Windows.Media.Brushes.White,
                    Fill = System.Windows.Media.Brushes.White
                };

                EegAtencionMeditacionBarraSc.Add(lineSeriesAtencionB);
                EegAtencionMeditacionBarraSc.Add(lineSeriesMeditacionB);
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
        #endregion
    }
}
