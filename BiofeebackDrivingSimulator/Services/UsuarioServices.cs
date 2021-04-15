using BiofeebackDrivingSimulator.Datos;
using System;
using System.Collections.Generic;
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
                Logger.Log.Error("Mensaje: ", ex);
            }
            return null;
        }

        public async Task<Usuario> EditarUsuario(int id, string nombres, string apellidos, int edad, bool esMuestra, bool sexo)
        {
            var usuario = ObtenerUsuario(id);
            using (var entidades = _context)
            {
                usuario.Nombres = nombres;
                usuario.Apellidos = apellidos;
                usuario.Edad = edad;
                usuario.EsMuestra = esMuestra;
                usuario.Sexo = sexo;
                await _context.SaveChangesAsync();
            }
            return usuario;
        }

        public Usuario ObtenerUsuario(int id)
        {
            var usuario = new Usuario();
            using (var entidades = _context)
            {
                usuario = entidades.Usuarios.Where(u => u.Id == id).FirstOrDefault();
            }
            return usuario;
        }

        public List<Usuario> ObtenerUsuarios()
        {
            var usuarios = new List<Usuario>();
            using (var entidades = _context)
            {
                usuarios = entidades.Usuarios.ToList();
            }
            return usuarios;
        }
    }
}
