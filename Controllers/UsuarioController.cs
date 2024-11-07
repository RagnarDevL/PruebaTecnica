using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using LiteThinking.Data;

public class UsuarioController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UsuarioController> _logger;

    public UsuarioController(ApplicationDbContext context, ILogger<UsuarioController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: /Usuario/Login
    public IActionResult Login()
    {
        return View();
    }

    // POST: /Usuario/Login
    [HttpPost]
    public async Task<IActionResult> Login(string correo, string contraseña)
    {
        _logger.LogInformation("Intento de inicio de sesión con el correo: {Correo}", correo);

        // Validar las credenciales contra la base de datos
        var usuario = _context.Usuarios.SingleOrDefault(u => u.CorreoElectronico == correo && u.Contraseña == contraseña);

        if (usuario != null)
        {
            _logger.LogInformation("Inicio de sesión exitoso para el usuario: {Correo}", correo);

            // Crear las claims del usuario
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.CorreoElectronico),
               
            };

            // Crear la identidad del usuario
            var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");

            // Crear el principal del usuario
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Iniciar sesión con cookies
            await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

            // Redirigir después del login
            return RedirectToAction("Index", "Inventario");
        }

        // Si las credenciales no son válidas, mostrar un mensaje de error
        ViewBag.Error = "Correo o contraseña incorrectos";
        return View();
    }

    // GET: /Usuario/Logout
    public async Task<IActionResult> Logout()
    {
        // Cerrar la sesión
        await HttpContext.SignOutAsync("MyCookieAuth");

        return RedirectToAction("Login");
    }
}