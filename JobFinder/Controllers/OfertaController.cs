using Microsoft.AspNetCore.Mvc;
using JobFinder.Core.Models;
using JobFinder.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JobFinder.Controllers
{
    public class OfertasController : Controller
    {
        private readonly IFirebaseService _firebaseService;

        public OfertasController(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        // GET: Ofertas/Index
        public async Task<IActionResult> Index()
        {
            var ofertas = await _firebaseService.ObtenerTodasOfertas();
            return View(ofertas);
        }

        // GET: Ofertas/Crear
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Ofertas/Crear
        [HttpPost]
        public async Task<IActionResult> Crear(Oferta oferta)
        {
            if (ModelState.IsValid)
            {
                await _firebaseService.CrearOferta(oferta);
                return RedirectToAction("Index");
            }
            return View(oferta);
        }

        // GET: Ofertas/Aplicar/5
        public async Task<IActionResult> Aplicar(string id)
        {
            var oferta = await _firebaseService.ObtenerOferta(id);
            if (oferta == null) return NotFound();
            return View(oferta);
        }

        // POST: Ofertas/Aplicar/5
        [HttpPost]
        public async Task<IActionResult> Aplicar(string id, string usuarioId)
        {
            await _firebaseService.AplicarAOferta(id, usuarioId); // Debes obtener el usuarioId del usuario autenticado
            return RedirectToAction("Index");
        }
    }
}
