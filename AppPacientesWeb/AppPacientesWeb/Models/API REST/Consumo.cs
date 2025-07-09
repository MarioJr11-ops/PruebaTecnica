using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AppPacientesWeb.Models.API_REST
{
    public class Consumo
    {

        private readonly HttpClient _httpClient;



        public Consumo()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> ConsumoAPI(string EndPoint) {
            
            try
            {
                var response = await _httpClient.GetAsync(EndPoint);

                response.EnsureSuccessStatusCode(); 

                var contenido = await response.Content.ReadAsStringAsync();

                return contenido;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }


        public async Task<bool> InsertarPacienteAsync(string JSON, string ENDPOINT)
        {

            try
            {


                var content = new StringContent(JSON, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(ENDPOINT, content);

                return response.IsSuccessStatusCode;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar la API: " + ex.Message);
                return false;
            }
        }





    }




}
