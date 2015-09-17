/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Kato.vNext"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Kato.vNext.Core;
using Kato.vNext.Services;
using Microsoft.Practices.ServiceLocation;

namespace Kato.vNext.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}            
            SimpleIoc.Default.Register<IWindowService>(() => new WindowService(Application.Current.MainWindow));
            SimpleIoc.Default.Register<ITaskbarService>(() => new TaskbarService(Application.Current.MainWindow));
            SimpleIoc.Default.Register<ApplicationDataService>();
            SimpleIoc.Default.Register<NotificationService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ServersViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<JobsViewModel>();
            SimpleIoc.Default.Register<TaskbarViewModel>();
            SimpleIoc.Default.Register<MonitoringViewModel>();

        }

        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public ServersViewModel ServersViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ServersViewModel>();
            }
        }

        public SettingsViewModel SettingsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsViewModel>();
            }
        }

        public JobsViewModel JobsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<JobsViewModel>();
            }
        }

        public TaskbarViewModel TaskbarViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TaskbarViewModel>();
            }
        }

        public MonitoringViewModel MonitoringViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MonitoringViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}