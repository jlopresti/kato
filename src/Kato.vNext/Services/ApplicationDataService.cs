using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kato.vNext.Models;
using Kato.vNext.Repositories;

namespace Kato.vNext.Services
{
    public class ApplicationDataService
    {
        private LocalDataRepository _localDataRepository;
        private bool _isInitialize;
        private List<ServerModel> _savedServers;
        public ApplicationDataService()
        {
            _localDataRepository = new LocalDataRepository();
        }

        private async Task EnsureInitialize()
        {
            if (!_isInitialize)
            {
                await Initialize();
                _isInitialize = true;
            }
        }

        private async Task Initialize()
        {
            _savedServers = await _localDataRepository.RetrieveDataAsync();
        }

        public Task AddServerAsync(ServerModel server)
        {
            _savedServers.Add(server);
            return _localDataRepository.SaveDataAsync(_savedServers);
        }

        public async Task<List<ServerModel>> GetServersAsync()
        {
            await EnsureInitialize();
            return _savedServers;
        }

        public Task ClearDataAsync()
        {
            _savedServers = new List<ServerModel>();
            return _localDataRepository.SaveDataAsync(_savedServers);
        }
    }
}
