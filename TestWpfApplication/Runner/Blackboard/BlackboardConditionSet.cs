using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestWpfApplication.Runner.Blackboard
{
    public enum BooleanOperator
    {
        And,
        Or,
    }

    public class BlackboardConditionSet : IBlackboardCondition
    {
        public IReadOnlyList<IBlackboardCondition> Conditions { get;}
        public BooleanOperator Operator { get; set; }

        public BlackboardConditionSet(IEnumerable<IBlackboardCondition> conditions, BooleanOperator op)
        {
            Conditions = new List<IBlackboardCondition>(conditions);
            Operator = op;
        }

        public async Task<bool> Evaluate(Blackboard blackboard)
        {
            bool result = false;

            if(Operator == BooleanOperator.And)
            {
                foreach(var condition in Conditions)
                {
                    result &= await condition.Evaluate(blackboard);
                }
            }
            else if(Operator == BooleanOperator.Or)
            {
                foreach(var condition in Conditions)
                {
                    result |= await condition.Evaluate(blackboard);
                }
            }

            return result;
        }
    }
}
