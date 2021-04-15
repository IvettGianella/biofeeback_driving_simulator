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
    public class EstandarizacionEegViewModel
    {
        #region Propiedades
        public Sesion Sesion { get; set; }
        public List<Sesion> Sesiones { get; set; }

        public string[] LabelsTimeAplha { get; set; }
        public string[] LabelsTimeBeta { get; set; }
        public SeriesCollection EegAlphaSc { get; set; }
        public SeriesCollection EegBetaSc { get; set; }
        public Func<double, string> YFormatter { get; set; }
        #endregion

        #region Constructor
        public EstandarizacionEegViewModel()
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

                this.Sesiones = new List<Sesion>();

                using (var entidades = new Entidades())
                {
                    var usuarios = entidades.Usuarios
                                           .Where(u => u.EsMuestra)
                                           .Include(s => s.Sesiones).ToList();

                    foreach (var usuario in usuarios)
                    {
                        foreach (var item in usuario.Sesiones)
                        {
                            var sesionTemp = entidades.Sesiones
                                                  .Where(s => s.Id == item.Id && s.Comentarios == "Conducción")
                                                  .Include(e => e.Eegs)
                                                  .FirstOrDefault();

                            if (sesionTemp != null)
                            {
                                this.Sesiones.Add(sesionTemp);
                            }
                        }
                    }
                }

                List<double> mediasAlpha = new List<double>();
                List<double> mediasBetha = new List<double>();

                foreach (var seion1 in Sesiones)
                {
                    var avgAlpha = seion1.Eegs.Select(a => a.LowAlpha).Average();
                    mediasAlpha.Add(avgAlpha);
                    var avgBeta = seion1.Eegs.Select(a => a.LowBeta).Average();
                    mediasBetha.Add(avgBeta);
                }

                var mediaMediaAplha = Math.Round((double)mediasAlpha.Average(), 2);
                var mediaMediaBetha = Math.Round((double)mediasBetha.Average(), 2);

                //Alpha
                this.EegAlphaSc = new SeriesCollection();

                var lineSeriesAlpha = new LineSeries
                {
                    Title = "Alpha",
                    Values = new ChartValues<double>(),
                    PointGeometry = null,
                    Stroke = System.Windows.Media.Brushes.Green,
                };

                LabelsTimeAplha = new string[this.Sesion.Eegs.Count];

                int countA = 0;

                List<Double> alpha = new List<double>();

                foreach (var item in this.Sesion.Eegs)
                {
                    double valAlpha = Math.Round(((item.LowAlpha * 50) / mediaMediaAplha), 2);
                    lineSeriesAlpha.Values.Add(valAlpha);
                    alpha.Add(valAlpha);
                    LabelsTimeAplha[countA] = item.FechaHora.TimeOfDay.ToString(@"hh\:mm\:ss");
                    countA++;
                }

                EegAlphaSc.Add(lineSeriesAlpha);

                ////Betha
                this.EegBetaSc = new SeriesCollection();

                var lineSeriesBeta = new LineSeries
                {
                    Title = "Beta ",
                    Values = new ChartValues<double>(),
                    PointGeometry = null,
                    Stroke = System.Windows.Media.Brushes.Red,
                };

                LabelsTimeBeta = new string[this.Sesion.Eegs.Count];

                int countB = 0;

                List<Double> beta = new List<double>();

                foreach (var item in this.Sesion.Eegs)
                {
                    double valBeta = Math.Round(((item.LowBeta * 50) / mediaMediaBetha), 2);
                    lineSeriesBeta.Values.Add(valBeta);
                    beta.Add(valBeta);
                    LabelsTimeBeta[countB] = item.FechaHora.TimeOfDay.ToString(@"hh\:mm\:ss");
                    countB++;
                }

                var avgAlpha2 = Math.Round((double)alpha.Average(), 4);
                var avgBeta2 = Math.Round((double)beta.Average(), 4);
                EegBetaSc.Add(lineSeriesBeta);
            }
            catch (System.Exception ex)
            {
                string message = ex.Message;
            }
        }


        #endregion
    }
}
