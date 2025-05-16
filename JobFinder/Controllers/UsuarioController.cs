using Microsoft.AspNetCore.Mvc;
using JobFinder.Core.Models;
using JobFinder.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JobFinder.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IFirebaseService _firebaseService;

        public UsuariosController(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        // GET: Usuarios/Registro
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        // POST: Usuarios/Registro
        [HttpPost]
        public async Task<IActionResult> Registro(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                await _firebaseService.CrearUsuario(usuario);
                // Implementa lógica de autenticación aquí
                return RedirectToAction("Index", "Ofertas");
            }
            return View(usuario);
        }
    }
}
