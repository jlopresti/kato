using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kato.vNext.Models
{
    public sealed class UserData
    {
        public UserData()
        {
            Servers = new List<SavedJenkinsServers>();
        }
        public List<SavedJenkinsServers> Servers { get; set; }
    }
    public class SavedJenkinsServers
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public SavedJenkinsServers()
        {
            Jobs = new List<SavedJob>();
        }
        public List<SavedJob> Jobs { get; set; }
    }

    public class SavedJob
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
