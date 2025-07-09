using AppPacientesWeb.Models;
using AppPacientesWeb.Models.API_REST;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace AppPacientesWeb.Controllers
{
    public class CustomersController : Controller
    {
        public async Task<IActionResult> ViewCustomers()
        {

            Consumo conso = new Consumo();
            string Resultado = await conso.ConsumoAPI("https://localhost:7056/GestionPacientes/ObtenerPacientes");
            var pacientes = JsonConvert.DeserializeObject<List<Pacientes>>(Resultado);

            return View(pacientes);
        }


        public async Task<IActionResult> CreateCustomers()
        {
            return View();
        }

        public async Task<IActionResult> AddNewUser(string Name, string LastName, string ID, DateTime FchaNacimiento, string Sexo)
        {

            var nuevoPaciente = new Pacientes
            {
                nombre = Name,
                apellido = LastName,
                numero_identificacion = ID,
                fecha_nacimiento = FchaNacimiento,
                genero = Sexo
            };

            string JSON = JsonConvert.SerializeObject(nuevoPaciente);

            Consumo conso = new Consumo();
            bool Exitoso  = await conso.InsertarPacienteAsync(JSON, "https://localhost:7056/GestionPacientes/InsertarPaciente");

            if (Exitoso) {

                TempData["MostrarAlertaUser"] = true;



            }
            return RedirectToAction("CreateCustomers", "Customers");
        }


    }
}
