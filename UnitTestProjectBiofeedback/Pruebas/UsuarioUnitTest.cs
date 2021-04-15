using BiofeebackDrivingSimulator.Datos;
using BiofeebackDrivingSimulator.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using UnitTestProjectBiofeedback.Utils;

namespace UnitTestProjectBiofeedback.Pruebas
{
    [TestClass]
    public class UsuarioUnitTest
    {
        [TestInitialize]
        public void Initalize()
        {
        }

        [TestMethod]
        public async Task AgregarUsuarioTest()
        {
            Usuario usuario = new Usuario 
            { 
                Nombres = "Usuario1", 
                Apellidos = "Usuario1", 
                Edad = 21, 
                EsMuestra = true, 
                Sesiones = null, 
                Sexo = true 
            };

            var mockSet = MockDbSet.CreateMockDbSet(new List<Usuario>());

            var mockContext = new Mock<Entidades>();
            mockContext.Setup(m => m.Usuarios).Returns(mockSet.Object);

            var service = new UsuarioServices(mockContext.Object);
            var usuarioCreado = await service.AgregarUsuario(usuario);

            mockSet.Verify(m => m.Add(It.IsAny<Usuario>()), Times.Once());
            mockContext.Verify(m => m.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task EditarUsuarioTest()
        {
            var data = new List<Usuario>
            {
                new Usuario 
                { 
                    Id = 1, 
                    Nombres = "Usuario1",
                    Apellidos = "Usuario1", 
                    Edad = 21, 
                    EsMuestra = true, 
                    Sesiones = null, 
                    Sexo = true  
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Usuario>>();
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<Entidades>();
            mockContext.Setup(m => m.Usuarios).Returns(mockSet.Object);

            var service = new UsuarioServices(mockContext.Object);
            var usuario = await service.EditarUsuario(1, "Usuario2", "Usuario2", 22, true, false);

            Assert.AreEqual("Usuario2", usuario.Nombres);
            Assert.AreEqual("Usuario2", usuario.Apellidos);
            Assert.AreEqual(22, usuario.Edad);
        }

        [TestMethod]
        public void ObetenerUsuarioTest()
        {
            var data = new List<Usuario>
            {
                new Usuario 
                { 
                    Id = 1, 
                    Nombres = "Usuario1", 
                    Apellidos = "Usuario1", 
                    Edad = 21, 
                    EsMuestra = true, 
                    Sesiones = null, 
                    Sexo = true  
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Usuario>>();
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<Entidades>();
            mockContext.Setup(c => c.Usuarios).Returns(mockSet.Object);

            var service = new UsuarioServices(mockContext.Object);
            var usuario = service.ObtenerUsuario(1);

            Assert.AreEqual("Usuario1", usuario.Nombres);
            Assert.AreEqual("Usuario1", usuario.Apellidos);
            Assert.AreEqual(21, usuario.Edad);
        }

        [TestMethod]
        public void ObetenerUsuariosTest()
        {
            var data = new List<Usuario>
            {
                new Usuario { Id = 1, Nombres = "Usuario1", Apellidos = "Usuario1", Edad = 21, EsMuestra = true, Sesiones = null, Sexo = true  },
                new Usuario { Id = 2, Nombres = "Usuario2", Apellidos = "Usuario2", Edad = 22, EsMuestra = true, Sesiones = null, Sexo = false  },
                new Usuario { Id = 3, Nombres = "Usuario3", Apellidos = "Usuario3", Edad = 23, EsMuestra = true, Sesiones = null, Sexo = true  },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Usuario>>();
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<Entidades>();
            mockContext.Setup(c => c.Usuarios).Returns(mockSet.Object);

            var service = new UsuarioServices(mockContext.Object);
            var usuarios = service.ObtenerUsuarios();

            Assert.AreEqual(3, usuarios.Count);
            Assert.AreEqual("Usuario1", usuarios[0].Nombres);
            Assert.AreEqual("Usuario2", usuarios[1].Nombres);
            Assert.AreEqual("Usuario3", usuarios[2].Nombres);
        }
    }
}
