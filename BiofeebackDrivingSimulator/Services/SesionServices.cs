using BiofeebackDrivingSimulator.Datos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiofeebackDrivingSimulator.Services
{
    public class SesionServices
    {
        private readonly Entidades _context;

        public SesionServices(Entidades context)
        {
            _context = context;
        }

        public async Task<Sesion> AgregarSesion(int idUsuario, Sesion sesionNueva)
        {
            Sesion sesion = new Sesion();
            using (var entidades = _context)
            {
                var usuario = entidades.Usuarios.Where(u => u.Id == idUsuario).FirstOrDefault();

                if (usuario != null)
                {
                    usuario.Sesiones = new List<Sesion>();
                    usuario.Sesiones.Add(sesionNueva);

                    await entidades.SaveChangesAsync();

                    usuario = entidades.Usuarios.Where(u => u.Id == idUsuario).FirstOrDefault();
                    sesion = usuario.Sesiones.ToList().OrderBy(s => s.Fecha).FirstOrDefault();
                }
            }
            return sesion;
        }

        public async void AgregarComentarioSesion(int id, string comentarios)
        {
            using (var entidades = _context)
            {
                var sesion = entidades.Sesiones
                                .Where(s => s.Id == id)
                                .FirstOrDefault();

                sesion.Comentarios = comentarios;
                await entidades.SaveChangesAsync();
            }
        }

        public Usuario ObtnerSesionesUsuario(int idUsuario)
        {
            Usuario usuario = new Usuario();
            using (var entidades = _context)
            {
                usuario = entidades.Usuarios
                                .Where(u => u.Id == idUsuario)
                                .FirstOrDefault();
            }
            return usuario;
        }

        public Sesion ObtenerSesionCompleta(int id)
        {
            Sesion sesion = new Sesion();
            using (var entidades = new Entidades())
            {
                sesion = entidades.Sesiones
                                       .Where(s => s.Id == id)
                                       .Include(s => s.FrecuenciaCardiacas)
                                       .Include(s => s.Eegs)
                                       .Include(s => s.Temperaturas)
                                       .FirstOrDefault();
            }
            return sesion;
        }
    }
}
