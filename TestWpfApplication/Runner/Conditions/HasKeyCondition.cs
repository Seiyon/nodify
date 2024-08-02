using TestWpfApplication.Runner.Blackboard;

namespace TestWpfApplication.Runner.Conditions
{
    [BlackboardItem("Has Key")]
    class HasKeyCondition : IBlackboardCondition
    {
        [BlackboardProperty("Key Name", BlackboardKeyType.String)]
        public BlackboardProperty Key { get; set; }

        public Task<bool> Evaluate(Blackboard.Blackboard blackboard)
        {
            var keyName = blackboard.GetObject<string>(Key);

            if (keyName != null)
            {
                return Task.FromResult(blackboard.HasKey(keyName));
            }

            return Task.FromResult(false);
        }
    }
}
