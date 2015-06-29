using Hardcodet.Wpf.TaskbarNotification;

namespace Kato.vNext.Core
{
    public interface ITaskbarService
    {
        void ShowStandardBalloon(string title, string message, BalloonIcon icon);
    }
}