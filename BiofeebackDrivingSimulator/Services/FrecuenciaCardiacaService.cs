using BiofeebackDrivingSimulator.Datos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiofeebackDrivingSimulator.Services
{
    public class FrecuenciaCardiacaService
    {
        private readonly Entidades _context;

        public FrecuenciaCardiacaService(Entidades context)
        {
            _context = context;
        }

        public async Task<Sesion> AgregarFrecuenciaCardiaca(int id, List<FrecuenciaCardiaca> frecuenciaCardiacas)
        {
            Sesion sesion = new Sesion();
            using (var entidades = _context)
            {
                var usuario = entidades.Usuarios
                                .Where(s => s.Id == id)
                                .FirstOrDefault();

                sesion = usuario.Sesiones.Where(u => u.Id == usuario.Id).FirstOrDefault();
                sesion.FrecuenciaCardiacas = new List<FrecuenciaCardiaca>();

                foreach (var item in frecuenciaCardiacas)
                {
                    sesion.FrecuenciaCardiacas.Add(item);
                }

                await entidades.SaveChangesAsync();
            }
            return sesion;
        }
    }
}
