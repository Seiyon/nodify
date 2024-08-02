using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nodify;
using TestWpfApplication.Runner.Blackboard;

namespace TestWpfApplication.ViewModel
{
    public class BlackboardKeyViewModel : ObservableObject
    {
        private readonly Dictionary<bool, object?> values = new Dictionary<bool, object?>();

        public string? PropertyName {  get; set; }
        private string name = "New Key";
        public string Name
        {
            get { return name; }
            set 
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    SetProperty(ref name, value);
                }   
            }
        }

        private BlackboardKeyType type;
        public BlackboardKeyType Type
        {
            get { return type; }
            set
            {
                if(SetProperty(ref type, value))
                {
                    Value = GetDefaultValue(type);
                }
            }
        }

        private object? _value = BoxValue.False;
        public object? Value
        {
            get => _value;
            set
            {
                SetProperty(ref value, GetRealValue(_value)).Then(()=> values[ValueIsKey] = Value);
            }
        }

        private bool valueIsKey;
        public bool ValueIsKey
        {
            get { return valueIsKey; }
            set
            {
                if (SetProperty(ref valueIsKey, value) && values.TryGetValue(valueIsKey, out var existingValue))
                {
                    Value = existingValue;
                }
            }
        }

        private bool _canChangeType = true;
        public bool CanChangeType
        {
            get => _canChangeType;
            set => SetProperty(ref _canChangeType, value);
        }

        public object? GetRealValue(object? value)
        {
            if(value is string str)
            {
                switch (type)
                {

                    case BlackboardKeyType.Boolean:
                        bool.TryParse(str, out var bTemp);
                        value = bTemp;
                        break;
                    case BlackboardKeyType.Integer:
                        int.TryParse(str, out var iTemp);
                        value = iTemp;
                        break;
                    case BlackboardKeyType.Double:
                        double.TryParse(str, out var dTemp);
                        value = dTemp;
                        break;
                    case BlackboardKeyType.String:
                    case BlackboardKeyType.Object:
                        value = str;
                        break;
                    default:
                        value = null;
                        break;
                }
            }

            return value;
        }

        public static object? GetDefaultValue(BlackboardKeyType type)
        {
            switch (type)
            {
                case BlackboardKeyType.Boolean : return  BoxValue.False;
                case BlackboardKeyType.Integer : return  BoxValue.Int0;
                case BlackboardKeyType.Double  : return BoxValue.Double0;
                case BlackboardKeyType.String  : return null;
                case BlackboardKeyType.Object:   return null;
                default: return null;
            }
        }
    }
}
