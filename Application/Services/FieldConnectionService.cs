using Application.DTOS.Responses;
using Newtonsoft.Json;
using System.Net.Http.Headers;

public class FieldConnectionService : IFieldConnectionService
{
    private readonly HttpClient _httpClient;

    public FieldConnectionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<FieldTypeResponse>> LlamarOtroMicroservicioAsync(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync("https://localhost:7267/api/v1/FieldTypeNavigator");

        if (response.IsSuccessStatusCode)
        {
            // Deserializar la respuesta si es exitosa
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<FieldTypeResponse>>(result);
        }
        else
        {
            // Manejar errores si la solicitud falla
            throw new Exception("Error al llamar al microservicio");
        }
    }
}