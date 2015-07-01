using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Hardcodet.Wpf.TaskbarNotification;

namespace Kato.vNext.Core
{
    public class TaskbarService : ITaskbarService
    {
        private Window _window;
        private Lazy<TaskbarIcon> _taskbar;
        public TaskbarService(Window window)
        {
            _window = window;
            _taskbar = new Lazy<TaskbarIcon>(EnsureTaskbarInitializd);
        }

        private TaskbarIcon EnsureTaskbarInitializd()
        {
            return UIHelper.FindChildren<TaskbarIcon>(_window).FirstOrDefault();
        }    

        public void ShowStandardBalloon(string title, string message, BalloonIcon icon)
        {            
            _taskbar.Value.ShowBalloonTip(title, message, icon);
        }

        public void ShowCustomBalloon(UIElement balloon, PopupAnimation animation, int? timeout = null)
        {
            if (_taskbar.Value.SupportsCustomToolTips)
            {
                _taskbar.Value.ShowCustomBalloon(balloon, animation, timeout);
            }
        }
    }
}
