using System.Windows;
using System.Windows.Controls.Primitives;
using Hardcodet.Wpf.TaskbarNotification;

namespace Kato.vNext.Core
{
    public interface ITaskbarService
    {
        void ShowStandardBalloon(string title, string message, BalloonIcon icon);
        void ShowCustomBalloon(UIElement balloon, PopupAnimation animation, int? timeout = null);
        void CloseCustomBalloon();
        void ResetCustomBalloonTimer();
    }
}