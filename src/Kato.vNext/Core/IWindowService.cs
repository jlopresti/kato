namespace Kato.vNext.Core
{
    public interface IWindowService
    {
        void ShowWindow();
        bool IsMinimized();
        bool IsNormal();
        bool IsMaximized();
        void GoToNormal();
        void GoToMaximized();
        void GoToMinimized();
        void ShowInTaskbar();
    }
}