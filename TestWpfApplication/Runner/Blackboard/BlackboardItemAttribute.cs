using System;

namespace TestWpfApplication.Runner.Blackboard
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class BlackboardItemAttribute : Attribute
    {
        public string DisplayName { get; }

        public BlackboardItemAttribute(string displayName)
        {
            DisplayName = displayName;
        }

    }
}
