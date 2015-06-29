using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jenkins.Api.Client
{

    public class BuildDetails
    {
        public Action[] actions { get; set; }
        //public bool building { get; set; }
        //public object description { get; set; }
        //public string displayName { get; set; }
        //public string fullDisplayName { get; set; }
        //public string id { get; set; }
        //public bool keepLog { get; set; }
        //public int number { get; set; }
        //public int queueId { get; set; }
        //public string result { get; set; }
        //public long timestamp { get; set; }
        //public string url { get; set; }
        public string builtOn { get; set; }
    }

    public class Action
    {
        public Cause[] causes { get; set; }
        public string scmName { get; set; }
        public int failCount { get; set; }
        public int skipCount { get; set; }
        public int totalCount { get; set; }
        public string urlName { get; set; }
    }   

    public class Cause
    {
        public string shortDescription { get; set; }
        public int upstreamBuild { get; set; }
        public string upstreamProject { get; set; }
        public string upstreamUrl { get; set; }
    }


}
