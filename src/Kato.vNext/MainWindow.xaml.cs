using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kato.vNext.Core;
using MahApps.Metro.Controls;

namespace Kato.vNext
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private int oldSelectedIndex = -1;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var fe = sender as TabControl;
            if (fe != null && fe.SelectedItem != null)
            {
                if (fe.SelectedIndex != oldSelectedIndex
                    && ((FrameworkElement) fe.SelectedItem).DataContext is ILazyLoader)
                {
                    var tabContent = (FrameworkElement) ((TabItem) fe.SelectedItem).Content;
                    var dc = tabContent.DataContext as ILazyLoader;
                    if (dc != null)
                    {
                        await dc.LoadAsync();
                    }
                }
                oldSelectedIndex = fe.SelectedIndex;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (WindowState != WindowState.Minimized)
            {
                e.Cancel = true;
                WindowState = WindowState.Minimized;
            }
            base.OnClosing(e);
        }

        protected override void OnStateChanged(EventArgs e)
        {
            ShowInTaskbar = WindowState != WindowState.Minimized;

            base.OnStateChanged(e);
        }
    }
}
