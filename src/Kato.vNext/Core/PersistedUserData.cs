using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Kato.vNext.Core
{
    public sealed class PersistedUserData
    {
        private string _savedFilePathBase;

        public PersistedUserData(string savedFilePathBase)
        {
            _savedFilePathBase = savedFilePathBase;
        }

        public Task<T> OpenAsync<T>()
        {
            string filePath = Path.Combine(_savedFilePathBase, typeof(T).Name + ".dat");

            if (!File.Exists(filePath))
                return Task.FromResult(default(T));

            string savedData = File.ReadAllText(filePath, Encoding.UTF8);
            return Task.Run(() => JsonConvert.DeserializeObject<T>(savedData));
        }

        public async Task SaveAsync<T>(T data)
        {
            string filePath = Path.Combine(_savedFilePathBase, typeof(T).Name + ".dat");

            string serializedData = await Task.Run(() =>JsonConvert.SerializeObject(data));
            try
            {
                await Task.Run(() => File.WriteAllText(filePath, serializedData, Encoding.UTF8));
            }
            catch (Exception e)
            {
                
            }
        }        
    }
}
