using BiofeebackDrivingSimulator.Datos;
using System.Collections.Generic;
using System.Data.Entity;
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
            Sesion sesion = new Sesion();
            using (var entidades = _context)
            {
                var usuario = entidades.Usuarios
                                .Where(s => s.Id == id)
                                .FirstOrDefault();
                sesion = usuario.Sesiones.Where(u => u.Id == usuario.Id).FirstOrDefault();
                sesion.Eegs = new List<Eeg>();
                sesion.Eegs.Add(eeg);
                await entidades.SaveChangesAsync();
            }
            return sesion;
        }
    }
}
