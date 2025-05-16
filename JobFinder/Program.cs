using JobFinder.Core.Models;
using JobFinder.Core.Services;
using Google.Cloud.Firestore;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin;
using System.IO;
using Google.Cloud.Firestore.V1;
using Microsoft.AspNetCore.Authentication.Cookies; // ✅ Necesario para Path.Combine

var builder = WebApplication.CreateBuilder(args);

// 🔥 Configuración robusta de Firebase
var currentDirectory = Directory.GetCurrentDirectory();
var credentialPath = Path.Combine(currentDirectory, "FirebaseCredentials", "firebase-service-account.json");

// 1. Validación del archivo de credenciales
if (!File.Exists(credentialPath))
{
    throw new FileNotFoundException("❗ Archivo de credenciales de Firebase no encontrado", credentialPath);
}

var credential = GoogleCredential.FromFile(credentialPath);
FirebaseApp.Create(new AppOptions { Credential = credential });

// 2. Configuración explícita de Firestore con credenciales
// ✅ Configuración CORRECTA de Firestore
var firestoreClient = new FirestoreClientBuilder
{
    Credential = credential
}.Build(); // Construye el cliente explícitamente

var firestore = FirestoreDb.Create(
    projectId: "jobfinder-6e7ca",
    client: firestoreClient); // Usa el parámetro correcto "client"

builder.Services.AddSingleton(firestore);

// 🔄 Servicio adaptado a Firebase
// Antes de builder.Build():
builder.Services.AddScoped<IFirebaseService, FirebaseService>(); // ✅ Registra interfaz + implementación

// 🎯 Configuración mejorada de MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Muestra errores detallados
}

// 🛡️ Middlewares con seguridad mejorada
app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = false // Mejor seguridad
});

app.UseRouting();

// 🔐 Configuración de autenticación (si la usas)
// app.UseAuthentication();
app.UseAuthorization();

// 🗺️ Sistema de rutas optimizado
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Añadir autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Home/Error";
    });

app.Run();