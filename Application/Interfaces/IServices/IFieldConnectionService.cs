using Application.DTOS.Responses;

public interface IFieldConnectionService
{
    public Task<List<FieldTypeResponse>> LlamarOtroMicroservicioAsync(string token);
}