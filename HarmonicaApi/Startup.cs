namespace HarmonicaApi
{
    public class Startup(IConfiguration configuration)
    {
        private IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMemoryCache();
            string? filePath = Configuration.GetValue<string>("SongsJsonFilePath");
            CanReadWriteJson json;
            services.AddSingleton(json = new CanReadWriteJson(filePath));
            services.AddSingleton(new SongDictionaryBuilder(json));
            services.AddCors(options =>
            {
                options.AddPolicy("LocalhostPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:8081") // replace with your localhost URL
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("LocalhostPolicy");
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}