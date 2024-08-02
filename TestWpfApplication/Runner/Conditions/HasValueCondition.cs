using TestWpfApplication.Runner.Blackboard;

namespace TestWpfApplication.Runner.Conditions
{
    [BlackboardItem("Has Value")]
    class HasValueCondition : IBlackboardCondition
    {
        [BlackboardProperty(BlackboardKeyType.Object)]
        public BlackboardKey Key { get; set; }

        public Task<bool> Evaluate(Blackboard.Blackboard blackboard)
            => Task.FromResult(blackboard.GetObject(Key) != null);

    }
}
