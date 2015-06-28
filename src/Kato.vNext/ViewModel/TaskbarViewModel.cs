using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Kato.vNext.Core;
using Kato.vNext.Messages;
using Kato.vNext.Models;
using Kato.vNext.Repositories;
using Kato.vNext.Services;

namespace Kato.vNext.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class TaskbarViewModel : ViewModelBase
    {
        private readonly ApplicationDataService _dataService;
        private readonly IWindowService _windowService;

        public ICommand OpenApplicationCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public TaskbarViewModel(ApplicationDataService dataService, IWindowService windowService)
        {
            _dataService = dataService;
            _windowService = windowService;
            OpenApplicationCommand = new RelayCommand(OpenApplication, () => _windowService.IsMinimized() || _windowService.IsNormal());          
        }

        private void OpenApplication()
        {
            if (_windowService.IsMinimized())
            {
                _windowService.GoToNormal();
                _windowService.ShowInTaskbar();
            }
            else if (_windowService.IsNormal())
            {
                _windowService.ShowWindow();
            }
        }
    }
}