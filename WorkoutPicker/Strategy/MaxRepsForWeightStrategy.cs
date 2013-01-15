using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WorkoutPicker.Strategy
{
    public class MaxRepsForWeightStrategy : ITemplateStrategy
    {
        public System.Windows.Controls.StackPanel Build()
        {
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock repsText = new TextBlock() { Text = "Reps: ", Width = 100 };
            TextBox repsBox = new TextBox() { Width = 20, Name="Reps" };
            TextBlock weightText = new TextBlock() { Text = "Weight: ", Width = 100 };
            TextBox weightBox = new TextBox() { Width = 30, Name="Weight" };
            panel.Children.Add(repsText);
            panel.Children.Add(repsBox);
            panel.Children.Add(weightText);
            panel.Children.Add(weightBox);
            return panel;
        }
    }
}
