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
    public class EegUnitTest
    {
        [TestInitialize]
        public void Initalize()
        {
        }

        [TestMethod]
        public async Task AgregarFrecuenciaCardiacaTest()
        {
            Eeg eeg = new Eeg
            {
                Id = 1,
                Attention = 100,
                Delta = 101,
                FechaHora = DateTime.Now,
                HighAlpha = 102,
                HighBeta = 103,
                HighGamma = 104,
                LowAlpha = 105,
                LowBeta = 106,
                LowGamma = 107,
                Meditation = 99,
                Senal = 0,
                Theta = 108
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

            var service = new EegService(mockContext.Object);
            var sesion = await service.AgregarEeg(1, eeg);
            var eegRpta = sesion.Eegs.ToList();

            Assert.AreEqual("105", eegRpta.First().LowAlpha.ToString());
            Assert.AreEqual("106", eegRpta.First().LowBeta.ToString());
        }
    }
}
