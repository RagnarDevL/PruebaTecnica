using LiteThinking.Data;
using LiteThinkingPrueba.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LiteThinkingPrueba.Controllers
{

    public class EmpresaController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<EmpresaController> _logger; // Inyecta el servicio de logging

        // Constructor del controlador
        public EmpresaController(ApplicationDbContext context, ILogger<EmpresaController> logger)
        {
            _dbContext = context;
            _logger = logger; // Asigna el logger inyectado
        }

        // Muestra el formulario para crear una nueva empresa
        [HttpGet]
        public ActionResult CreateEmpresa()
        {
            _logger.LogInformation("Accediendo a la vista CreateEmpresa.");
            return View(new Empresa());
        }

        // Maneja el envío del formulario para crear una nueva empresa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEmpresa(Empresa model)
        {
            _logger.LogInformation("Recibiendo datos del formulario para crear una nueva empresa.");

            // Verificar si el modelo es válido
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Los datos del formulario son válidos. NIT: {NIT}, Nombre: {Nombre}, Dirección: {Direccion}, Teléfono: {Telefono}",
                                        model.NIT, model.Nombre, model.Direccion, model.Telefono);

                try
                {
                    // Mapear el ViewModel a la entidad que se guardará en la base de datos
                    var empresa = new Empresa
                    {
                        NIT = model.NIT,
                        Nombre = model.Nombre,
                        Direccion = model.Direccion,
                        Telefono = model.Telefono
                    };

                    // Guardar la entidad en la base de datos
                    _dbContext.Empresas.Add(empresa);
                    _dbContext.SaveChanges();

                    _logger.LogInformation("Empresa creada exitosamente en la base de datos. NIT: {NIT}", empresa.NIT);

                    // Redirigir a alguna otra página, como el listado de empresas
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ocurrió un error al guardar la empresa en la base de datos.");
                    ModelState.AddModelError("", "Error al crear la empresa. Por favor, inténtalo nuevamente.");
                }
            }
            else
            {
                // Registrar un log con los detalles de los errores de validación
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        _logger.LogWarning("Error en el campo {Field}: {Error}", modelState.Key, error.ErrorMessage);
                    }
                }
            }

            // Si el modelo no es válido, vuelve a mostrar el formulario con los errores
            return View(model);
        }
    }
}
