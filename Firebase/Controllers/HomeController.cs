using Firebase.Auth;
using Firebase.Models;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Firebase.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }




        [HttpPost]
        public async Task<ActionResult> SubirArchivo(IFormFile archivo)
        {
            //Leemos el archivo subido
            Stream archivoASubir = archivo.OpenReadStream();

            //Configuramos la conexion hacia FireBase
            string email = "arrivillagariveran@gmail.com";
            string clave = "12&10#2017*";
            string ruta = "nose-819e2.appspot.com";
            string api_key = "AIzaSyCO4YDlTb4ZQo08K5mAfeReGLjKDFQpAVQ";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var autenticarFireBase = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();
            var tokenUser = autenticarFireBase.FirebaseToken;

            var tareaCargarArchivo = new FirebaseStorage(ruta,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(tokenUser),
                    ThrowOnCancel = true
                }).Child("Archivos").Child(archivo.FileName).PutAsync(archivoASubir, cancellation.Token);

            var urlArchivoCargado = await tareaCargarArchivo;

            return RedirectToAction("VerImagen");
        }

    }

   

}
