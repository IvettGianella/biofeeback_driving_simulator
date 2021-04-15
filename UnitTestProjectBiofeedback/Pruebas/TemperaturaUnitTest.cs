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
    public class TemperaturaUnitTest
    {
        [TestInitialize]
        public void Initalize()
        {
        }

        [TestMethod]
        public async Task AgregarFrecuenciaCardiacaTest()
        {
            Temperatura temperatura = new Temperatura
            {
                Id = 1,
                FechaHora = DateTime.Now,
                Valor = 33
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

            var service = new TemperaturaService(mockContext.Object);
            var sesion = await service.AgregarTemperatura(1, temperatura);
            var tempeturaRpta = sesion.Temperaturas.ToList();

            Assert.AreEqual("33", tempeturaRpta.First().Valor.ToString());
        }
    }
}
