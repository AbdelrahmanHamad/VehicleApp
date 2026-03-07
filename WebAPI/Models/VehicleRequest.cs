namespace WebAPI.Models
{
    public record GetModelsRequest(int MakeId, int Year);
    public record GetTypesRequest(int MakeId);
}
