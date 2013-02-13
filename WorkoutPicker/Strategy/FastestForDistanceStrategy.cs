using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WorkoutPicker.Strategy
{
    class FastestForDistanceStrategy : ITemplateStrategy
    {
        public System.Windows.Controls.StackPanel Build()
        {
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock timeText = new TextBlock() { Text = "Time: ", Width = 100, FontSize = 12 };
            TextBox timeBox = new TextBox() { Width = 100, Name = "Time", FontSize = 12 };
            panel.Children.Add(timeText);
            panel.Children.Add(timeBox);
            return panel;
        }


        public dynamic CreateBestScore(Entities.ExerciseToSave exercise)
        {
            return exercise.Time;
        }


        public Entities.BestExercise CompareExercisesByTopScore(Entities.BestExercise oldExercise, Entities.BestExercise newExercise)
        {
            if (newExercise.BestScore < oldExercise.BestScore)
                return newExercise;
            else
                return oldExercise;
        }


        public System.Windows.Documents.Paragraph BuildParagraph(Entities.BestExercise exercise)
        {
            return new Paragraph(new Run(exercise.BestScore.ToString()));
        }
    }
}
