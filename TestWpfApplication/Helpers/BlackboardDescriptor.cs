using Nodify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestWpfApplication.Runner.Blackboard;

namespace TestWpfApplication.Helpers
{
    public class BlackboardDescriptor
    {
        private static readonly Dictionary<Type, ItemDescription> descriptions = new Dictionary<Type, ItemDescription>();

        private class KeyDescription
        {
            public string DisplayName { get; }
            public string PropertyName { get; }
            public BlackboardKeyType Type { get; }
            public bool CanChangeType { get; }

            public KeyDescription(string dispName, string propName, BlackboardKeyType type, bool canChangeType) 
            {
                DisplayName = dispName;
                PropertyName = propName;
                Type = type;
                CanChangeType = canChangeType;
            }
        }

        private class ItemDescription
        {
            public string? Name { get; set; }
            public List<KeyDescription> Input { get; } = new List<KeyDescription>();
            public List<KeyDescription> Output { get; } = new List<KeyDescription>();
        }

        public static ViewModel.BlackboardItemViewModel? GetItem(ViewModel.BlackboardItemReferenceViewModel? actionRef)
        {

            if (actionRef?.Type != null)
            {
                var description = GetDescription(actionRef.Type);

                var input = description.Input.Select(d => new ViewModel.BlackboardKeyViewModel
                {
                    Name = d.DisplayName,
                    Type = d.Type,
                    PropertyName = d.PropertyName,
                    CanChangeType = d.CanChangeType,
                    ValueIsKey = true
                });

                var output = description.Output.Select(d => new ViewModel.BlackboardKeyViewModel
                {
                    Name = d.DisplayName,
                    Type = d.Type,
                    PropertyName = d.PropertyName,
                    CanChangeType = d.CanChangeType,
                    ValueIsKey = true
                });

                return new ViewModel.BlackboardItemViewModel
                {
                    Name = actionRef.Name,
                    Type = actionRef.Type,
                    Input = new NodifyObservableCollection<ViewModel.BlackboardKeyViewModel>(input),
                    Output = new NodifyObservableCollection<ViewModel.BlackboardKeyViewModel>(output),
                };
            }

            return default;
        }


        public static ViewModel.BlackboardItemReferenceViewModel GetReference(Type type)
        {
            var desc = GetDescription(type);

            return new ViewModel.BlackboardItemReferenceViewModel
            {
                Name = desc.Name,
                Type = type
            };
        }

        private static ItemDescription GetDescription(Type type)
        {
            if (!descriptions.TryGetValue(type, out var description))
            {
                var actionAttr = type.GetCustomAttribute<BlackboardItemAttribute>();

                var desc = new ItemDescription
                {
                    Name = actionAttr?.DisplayName ?? type.Name
                };

                var props = type.GetProperties();
                for (int i = 0; i < props.Length; i++)
                {
                    var prop = props[i];
                    var keyAttr = prop.GetCustomAttribute<BlackboardPropertyAttribute>();

                    if (keyAttr != null)
                    {
                        var key = new KeyDescription(keyAttr.Name ?? prop.Name, prop.Name, keyAttr.Type, keyAttr.CanChangeType);

                        if (keyAttr.Usage == BlackboardKeyUsage.Input)
                        {
                            desc.Input.Add(key);
                        }
                        else
                        {
                            desc.Output.Add(key);
                        }
                    }
                }

                descriptions.Add(type, desc);

                return desc;
            }

            return description;
        }


        public static List<ViewModel.BlackboardItemReferenceViewModel> GetAvailableItems<T>()
        {
            var result = new List<ViewModel.BlackboardItemReferenceViewModel>();
            var ourType = typeof(T);

            var types = ourType.Assembly.GetTypes();

            for (int i = 0; i < types.Length; i++)
            {
                var type = types[i];
                if (type.IsClass && !type.IsAbstract && ourType.IsAssignableFrom(type))
                {
                    result.Add(GetReference(type));
                }
            }

            return result;
        }
    }
}
