using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WorkoutPicker.Strategy
{
    class CompletionStrategy : ITemplateStrategy
    {
        public System.Windows.Controls.StackPanel Build()
        {
            return new StackPanel();
        }
    }
}
