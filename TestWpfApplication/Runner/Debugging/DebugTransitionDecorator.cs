using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWpfApplication.Runner.Debugging
{
    public class DebugTransitionDecorator : Transition
    {
        private readonly Transition transition;

        public DebugTransitionDecorator(Transition transition) : base(transition.From, transition.To)
        {
            this.transition = transition;
        }

        public override async Task<bool> CanActivate(Blackboard.Blackboard blackboard)
        {
            int? delay = blackboard.GetValue<int>(DebugBlackboardDecorator.TransitionDelayKey);

            await Task.Delay(Math.Max(10, delay ?? 10));

            return await transition.CanActivate(blackboard);

        }
    }
}
