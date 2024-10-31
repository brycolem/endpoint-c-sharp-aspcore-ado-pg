using CSharpAspCoreAdoPg.Configurations;
using CSharpAspCoreAdoPg.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Text.Json;

namespace CSharpAspCoreAdoPg.Services
{
    public interface IApplicationService
    {
        Task<IEnumerable<Application>> GetApplicationsAsync();
        Task AddApplicationAsync(Application application);
    }

    public class ApplicationService : IApplicationService
    {
        private readonly string _connectionString;

        public ApplicationService(DatabaseConfiguration config)
        {
            _connectionString = config.ConnectionString ?? throw new InvalidOperationException("Connection string not initialized.");
        }

        public async Task<IEnumerable<Application>> GetApplicationsAsync()
        {
            var applications = new List<Application>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"
                    SELECT a.id, a.employer, a.title, a.link, a.company_id,
                        COALESCE(json_agg(json_build_object('id', n.id, 'noteText', n.note_text, 'applicationId', n.application_id)) 
                        FILTER (WHERE n.id IS NOT NULL), '[]') as notes
                    FROM applications a
                    LEFT JOIN notes n ON a.id = n.application_id
                    GROUP BY a.id, a.employer, a.title, a.link, a.company_id";

                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var app = new Application
                        {
                            Id = reader.GetInt32(0),
                            Employer = reader.GetString(1),
                            Title = reader.GetString(2),
                            Link = reader.GetString(3),
                            CompanyId = reader.GetInt32(4),
                            Notes = JsonSerializer.Deserialize<List<Note>>(reader.GetString(5)) ?? new List<Note>()
                        };

                        applications.Add(app);
                    }
                }
            }

            return applications;
        }

        public async Task AddApplicationAsync(Application application)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand("INSERT INTO applications (employer, title, link, company_id) VALUES (@employer, @title, @link, @companyId) RETURNING id", connection))
                {
                    command.Parameters.AddWithValue("employer", application.Employer);
                    command.Parameters.AddWithValue("title", application.Title);
                    command.Parameters.AddWithValue("link", application.Link);
                    command.Parameters.AddWithValue("companyId", application.CompanyId);

                    application.Id = (int)(await command.ExecuteScalarAsync() ?? 0);
                }
            }
        }
    }
}
