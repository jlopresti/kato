using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using Hardcodet.Wpf.TaskbarNotification;
using Kato.vNext.Controls.Balloons;
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
            _service.ShowCustomBalloon(new MaterialBalloon(new MaterialBalloonModel() {Title = serverName, Body = message, Icon = "/Assets/failed-jenkins.png"}), PopupAnimation.Slide, null);
        }

        public void ShowInfo(string serverName, string message)
        {
            _service.ShowCustomBalloon(new MaterialBalloon(new MaterialBalloonModel() { Title = serverName, Body = message, Icon = "/Assets/jenkins.png" }), PopupAnimation.Slide, null);
        }

        public void ShowSuccess(string serverName, string message)
        {
            _service.ShowCustomBalloon(new MaterialBalloon(new MaterialBalloonModel() { Title = serverName, Body = message, Icon = "/Assets/success-jenkins.png" }), PopupAnimation.Slide, null);
        }
    }
}
