using System;


namespace TestWpfApplication.Runner.Blackboard
{
    public enum BlackboardKeyUsage
    {
        Input,
        Output
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class BlackboardPropertyAttribute : Attribute
    {
        public string? Name { get; }
        public BlackboardKeyType Type { get; }
        public BlackboardKeyUsage Usage { get; set; }

        public bool CanChangeType { get; set; }

        public BlackboardPropertyAttribute(string? name, BlackboardKeyType type = BlackboardKeyType.Object)
        {
            Name = name;
        }

        public BlackboardPropertyAttribute(BlackboardKeyType type = BlackboardKeyType.Object) : this(null, type)
        {

        }

    }
}
