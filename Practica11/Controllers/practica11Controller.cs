using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Practica11.Models;
using Firebase.Auth;
using Firebase.Storage;


namespace PracticaMVC.Controllers
{
    public class practica11Controller : Controller
    {
        private readonly ILogger<practica11Controller> _logger;

        public practica11Controller(ILogger<practica11Controller> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SubirArchivo(IFormFile archivo)
        {
            Stream archivoSubir = archivo.OpenReadStream();

            string email = "cargar@gmail.com";
            string clave = "del1alnueve";
            string ruta = "practica11diegoburgos.appspot.com";
            string api_key = "AIzaSyAYRSR72dL6LU-uQUoD8iJYe3NDRgm4QCc";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var autenticarFireBase = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();
            var tokenUser = autenticarFireBase.FirebaseToken;

            var tareaCargarArchivo = new FirebaseStorage(ruta, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(tokenUser),
                ThrowOnCancel = true
            }).Child("Archivos").Child(archivo.FileName).PutAsync(archivoSubir, cancellation.Token);

            var urlArchivoCargado = await tareaCargarArchivo;

            return RedirectToAction("VerImagen");
        }
    }
}