using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

    public class WindowService : IWindowService
    {
        private Window _window;
        public WindowService(Window window)
        {
            _window = window;
        }

        public void ShowWindow()
        {
            _window.Activate();
        }

        public bool IsMinimized()
        {
            return _window.WindowState == WindowState.Minimized;
        }

        public bool IsNormal()
        {
            return _window.WindowState == WindowState.Normal;
        }

        public bool IsMaximized()
        {
            return _window.WindowState == WindowState.Maximized;
        }

        public void GoToNormal()
        {
            _window.WindowState = WindowState.Normal;
        }

        public void GoToMaximized()
        {
            _window.WindowState = WindowState.Maximized;
        }

        public void GoToMinimized()
        {
            _window.WindowState = WindowState.Minimized;
        }

        public void ShowInTaskbar()
        {
            _window.ShowInTaskbar = true;
        }
    }
}
