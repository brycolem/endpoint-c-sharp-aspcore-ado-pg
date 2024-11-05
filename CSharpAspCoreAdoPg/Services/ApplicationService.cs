using CSharpAspCoreAdoPg.Models;
using CSharpAspCoreAdoPg.Repositories;

namespace CSharpAspCoreAdoPg.Services
{
    public interface IApplicationService
    {
        Task<IEnumerable<Application>> GetApplicationsAsync();
        Task<Application?> GetApplicationAsync(int id);
        Task AddApplicationAsync(Application application);
    }

    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;

        public ApplicationService(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public Task<IEnumerable<Application>> GetApplicationsAsync()
        {
            return _applicationRepository.GetAllApplicationsAsync();
        }

        public Task<Application?> GetApplicationAsync(int id)
        {
            var application = _applicationRepository.GetApplicationByIdAsync(id);
            return _applicationRepository.GetApplicationByIdAsync(id);
        }

        public async Task AddApplicationAsync(Application application)
        {
            await _applicationRepository.AddApplicationAsync(application);
        }
    }
}
