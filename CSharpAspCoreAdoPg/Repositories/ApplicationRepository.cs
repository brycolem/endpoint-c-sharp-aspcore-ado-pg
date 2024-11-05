using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Threading.Tasks;
using CSharpAspCoreAdoPg.Models;
using CSharpAspCoreAdoPg.Wrappers;

namespace CSharpAspCoreAdoPg.Repositories
{
  public interface IApplicationRepository
  {
    Task<IEnumerable<Application>> GetAllApplicationsAsync();
    Task<Application?> GetApplicationByIdAsync(int id);
    Task AddApplicationAsync(Application application);
  }

  public class ApplicationRepository : IApplicationRepository
  {
    private readonly IDbConnectionFactory _connectionFactory;

    public ApplicationRepository(IDbConnectionFactory connectionFactory)
    {
      _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Application>> GetAllApplicationsAsync()
    {
      var applications = new List<Application>();

      using (var connection = _connectionFactory.CreateConnection())
      {
        await OpenConnectionAsync(connection);

        string query = @"
                SELECT a.id, a.employer, a.title, a.link, a.company_id,
                    COALESCE(json_agg(json_build_object('id', n.id, 'noteText', n.note_text, 'applicationId', n.application_id)) 
                    FILTER (WHERE n.id IS NOT NULL), '[]') as notes
                FROM applications a
                LEFT JOIN notes n ON a.id = n.application_id
                GROUP BY a.id, a.employer, a.title, a.link, a.company_id";

        using (var command = connection.CreateCommand())
        {
          command.CommandText = query;
          using (var reader = await ExecuteReaderAsync(command))
          {
            while (await ReadAsync(reader))
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
      }

      return applications;
    }

    public async Task<Application?> GetApplicationByIdAsync(int id)
    {
      using (var connection = _connectionFactory.CreateConnection())
      {
        await OpenConnectionAsync(connection);

        using (var command = connection.CreateCommand())
        {
          command.CommandText = @"
                    SELECT a.id, a.employer, a.title, a.link, a.company_id,
                        COALESCE(json_agg(json_build_object('id', n.id, 'noteText', n.note_text, 'applicationId', n.application_id)) 
                        FILTER (WHERE n.id IS NOT NULL), '[]') as notes
                    FROM applications a
                    LEFT JOIN notes n ON a.id = n.application_id
                    WHERE a.id = @id
                    GROUP BY a.id, a.employer, a.title, a.link, a.company_id";

          AddParameter(command, "@id", id);

          using (var reader = await ExecuteReaderAsync(command))
          {
            if (await ReadAsync(reader))
            {
              return new Application
              {
                Id = reader.GetInt32(0),
                Employer = reader.GetString(1),
                Title = reader.GetString(2),
                Link = reader.GetString(3),
                CompanyId = reader.GetInt32(4),
                Notes = JsonSerializer.Deserialize<List<Note>>(reader.GetString(5)) ?? new List<Note>()
              };
            }
          }
        }
      }

      return null;
    }

    public async Task AddApplicationAsync(Application application)
    {
      using (var connection = _connectionFactory.CreateConnection())
      {
        await OpenConnectionAsync(connection);

        using (var command = connection.CreateCommand())
        {
          command.CommandText = @"
                    INSERT INTO applications (employer, title, link, company_id) 
                    VALUES (@employer, @title, @link, @companyId) RETURNING id";

          AddParameter(command, "@employer", application.Employer);
          AddParameter(command, "@title", application.Title);
          AddParameter(command, "@link", application.Link);
          AddParameter(command, "@companyId", application.CompanyId);

          var result = await ExecuteScalarAsync(command);
          application.Id = Convert.ToInt32(result);
        }
      }
    }

    private async Task OpenConnectionAsync(IDbConnection connection)
    {
      if (connection is System.Data.Common.DbConnection dbConnection)
      {
        await dbConnection.OpenAsync();
      }
      else
      {
        throw new NotSupportedException("Connection does not support async operations.");
      }
    }

    private void AddParameter(IDbCommand command, string parameterName, object value)
    {
      var parameter = command.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.Value = value ?? DBNull.Value;
      command.Parameters.Add(parameter);
    }

    private async Task<IDataReader> ExecuteReaderAsync(IDbCommand command)
    {
      if (command is System.Data.Common.DbCommand dbCommand)
      {
        return await dbCommand.ExecuteReaderAsync();
      }
      else
      {
        throw new NotSupportedException("Command does not support async operations.");
      }
    }

    private async Task<object?> ExecuteScalarAsync(IDbCommand command)
    {
      if (command is System.Data.Common.DbCommand dbCommand)
      {
        var result = await dbCommand.ExecuteNonQueryAsync();
        return result;
      }
      else
      {
        throw new NotSupportedException("Command does not support async operations.");
      }
    }

    private async Task<bool> ReadAsync(IDataReader reader)
    {
      if (reader is System.Data.Common.DbDataReader dbReader)
      {
        return await dbReader.ReadAsync();
      }
      else
      {
        throw new NotSupportedException("DataReader does not support async operations.");
      }
    }
  }
}
