using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WorkoutPicker.Strategy
{
    class FastestForDistanceStrategy : ITemplateStrategy
    {
        public System.Windows.Controls.StackPanel Build()
        {
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock timeText = new TextBlock() { Text = "Time: ", Width = 100 };
            TextBox timeBox = new TextBox() { Width = 100, Name = "Time" };
            panel.Children.Add(timeText);
            panel.Children.Add(timeBox);
            return panel;
        }
    }
}
