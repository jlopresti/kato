using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Ioc;
using Kato.vNext.Core;

namespace Kato.vNext.Controls.Balloons
{
    /// <summary>
    /// Interaction logic for MaterialBalloon.xaml
    /// </summary>
    public partial class MaterialBalloon : UserControl
    {
        private ITaskbarService _service;
        public MaterialBalloonModel Model { get; private set; }
        private Timer _timer;
        public MaterialBalloon(MaterialBalloonModel model)
        {
            InitializeComponent();
            DataContext = Model = model;
            _service = SimpleIoc.Default.GetInstance<ITaskbarService>();
            _timer=  new Timer(OnTick);
            sb = new Storyboard();
            this.Loaded += MaterialBalloon_Loaded;
            this.Unloaded += MaterialBalloon_Unloaded;
        }

        private void MaterialBalloon_Unloaded(object sender, RoutedEventArgs e)
        {
            _timer.Change(-1, -1);
            _timer.Dispose();
        }

        private void MaterialBalloon_Loaded(object sender, RoutedEventArgs e)
        {
            //_service.ResetCustomBalloonTimer();
            MouseLeave();
        }

        private void OnTick(object state)
        {
            _service.CloseCustomBalloon();

        }

        private bool _isAnimatedEnter = false;
        private bool _isAnimatedExit = false;
        private Storyboard sb;

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            MouseLeave();
        }

        private void MouseLeave()
        {
            if (!_isAnimatedExit)
            {
                sb.Stop();
                Debug.Write("EXIT");
                _isAnimatedExit = true;
                var da = new DoubleAnimation();
                da.From = 1;
                da.To = 0;
                da.Duration = TimeSpan.FromMilliseconds(4000);
                Storyboard.SetTarget(da, Grid);
                Storyboard.SetTargetProperty(da, new PropertyPath(FrameworkElement.OpacityProperty));
                sb.Children.Clear();
                sb.Children.Add(da);
                sb.Completed += Sb_Completed1;
                sb.Begin();
            }
            _timer.Change(4000, int.MaxValue);
        }

        private void Sb_Completed1(object sender, EventArgs e)
        {
            _isAnimatedExit = false;
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            MouseEnter();
        }

        private void MouseEnter()
        {
            _timer.Change(-1, -1);
            //_service.ResetCustomBalloonTimer();
            if (!_isAnimatedEnter)
            {
                sb.Stop();
                Debug.Write("ENTER");
                _isAnimatedEnter = true;
                var da = new DoubleAnimation();
                da.To = 1;
                da.Duration = TimeSpan.FromMilliseconds(300);
                Storyboard.SetTarget(da, Grid);
                Storyboard.SetTargetProperty(da, new PropertyPath(FrameworkElement.OpacityProperty));
                sb.Children.Clear();
                sb.Children.Add(da);
                sb.Completed += Sb_Completed;
                sb.Begin();
            }
        }

        private void Sb_Completed(object sender, EventArgs e)
        {
            _isAnimatedEnter = false;
        }
    }

    public class MaterialBalloonModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Icon { get; set; }
    }
}

