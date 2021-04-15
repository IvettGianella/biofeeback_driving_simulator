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
    public class FrecuenciaCardiacaUnitTest
    {
        [TestInitialize]
        public void Initalize()
        {
        }

        [TestMethod]
        public async Task AgregarFrecuenciaCardiacaTest()
        {
            List<FrecuenciaCardiaca> frecuenciaCardiacas = new List<FrecuenciaCardiaca>
            {
                new FrecuenciaCardiaca
                {
                    Id = 1, 
                    FechaHora = DateTime.Now, 
                    Valor = 86
                },
                new FrecuenciaCardiaca
                {
                    Id = 2,
                    FechaHora = DateTime.Now,
                    Valor = 87
                }
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
                    Sesiones = new List<Sesion>{ new Sesion
                    {
                        Id = 1,
                        Comentarios = "Comentario1",
                        Fecha = DateTime.Now,
                        Eegs = null,
                        FrecuenciaCardiacas = null,
                        Temperaturas = null
                    } },
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

            var service = new FrecuenciaCardiacaService(mockContext.Object);
            var sesion = await service.AgregarFrecuenciaCardiaca(1, frecuenciaCardiacas);
            var FrecuenciaCardiacaRpta = sesion.FrecuenciaCardiacas.ToList();

            Assert.AreEqual("86", FrecuenciaCardiacaRpta[0].Valor.ToString());
            Assert.AreEqual("87", FrecuenciaCardiacaRpta[1].Valor.ToString());

        }
    }
}
