using LiteThinking.Data;
using LiteThinkingPrueba.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LiteThinkingPrueba.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmpresaController> _logger; // Inyecta el servicio de logging

        // Constructor del controlador
        public ProductoController(ApplicationDbContext context, ILogger<EmpresaController> logger)
        {
            _context = context;
            _logger = logger; // Asigna el logger inyectado
        }
        

        // GET: Producto/Create
        public ActionResult Create()
        {

            ViewBag.EmpresaNIT = new SelectList(_context.Empresas, "NIT", "Nombre");
            return View();
        }

        // POST: Producto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind( "Codigo,Nombre,Caracteristicas,Precio,EmpresaNIT")] Producto producto)
        {
            _logger.LogInformation("Los datos del formulario son válidos. Codigo: {Codigo}, Nombre: {Nombre}, Caracteristicas: {Caracteristicas}, Empresa: {Empresa}",
                                         producto.Codigo, producto.Nombre, producto.Caracteristicas, producto.Empresa);

            if (ModelState.IsValid)
            {
                _logger.LogInformation("INFORMACION VALIDA");

                _context.Productos.Add(producto);
                _context.SaveChanges();
                return RedirectToAction("Index");
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
            ViewBag.EmpresaNIT = new SelectList(_context.Empresas, "NIT", "Nombre", producto.Empresa);
            return View(producto);
        }
    }
}
