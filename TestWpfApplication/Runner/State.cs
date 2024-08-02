using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWpfApplication.Runner.Blackboard;

namespace TestWpfApplication.Runner
{
    public class State
    {
        public Guid Id { get; }

        public IBlackboardAction? Action { get; }

        public IReadOnlyList<Transition> Transitions { get; }

        public virtual Task Activate(Blackboard.Blackboard blackboard) { return Action?.Execute(blackboard) ?? Task.CompletedTask; }

        public State(Guid id, IEnumerable<Transition> transitions, IBlackboardAction? action = default)
        {
            Id = id;
            Action = action;
            Transitions = new List<Transition>(transitions);
        }
    }
}
