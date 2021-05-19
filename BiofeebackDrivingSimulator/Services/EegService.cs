using BiofeebackDrivingSimulator.Datos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace BiofeebackDrivingSimulator.Services
{
    public class EegService
    {
        private readonly Entidades _context;

        public EegService(Entidades context)
        {
            _context = context;
        }

        public async Task<Sesion> AgregarEeg(int id, Eeg eeg)
        {
            try
            {
                var sesion = _context.Sesiones
                                    .Where(s => s.Id == id)
                                    .FirstOrDefault();
                sesion.Eegs = new List<Eeg>();
                sesion.Eegs.Add(eeg);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log.Error("Mensaje: ", ex);
            }
            return null;

            //Sesion sesion = new Sesion();
            //using (var entidades = _context)
            //{
            //    var usuario = entidades.Usuarios
            //                    .Where(s => s.Id == id)
            //                    .FirstOrDefault();
            //    sesion = usuario.Sesiones.Where(u => u.Id == usuario.Id).FirstOrDefault();
            //    sesion.Eegs = new List<Eeg>();
            //    sesion.Eegs.Add(eeg);
            //    await entidades.SaveChangesAsync();
            //}
            //return sesion;
        }
    }
}
