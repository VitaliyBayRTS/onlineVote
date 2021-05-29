using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OV.MVX.Helpers
{
    public class ErrorHandler : INotifyDataErrorInfo
    {
        public readonly Dictionary<string, List<string>> _propertyError = new Dictionary<string, List<string>>();
        public bool HasErrors => _propertyError.Any();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public void ClearError(string propertyName)
        {
            if (_propertyError.Remove(propertyName))
            {
                OnErrorChange(propertyName);
            }
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            return _propertyError.GetValueOrDefault(propertyName ?? "", null);
        }

        public void AddError(string propertyName, string errorMessage)
        {
            if (!_propertyError.ContainsKey(propertyName))
            {
                _propertyError.Add(propertyName, new List<string>());
            }

            _propertyError[propertyName].Add(errorMessage);
            OnErrorChange(propertyName);
        }

        private void OnErrorChange(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
