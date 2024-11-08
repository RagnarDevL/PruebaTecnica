using LiteThinking.Data;
using LiteThinkingPrueba.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.Internal;
using System.Net.Mail;
using System.Reflection.Metadata;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Document = iTextSharp.text.Document;
using System.Net;

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

            return RedirectToAction("Index", "Inventario");
        }

        // POST: Inventario/EnviarCorreo
        [HttpPost]
        public ActionResult EnviarCorreo(string correo)
        {
            var productos = _context.Productos.Include(p => p.Empresa).ToList();
            string pdfPath = GenerarPDF(productos); // Generar el PDF

            EnviarEmailConPDF(correo, pdfPath); // Función para enviar el correo

            return RedirectToAction("Index", "Inventario");
        }

        private String GenerarPDF(List<Producto> productos)
        {
            // Obtener la fecha y hora actuales para concatenarlas al nombre del archivo
            string fechaHora = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            string rutaPdf = $@"C:\Reportes\Inventario_{fechaHora}.pdf";
            // Verificar si el directorio existe, si no, crearlo
            string directorio = Path.GetDirectoryName(rutaPdf);
            if (!Directory.Exists(directorio))
            {
                Directory.CreateDirectory(directorio);
            }

            // Crear el documento PDF
            Document documento = new Document();

            // Crear escritor para el archivo PDF
            PdfWriter.GetInstance(documento, new FileStream(rutaPdf, FileMode.Create));

            // Abrir el documento para escribir
            documento.Open();

            // Agregar título al documento
            documento.Add(new Paragraph("Reporte de Inventario"));
            documento.Add(new Paragraph(" "));
            documento.Add(new Paragraph("Fecha: " + DateTime.Now.ToString()));
            documento.Add(new Paragraph(" "));

            // Crear una tabla con tres columnas: Nombre, Precio, y Cantidad
            PdfPTable tabla = new PdfPTable(3);
            tabla.AddCell("Nombre");
            tabla.AddCell("Precio");
            tabla.AddCell("Empresa");

            // Iterar sobre la lista de productos y agregar cada uno a la tabla
            foreach (Producto producto in productos)
            {
                tabla.AddCell(producto.Nombre);
                tabla.AddCell(producto.Precio.ToString("C2"));  // Formato de moneda
                tabla.AddCell(producto.Empresa.Nombre.ToString());
            }

            // Agregar la tabla al documento
            documento.Add(tabla);

            // Cerrar el documento
            documento.Close();

            // Devolver la ruta del archivo PDF generado
            return rutaPdf;
        }

        public void EnviarEmailConPDF(string correo, string pdfPath)
        {
            _logger.LogInformation("VAMOS A evniar EL PDFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF.");
            // Configuración del servidor SMTP (en este caso, Gmail)

            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;  // Habilitar SSL
                client.UseDefaultCredentials = false;

                // Credenciales de tu correo
                client.Credentials = new NetworkCredential("kdleon3@misena.edu.co", "Corona0628!");

                // Crear el mensaje de correo
                var mail = new MailMessage("kdleon3@misena.edu.co", correo)
                {
                    Subject = "Inventario",
                    Body = "Adjunto el inventario actualizado.",
                    IsBodyHtml = true  // Si el cuerpo del correo tiene formato HTML
                };

                // Adjuntar el archivo PDF
                mail.Attachments.Add(new Attachment(pdfPath));

                try
                {
                    // Enviar el correo
                    client.Send(mail);
                    
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"errror enviado exitosamenteeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee: {ex.Message}");

                    
                }
            }
        }
    }
}
