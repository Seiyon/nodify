using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWpfApplication.Runner.Debugging
{
    public class DebugStateDecorator : State
    {
        private readonly State state;

        public DebugStateDecorator(State state) : base(state.Id, state.Transitions)
        {
            this.state = state;
        }

        public override async Task Activate(Blackboard.Blackboard blackboard)
        {
            int? delay = blackboard.GetValue<int>(DebugBlackboardDecorator.StateDelayKey);

            await Task.Delay(Math.Max(10, delay ?? 10));

            await state.Activate(blackboard);
        }

    }
}
