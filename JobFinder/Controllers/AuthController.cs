
// Controllers/AuthController.cs
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using JobFinder.Core.Models;

namespace JobFinder.Controllers
{
    public class AuthController : Controller
    {
        private readonly FirestoreDb _firestore;
        private readonly PasswordHasher<string> _hasher;

        public AuthController(FirestoreDb firestore)
        {
            _firestore = firestore;
            _hasher = new PasswordHasher<string>();
        }

        // GET: /Auth/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Auth/Register
        [HttpPost]
        public async Task<IActionResult> Register(AuthModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Hash de la contraseña
            var hashedPassword = _hasher.HashPassword(model.Correo, model.Password);

            // Datos a guardar
            var userData = new Dictionary<string, object>
            {
                { "usuario", model.Usuario },
                { "correo", model.Correo },
                { "passwordHash", hashedPassword },
                { "telefono", model.Telefono },
                { "fechaNacimiento", model.FechaNacimiento },
                { "localidad", model.Localidad },
                { "transporte", model.Transporte },
                { "esEmpresa", model.EsEmpresa }
            };

            try
            {
                // Guardar en Firestore en colección 'usuarios'
                await _firestore.Collection("usuarios").Document(model.Correo).SetAsync(userData);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al registrar: " + ex.Message);
                return View(model);
            }

            // Autenticar tras registro
            await SignInUser(model.Correo, model.Usuario, model.EsEmpresa);
            return RedirectToAction("Index", "Ofertas");
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        public async Task<IActionResult> Login(AuthModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // Buscar usuario por correo
                var snap = await _firestore.Collection("usuarios").Document(model.Correo).GetSnapshotAsync();
                if (!snap.Exists)
                {
                    ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                    return View(model);
                }

                // Obtener hash y verificar
                var storedHash = snap.GetValue<string>("passwordHash");
                var result = _hasher.VerifyHashedPassword(model.Correo, storedHash, model.Password);
                if (result == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                    return View(model);
                }

                // Leer rol y demás campos
                bool esEmpresa = snap.GetValue<bool>("esEmpresa");
                string usuario = snap.GetValue<string>("usuario");

                // Firmar cookie
                await SignInUser(model.Correo, usuario, esEmpresa, model.RememberMe);
                return RedirectToAction("Index", "Ofertas");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al iniciar sesión: " + ex.Message);
                return View(model);
            }
        }

        // POST: /Auth/Logout
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // Método privado para firmar cookie
        private async Task SignInUser(string email, string nombre, bool esEmpresa, bool rememberMe = false)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, email),
                new Claim(ClaimTypes.Name, nombre),
                new Claim("role", esEmpresa ? "empresa" : "usuario")
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var props = new AuthenticationProperties { IsPersistent = rememberMe };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity), props);
        }
    }
}
