using APIPacientes.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySqlConnector;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace APIPacientes.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GestionPacientesController : ControllerBase
    {


        private readonly string _conexion;

        public GestionPacientesController(IConfiguration configuration)
        {
            _conexion = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        [Route("ObtenerPacientes")]
        public async Task<IActionResult> ObtenerPacientes()
        {
            using var connection = new MySqlConnection(_conexion);

            try
            {

                DataTable Recibido = new();
                string JSON = string.Empty;
                await connection.OpenAsync();


                using var command = new MySqlCommand("ObtenerPacientes", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };


                using var reader = await command.ExecuteReaderAsync();
                Recibido.Load(reader);

                JSON = JsonConvert.SerializeObject(Recibido);

                await connection.CloseAsync();

                return Ok(JSON);

            }
            catch (Exception E)
            {

                await connection.CloseAsync();
                return BadRequest(E.Message);

            }


        }


        [HttpPost]
        [Route("InsertarPaciente")]
        public async Task<IActionResult> InsertarPaciente([FromBody] Pacientes paciente)
        {
            using var connection = new MySqlConnection(_conexion);

            try
            {


                await connection.OpenAsync();


                using var command = new MySqlCommand("InsertarPaciente", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new MySqlParameter("@p_nombre", paciente.nombre));
                command.Parameters.Add(new MySqlParameter("@p_apellido", paciente.apellido));
                command.Parameters.Add(new MySqlParameter("@p_fecha_nacimiento",
                    paciente.fecha_nacimiento == DateTime.MinValue ? DBNull.Value : paciente.fecha_nacimiento));
                command.Parameters.Add(new MySqlParameter("@p_genero", paciente.genero));
                command.Parameters.Add(new MySqlParameter("@p_numero_identificacion", paciente.numero_identificacion));

                await command.ExecuteNonQueryAsync();

                await connection.CloseAsync();

                return Ok();

            }
            catch (Exception E)
            {

                await connection.CloseAsync();
                return BadRequest(E.Message);

            }


        }



        


    }
}
