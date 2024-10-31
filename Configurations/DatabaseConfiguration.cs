namespace CSharpAspCoreAdoPg.Configurations
{
    public class DatabaseConfiguration
    {
        public string ConnectionString { get; }

        public DatabaseConfiguration(IConfiguration configuration)
        {
            string database = configuration.GetValue<string>("DATABASE") ?? "DefaultDatabase";
            string dbUser = configuration.GetValue<string>("DB_USER") ?? "DefaultUser";
            string dbPassword = configuration.GetValue<string>("DB_PWD") ?? "DefaultPassword";
            string dbHost = "localhost";

            ConnectionString = $"Server={dbHost};Database={database};User Id={dbUser};Password={dbPassword};Pooling=true;MaxPoolSize=100;";
        }
    }
}
