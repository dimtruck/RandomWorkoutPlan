using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WorkoutPicker.Strategy
{
    class LongestForWeightStrategy : ITemplateStrategy
    {
        public System.Windows.Controls.StackPanel Build()
        {
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock timeText = new TextBlock() { Text = "Time: ", Width = 100 };
            TextBox timeBox = new TextBox() { Width = 20, Name = "Time" };
            TextBlock weightText = new TextBlock() { Text = "Weight: ", Width = 100 };
            TextBox weightBox = new TextBox() { Width = 30, Name = "Weight" };
            panel.Children.Add(timeText);
            panel.Children.Add(timeBox);
            panel.Children.Add(weightText);
            panel.Children.Add(weightBox);
            return panel;
        }
    }
}
