using BiofeebackDrivingSimulator.Datos;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BiofeebackDrivingSimulator.Services
{
    public class TemperaturaService
    {
        private readonly Entidades _context;

        public TemperaturaService(Entidades context)
        {
            _context = context;
        }

        public async Task<Sesion> AgregarTemperatura(int id, Temperatura temperatura)
        {
            Sesion sesion = new Sesion();
            using (var entidades = _context)
            {
                var usuario = entidades.Usuarios
                                .Where(s => s.Id == id)
                                .FirstOrDefault();
                sesion = usuario.Sesiones.Where(u => u.Id == usuario.Id).FirstOrDefault();
                sesion.Temperaturas = new List<Temperatura>();
                sesion.Temperaturas.Add(temperatura);
                await entidades.SaveChangesAsync();
            }
            return sesion;
        }
    }
}
