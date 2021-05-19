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
    public class UsuarioServices
    {
        private readonly Entidades _context;

        public UsuarioServices(Entidades context)
        {
            _context = context;
        }

        public async Task<Usuario> AgregarUsuario(Usuario usuario)
        {
            try
            {
                var usuarioNuevo = _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                return usuarioNuevo;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log.Error("Mensaje: ", ex);
            }
            return null;
        }

        public async Task<Usuario> EditarUsuario(int id, string nombres, string apellidos, int edad, bool esMuestra, bool sexo)
        {
            try
            {
                var usuario = ObtenerUsuario(id);

                if (usuario == null) return null;

                usuario.Nombres = nombres;
                usuario.Apellidos = apellidos;
                usuario.Edad = edad;
                usuario.EsMuestra = esMuestra;
                usuario.Sexo = sexo;
                await _context.SaveChangesAsync();
                return usuario;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log.Error("Mensaje: ", ex);
            }

            return null;
        }

        public Usuario ObtenerUsuario(int id)
        {
            try
            {
                var usuario = _context.Usuarios.Find(id);
                return usuario;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log.Error("Mensaje: ", ex);
            }

            return null;
        }

        public List<Usuario> ObtenerUsuarios()
        {
            try
            {
                return _context.Usuarios.ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log.Error("Mensaje: ", ex);
            }

            return null;
        }

        public async Task<List<Usuario>> ObtenerUsuariosAsync()
        {
            try
            {
                var usuarios = await _context.Usuarios.ToListAsync();
                return usuarios;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log.Error("Mensaje: ", ex);
            }

            return null;
        }
    }
}
