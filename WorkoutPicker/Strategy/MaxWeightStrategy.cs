using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WorkoutPicker.Strategy
{
    class MaxWeightStrategy : ITemplateStrategy
    {
        public System.Windows.Controls.StackPanel Build()
        {
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal, Width = 200 };
            TextBlock weightText = new TextBlock() { Text = "Weight: ", Width = 100, FontSize = 12 };
            TextBox weightBox = new TextBox() { Width = 100, Name = "Weight", FontSize = 12 };
            panel.Children.Add(weightText);
            panel.Children.Add(weightBox);
            return panel;
        }


        public dynamic CreateBestScore(Entities.ExerciseToSave exercise)
        {
            return exercise.Weight;
        }


        public Entities.BestExercise CompareExercisesByTopScore(Entities.BestExercise oldExercise, Entities.BestExercise newExercise)
        {
            if (newExercise.BestScore > oldExercise.BestScore)
                return newExercise;
            else
                return oldExercise;
        }


        public Paragraph BuildParagraph(Entities.BestExercise exercise)
        {
            return new Paragraph(new Run(exercise.BestScore.ToString()));
        }


        public double CreateChartedScore(Entities.ExerciseToSave exercise)
        {
            return exercise.Weight;
        }
    }
}
