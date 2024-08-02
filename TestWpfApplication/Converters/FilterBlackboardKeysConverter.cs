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
    public class FilterBlackboardKeysConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length >= 2 && values[0] is IEnumerable<ViewModel.BlackboardKeyViewModel> keys && values[1] is Runner.Blackboard.BlackboardKeyType filter)
            {
                return keys.Where(k => k.Type == filter || filter == Runner.Blackboard.BlackboardKeyType.Object);
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
