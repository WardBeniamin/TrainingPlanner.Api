namespace TrainingPlanner.Api.Errors
{
    public class ApiError
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;

        // Bra i dev-läge för att se exakt vad som hände
        public string? Details { get; set; }

        // Extra spårning (bra att ha)
        public string? TraceId { get; set; }
    }
}
