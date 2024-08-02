using Nodify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWpfApplication.ViewModel
{
    public class BlackboardItemViewModel : ObservableObject
    {
        private string? name;
        public string? Name { get { return name; } set { SetProperty(ref name, value); } }

        private Type? type;
        public Type? Type { get { return type;}  set { SetProperty(ref type, value); } }

        private NodifyObservableCollection<BlackboardKeyViewModel> _input = new NodifyObservableCollection<BlackboardKeyViewModel>();
        public NodifyObservableCollection<BlackboardKeyViewModel> Input
        {
            get => _input;
            set
            {
                if (value == null)
                {
                    value = new NodifyObservableCollection<BlackboardKeyViewModel>();
                }

                SetProperty(ref _input!, value);
            }
        }

        private NodifyObservableCollection<BlackboardKeyViewModel> _output = new NodifyObservableCollection<BlackboardKeyViewModel>();
        public NodifyObservableCollection<BlackboardKeyViewModel> Output
        {
            get => _output;
            set
            {
                if (value == null)
                {
                    value = new NodifyObservableCollection<BlackboardKeyViewModel>();
                }

                SetProperty(ref _output!, value);
            }
        }
    }
}
