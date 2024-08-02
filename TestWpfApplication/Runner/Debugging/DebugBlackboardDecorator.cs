using TestWpfApplication.Runner.Blackboard;

namespace TestWpfApplication.Runner.Debugging
{
    public class DebugBlackboardDecorator : Blackboard.Blackboard
    {
        public static BlackboardKey StateDelayKey { get; } = "__state.delay";
        public static BlackboardKey TransitionDelayKey { get; } = "__transition.delay";
        private Blackboard.Blackboard? _blackboard;
        public event Action<BlackboardKey, object?>? ValueChanged;

        public DebugBlackboardDecorator(Blackboard.Blackboard? blackboard = default)
        {
            Attach(blackboard);
        }

        public override IReadOnlyCollection<BlackboardKey> Keys => _blackboard?.Keys ?? Array.Empty<BlackboardKey>();

        public override void Remove(BlackboardKey key)
        {
            _blackboard?.Remove(key);
        }

        public override void Clear()
        {
            _blackboard?.Clear();
        }

        public override T? GetObject<T>(BlackboardKey key) where T : class
        {
            return _blackboard?.GetObject<T>(key);
        }

        public override object? GetObject(BlackboardKey key)
        {
            return _blackboard?.GetObject(key);
        }

        public override T? GetValue<T>(BlackboardKey key)
        {
            return _blackboard?.GetValue<T>(key);
        }

        public override void Set(BlackboardKey key, object? value)
        {
            _blackboard?.Set(key, value);
            ValueChanged?.Invoke(key, value);
        }

        public override bool HasKey(BlackboardKey key)
        {
            return _blackboard?.HasKey(key) ?? false;
        }

        public virtual void Attach(Blackboard.Blackboard? blackboard)
        {
            _blackboard = blackboard;
            Set(StateDelayKey, 100);
        }
    }
}
