using BiofeebackDrivingSimulator.Datos;
using BiofeebackDrivingSimulator.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProjectBiofeedback.Pruebas
{
    [TestClass]
    public class SesionUnitTest
    {
        [TestInitialize]
        public void Initalize()
        {
        }

        [TestMethod]
        public async Task AgregarSesionTest()
        {
            Sesion sesion = new Sesion 
            { 
                Id = 1,
                Comentarios = "Comentario1",
                Fecha = DateTime.Now,
                Eegs = null,
                FrecuenciaCardiacas = null,
                Temperaturas = null 
            };

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

            var service = new SesionServices(mockContext.Object);
            var sesionRpta = await service.AgregarSesion(1, sesion);

            Assert.AreEqual("Comentario1", sesionRpta.Comentarios);
        }

        [TestMethod]
        public void ObtnerSesionesUsuarioTest()
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
                Sesiones = new List<Sesion> { new Sesion
                {
                    Id = 1,
                    Comentarios = "Comentario1",
                    Fecha = DateTime.Now,
                    Eegs = null,
                    FrecuenciaCardiacas = null,
                    Temperaturas = null
                },
                new Sesion
                {
                    Id = 2,
                    Comentarios = "Comentario2",
                    Fecha = DateTime.Now,
                    Eegs = null,
                    FrecuenciaCardiacas = null,
                    Temperaturas = null
                }},
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

            var service = new SesionServices(mockContext.Object);
            var usuarioSesiones = service.ObtnerSesionesUsuario(1);
            var sesiones = usuarioSesiones.Sesiones.ToList();

            Assert.AreEqual("Comentario1", sesiones[0].Comentarios);
            Assert.AreEqual("Comentario2", sesiones[1].Comentarios);
        }
    }  
}

