using BiofeebackDrivingSimulator.Datos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
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
            try
            {
                UsuarioServices usuarioServices = new UsuarioServices(_context);
                Sesion sesion = new Sesion();
                var usuario = usuarioServices.ObtenerUsuario(idUsuario);
                if (usuario != null) 
                {
                    usuario.Sesiones = new List<Sesion>();
                    usuario.Sesiones.Add(sesionNueva);
                    await _context.SaveChangesAsync();
                    usuario = usuarioServices.ObtenerUsuario(idUsuario);
                    sesion = usuario.Sesiones.ToList().OrderByDescending(s => s.Fecha).FirstOrDefault();
                }
                return sesion;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log.Error("Mensaje: ", ex);
            }
            return null;
        }

        public async void AgregarComentarioSesion(int idSesiones, string comentarios)
        {
            try
            {
                var sesion = _context.Sesiones
                                    .Where(s => s.Id == idSesiones)
                                    .FirstOrDefault();
                sesion.Comentarios = comentarios;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log.Error("Mensaje: ", ex);
            }
        }

        public Usuario ObtnerSesionesUsuario(int idUsuario)
        {
            try
            {
                var usuarioSesiones = _context.Usuarios.Find(idUsuario);
                return usuarioSesiones;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log.Error("Mensaje: ", ex);
            }
            return null;
        }

        public async void BorrarSesion(int idSesion)
        {
            try
            {
                using (var entidades = new Entidades())
                {
                    var sesion = _context.Sesiones.Find(idSesion);
                    _context.Sesiones.Remove(sesion);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log.Error("Mensaje: ", ex);
            }
            
        }
    }
}
