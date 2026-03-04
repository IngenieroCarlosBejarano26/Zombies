namespace ZombieHordeDefenseSystem.Middleware
{
    public class ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        private readonly RequestDelegate _next = next;
        private readonly IConfiguration _configuration = configuration;
        private const string APIKEYHEADER = "X-Api-Key";

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(APIKEYHEADER, out var extractedApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Acceso denegado. Falta X-Api-Key.");
                return;
            }

            string? validApiKey = _configuration.GetValue<string>("Authentication:ApiKey");

            if (string.IsNullOrEmpty(validApiKey) || !validApiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Acceso denegado. API Key incorrecta.");
                return;
            }

            await _next(context);
        }
    }
}
