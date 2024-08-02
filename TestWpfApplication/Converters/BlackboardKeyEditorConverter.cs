using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace TestWpfApplication.Converters
{
    public class BlackboardKeyEditorConverter : MarkupExtension, IMultiValueConverter
    {
        public bool CanChangeInputType { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length >= 2 && values[0] is ICollection<ViewModel.BlackboardKeyViewModel> availableKeys && values[1] is ViewModel.BlackboardKeyViewModel target)
            {
                return new ViewModel.BlackboardKeyEditorViewModel
                {
                    AvailableKeys = availableKeys,
                    Target = target,
                    IsEditing = values.Length >= 3 && values[2] is bool b && b,
                    CanChangeInputType = CanChangeInputType && (target.Type != Runner.Blackboard.BlackboardKeyType.Object || target.CanChangeType),
                    CanChangeKeyType = target.CanChangeType
                };
            }

            return values;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
