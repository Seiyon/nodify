using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TestWpfApplication.Runner.Blackboard;

namespace TestWpfApplication.Runner
{
    public class Transition
    {
        public Guid From { get; }
        public Guid To { get; }
        public IBlackboardCondition? Condition { get; }

        public virtual Task<bool> CanActivate(Blackboard.Blackboard blackboard) 
            => Condition?.Evaluate(blackboard) ?? Task.FromResult(false);

        public Transition(Guid from, Guid to, IBlackboardCondition? condition = default) {
            From = from;
            To = to;
            Condition = condition;
        }

    }
}
