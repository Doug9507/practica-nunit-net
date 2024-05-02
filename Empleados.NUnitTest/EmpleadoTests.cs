using Empleados.Core.Modelos;
using Empleados.Infraestructura.Data;
using Empleados.Infraestructura.Repositorio;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Empleados.NUnitTest
{
    [TestFixture]
    public class EmpleadoTests
    {
        Empleado empleado1;
        Empleado empleado2;
        DbContextOptions<ApplicationDbContext> options;
        ApplicationDbContext context;
        EmpleadoRepositorio empleadoRepositorio;

        [SetUp]
        public void Setup()
        {
            empleado1 = new Empleado()
            {
                Apellidos = "Perez",
                Nombres = "Juan",
                Cargo = "Desarrollador",
                CompaniaId = 1,
            };

            empleado2 = new Empleado()
            {
                Apellidos = "Gomez",
                Nombres = "Maria",
                Cargo = "Analista",
                CompaniaId = 2,
            };

            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            context = new ApplicationDbContext(options);

            empleadoRepositorio = new EmpleadoRepositorio(context);
        }

        [Test]
        public async Task EmpleadoAgregar_IngresarEmpleado_GuardadoExitoso()
        {
            // Arrange
            //var context = new ApplicationDbContext(options);
            //var empleadoRepositorio = new EmpleadoRepositorio(context);

            // Act
            await empleadoRepositorio.Agregar(empleado1);
            await empleadoRepositorio.Agregar(empleado2);
            await empleadoRepositorio.Guardar();

            var emp = await empleadoRepositorio.ObtenerPrimero();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(empleado1, emp);
                Assert.AreEqual(empleado1.Id, emp.Id);
                Assert.AreEqual(empleado1.Nombres, emp.Nombres);
            });
        }
    }
}
