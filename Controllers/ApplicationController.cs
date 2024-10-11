using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using CSharpAspCoreAdoPg.Models;

namespace CSharpAspCoreAdoPg.Controllers
{
    [ApiController]
    [Route("application")]
    public class ApplicationsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ApplicationsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: /application
        [HttpGet]
        public IActionResult GetApplications()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            var applications = new List<Application>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT a.id, a.employer, a.title, a.link, a.company_id,
                           json_agg(json_build_object('id', n.id, 'noteText', n.note_text, 'applicationId', n.application_id)) as notes
                    FROM applications a
                    LEFT JOIN notes n ON a.id = n.application_id
                    GROUP BY a.id, a.employer, a.title, a.link, a.company_id";
                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var app = new Application
                        {
                            Id = reader.GetInt32(0),
                            Employer = reader.GetString(1),
                            Title = reader.GetString(2),
                            Link = reader.GetString(3),
                            CompanyId = reader.GetInt32(4),
                            Notes = new List<Note>()
                        };

                        if (!reader.IsDBNull(5))
                        {
                            var notesJson = reader.GetString(5);
                            var notesList = System.Text.Json.JsonSerializer.Deserialize<List<Note>>(notesJson);
                            if (notesList != null)
                            {
                                app.Notes = notesList;
                            }
                        }

                        applications.Add(app);
                    }
                }
            }

            return Ok(applications);
        }

        // POST: /application
        [HttpPost]
        public IActionResult AddApplication([FromBody] Application application)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("INSERT INTO applications (employer, title, link, company_id) VALUES (@employer, @title, @link, @companyId) RETURNING id", connection))
                {
                    command.Parameters.AddWithValue("employer", application.Employer);
                    command.Parameters.AddWithValue("title", application.Title);
                    command.Parameters.AddWithValue("link", application.Link);
                    command.Parameters.AddWithValue("companyId", application.CompanyId);

                    int newApplicationId = (int)command.ExecuteScalar();
                    application.Id = newApplicationId;
                }
            }

            return Ok($"Application '{application.Title}' added successfully.");
        }
    }
}
