using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WorkoutPicker.Strategy
{
    public class NoneStrategy : ITemplateStrategy
    {
        public System.Windows.Controls.StackPanel Build()
        {
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal };
            return panel;
        }
    }
}
