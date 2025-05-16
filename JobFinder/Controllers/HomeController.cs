using JobFinder.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFirebaseService _firebaseService;

        public HomeController(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        public async Task<IActionResult> Index()
        {
            var ofertas = await _firebaseService.ObtenerTodasOfertas();
            return View(ofertas); // ✅ Pasa las ofertas a la vista
        }
    }
}
