using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC_MaestriaAPI.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;

namespace MVC_MaestriaAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        // Vista principal sin datos (puedes eliminarla si no la usas)
        public IActionResult Index()
        {
            return View();
        }

        // Nueva acción que consume la API y pasa los datos a la vista Dashboard 
        public async Task<IActionResult> Dashboard()
        {
            string apiUrl = "https://hkgtyd5yfh.execute-api.us-east-1.amazonaws.com/dev/almacenamiento";
            List<SensorDato> datos = new List<SensorDato>();

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                datos = JsonSerializer.Deserialize<List<SensorDato>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                datos = datos
                    .OrderByDescending(d => d.Fecha)
                    .OrderBy(d => d.Fecha)
                    .ToList();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al obtener datos del API: " + ex.Message;
            }

            return View(datos);
        }


        public async Task<IActionResult> ObtenerDatos()
        {
            string apiUrl = "https://hkgtyd5yfh.execute-api.us-east-1.amazonaws.com/dev/almacenamiento";
            List<SensorDato> datos = new List<SensorDato>();

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                datos = JsonSerializer.Deserialize<List<SensorDato>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                datos = datos.OrderByDescending(d => d.Fecha).ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener datos: " + ex.Message });
            }

            return Json(datos);
        }

        public class SensorDato
        {
            public string Id { get; set; }
            public DateTime Fecha { get; set; }
            public double Temperatura { get; set; }
            public double Humedad { get; set; }
        }

    }
}
