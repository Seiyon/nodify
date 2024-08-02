using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWpfApplication.Runner.Blackboard
{
    public interface IBlackboardAction
    {
        Task Execute(Blackboard blackboard);
    }
}
