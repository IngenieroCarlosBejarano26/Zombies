using ZombieHordeDefenseSystem.Middleware;

namespace ZombieHordeDefenseSystem.Extensions
{
    public static class AppExtensions
    {
        public static WebApplication UseProjectPipeline(this WebApplication app)
        {
            

            app.UseCors("AllowAll");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<ApiKeyMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

    }
}
