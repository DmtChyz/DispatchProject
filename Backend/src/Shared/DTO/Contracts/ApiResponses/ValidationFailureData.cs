namespace Shared.DTO.Contracts.ApiResponses
{
    public sealed record ValidationFailureData(
        IReadOnlyDictionary<string, string> Fields
    );
}