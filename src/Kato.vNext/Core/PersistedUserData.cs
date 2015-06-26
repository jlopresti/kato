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

        public T Open<T>()
        {
            string filePath = Path.Combine(_savedFilePathBase, typeof(T).Name + ".dat");

            if (!File.Exists(filePath))
                return default(T);

            string savedData = File.ReadAllText(filePath, Encoding.UTF8);
            return JsonConvert.DeserializeObject<T>(savedData);
        }

        public void Save<T>(T data)
        {
            string filePath = Path.Combine(_savedFilePathBase, typeof(T).Name + ".dat");

            string serializedData = JsonConvert.SerializeObject(data);
            try
            {
                File.WriteAllText(filePath, serializedData, Encoding.UTF8);
            }
            catch (Exception e)
            {
                
            }
        }        
    }
}
