using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;


namespace Kato.vNext.Core
{
    public class ValidatableModel : ObservableObject, INotifyDataErrorInfo, IDisposable
    {
        private ConcurrentDictionary<string, List<string>> _errors =
            new ConcurrentDictionary<string, List<string>>();


        public ValidatableModel()
        {
            PropertyChanged += ValidatableModel_PropertyChanged;
        }

        private async void ValidatableModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            await ValidateAsync();
            OnPropertyChanged();
        }

        protected virtual void OnPropertyChanged()
        {
            
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public void OnErrorsChanged(string propertyName)
        {
            var handler = ErrorsChanged;
            if (handler != null)
                handler(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            List<string> errorsForName;
            _errors.TryGetValue(propertyName, out errorsForName);
            return errorsForName;
        }

        public bool HasErrors
        {
            get { return _errors.Any(kv => kv.Value != null && kv.Value.Count > 0); }
        }

        public Task ValidateAsync()
        {
            return Task.Run(() => Validate());
        }

        public void AddError(string property, string message)
        {
            if (_errors.ContainsKey(property))
            {
                List<string> outLi;
                _errors.TryRemove(property, out outLi);
            }
            if (!string.IsNullOrEmpty(message))
            {
                _errors.TryAdd(property, new List<string> {message});
            }
            OnErrorsChanged(property);
        }

        private object _lock = new object();
        public void Validate()
        {
            lock (_lock)
            {
                var validationContext = new ValidationContext(this, null, null);
                var validationResults = new List<ValidationResult>();
                Validator.TryValidateObject(this, validationContext, validationResults, true);

                foreach (var kv in _errors.ToList())
                {
                    if (validationResults.All(r => r.MemberNames.All(m => m != kv.Key)))
                    {
                        List<string> outLi;
                        _errors.TryRemove(kv.Key, out outLi);
                        OnErrorsChanged(kv.Key);
                    }
                }

                var q = from r in validationResults
                        from m in r.MemberNames
                        group r by m into g
                        select g;

                foreach (var prop in q)
                {
                    var messages = prop.Select(r => r.ErrorMessage).ToList();

                    if (_errors.ContainsKey(prop.Key))
                    {
                        List<string> outLi;
                        _errors.TryRemove(prop.Key, out outLi);
                    }
                    _errors.TryAdd(prop.Key, messages);
                    OnErrorsChanged(prop.Key);
                }
            }
        }

        public void Dispose()
        {
            PropertyChanged -= ValidatableModel_PropertyChanged;
        }
    }

}
