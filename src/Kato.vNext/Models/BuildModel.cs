using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Jenkins.Api.Client;

namespace Kato.vNext.Models
{
    public class BuildModel : ObservableObject
    {
        private Build _build;
        public BuildModel(Build build)
        {
            _build = build;
            TimeStamp = ConvertTimestamp(_build.Timestamp);
            EstimatedDuration = TimeSpan.FromMilliseconds(_build.EstimatedDuration);
            Duration = _build.Duration == 0 && Building ? (DateTime.UtcNow - TimeStamp) : TimeSpan.FromMilliseconds(_build.Duration);
        }

        public bool Building { get { return _build.Building; } }
        public string Result { get { return _build.Result; } }
        public string BuiltOn { get { return _build.BuiltOn; } }
        public string Url { get { return _build.Url.ToString(); } }

        public double BuildPercentage
        {
            get { return Building ? (Duration.TotalSeconds / EstimatedDuration.TotalSeconds) * 100.00 : 0; }
        }

        public TimeSpan Duration { get; private set; }

        public TimeSpan EstimatedDuration { get; private set; }

        public DateTime TimeStamp { get; private set; }

        static readonly DateTime s_epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private DateTime ConvertTimestamp(long timeStamp)
        {
            return s_epoch + TimeSpan.FromMilliseconds(timeStamp);
        }
    }
}
