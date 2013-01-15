using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WorkoutPicker.Strategy
{
    class MostRepsForTimeStrategy : ITemplateStrategy
    {
        public System.Windows.Controls.StackPanel Build()
        {
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock repsText = new TextBlock() { Text = "Reps: ", Width = 100 };
            TextBox repsBox = new TextBox() { Width = 100, Name="Reps" };
            panel.Children.Add(repsText);
            panel.Children.Add(repsBox);
            return panel;
        }
    }
}
