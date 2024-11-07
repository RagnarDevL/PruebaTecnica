using LiteThinking.Data;
using LiteThinkingPrueba.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace LiteThinkingPrueba.Controllers
{
    public class InventarioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmpresaController> _logger; // Inyecta el servicio de logging

        // Constructor del controlador
        public InventarioController(ApplicationDbContext context, ILogger<EmpresaController> logger)
        {
            _context = context;
            _logger = logger; // Asigna el logger inyectado
        }

        // GET: Inventario
        public ActionResult Index()
        {
            var productos = _context.Productos.Include(p => p.Empresa).ToList();
            return View(productos);
        }

        // POST: Inventario/DownloadPDF
        [HttpPost]
        public ActionResult DownloadPDF()
        {
            var productos = _context.Productos.Include(p => p.Empresa).ToList();
            string pdfPath = GenerarPDF(productos); // Función para generar el PDF

            return File(pdfPath, "application/pdf", "Inventario.pdf");
        }

        // POST: Inventario/EnviarCorreo
        [HttpPost]
        public ActionResult EnviarCorreo(string correo)
        {
            var productos = _context.Productos.Include(p => p.Empresa).ToList();
            string pdfPath = GenerarPDF(productos); // Generar el PDF

            EnviarEmailConPDF(correo, pdfPath); // Función para enviar el correo

            return RedirectToAction("Index");
        }

        private string GenerarPDF(List<Producto> productos)
        {
            // Lógica para generar el PDF
            // Usar una librería como iTextSharp o PdfSharp
            return "ruta/del/pdf/Inventario.pdf";
        }

        private void EnviarEmailConPDF(string correo, string pdfPath)
        {
            // Lógica para enviar el correo con el PDF adjunto
            using (var client = new SmtpClient())
            {
                var mail = new MailMessage("kdleon3@misena.edu.co", correo)
                {
                    Subject = "Inventario",
                    Body = "Adjunto el inventario actualizado.",
                    IsBodyHtml = true
                };
                mail.Attachments.Add(new Attachment(pdfPath));

                client.Send(mail);
            }
        }
    }
}
