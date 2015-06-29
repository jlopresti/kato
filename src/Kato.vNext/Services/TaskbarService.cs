using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardcodet.Wpf.TaskbarNotification;
using Kato.vNext.Core;
using Kato.vNext.Models;

namespace Kato.vNext.Services
{
    public class NotificationService
    {
        private readonly ITaskbarService _service;

        public NotificationService(ITaskbarService service)
        {
            _service = service;
        }

        public void ShowError(string serverName, string message)
        {
            _service.ShowStandardBalloon(serverName, message, BalloonIcon.Error);
        }
    }
}
