using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WorkoutPicker.Strategy
{
    class MostRepsForTimeStrategy : ITemplateStrategy
    {
        public System.Windows.Controls.StackPanel Build()
        {
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal, Width = 200 };
            TextBlock repsText = new TextBlock() { Text = "Reps: ", Width = 100, FontSize = 12 };
            TextBox repsBox = new TextBox() { Width = 100, Name = "Reps", FontSize = 12 };
            panel.Children.Add(repsText);
            panel.Children.Add(repsBox);
            return panel;
        }


        public dynamic CreateBestScore(Entities.ExerciseToSave exercise)
        {
            return exercise.Reps;
        }


        public Entities.BestExercise CompareExercisesByTopScore(Entities.BestExercise oldExercise, Entities.BestExercise newExercise)
        {
            if (newExercise.BestScore > oldExercise.BestScore)
                return newExercise;
            else
                return oldExercise;
        }


        public System.Windows.Documents.Paragraph BuildParagraph(Entities.BestExercise exercise)
        {
            return new Paragraph(new Run(exercise.BestScore.ToString()));
        }


        public double CreateChartedScore(Entities.ExerciseToSave exercise)
        {
            return exercise.Reps;
        }
    }
}
