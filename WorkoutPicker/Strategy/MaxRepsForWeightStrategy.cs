using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WorkoutPicker.Strategy
{
    public class MaxRepsForWeightStrategy : ITemplateStrategy
    {
        public System.Windows.Controls.StackPanel Build()
        {
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal, Width = 200 };
            TextBlock repsText = new TextBlock() { Text = "Reps: ", Width = 60, FontSize = 12 };
            TextBox repsBox = new TextBox() { Width = 40, Name = "Reps", FontSize = 12 };
            TextBlock weightText = new TextBlock() { Text = "Weight: ", Width = 55, FontSize = 12 };
            TextBox weightBox = new TextBox() { Width = 45, Name = "Weight", FontSize = 12 };
            panel.Children.Add(repsText);
            panel.Children.Add(repsBox);
            panel.Children.Add(weightText);
            panel.Children.Add(weightBox);
            return panel;
        }


        public dynamic CreateBestScore(Entities.ExerciseToSave exercise)
        {
            return new { Reps = exercise.Reps, Weight = exercise.Weight };
        }


        public Entities.BestExercise CompareExercisesByTopScore(Entities.BestExercise oldExercise, Entities.BestExercise newExercise)
        {
            if ((newExercise.BestScore.Weight > oldExercise.BestScore.Weight) ||
                        (newExercise.BestScore.Weight == oldExercise.BestScore.Weight && newExercise.BestScore.Reps > oldExercise.BestScore.Reps))
                return newExercise;
            else
                return oldExercise;

        }


        public System.Windows.Documents.Paragraph BuildParagraph(Entities.BestExercise exercise)
        {
            return new Paragraph(new Run(exercise.BestScore.Weight + " for " + exercise.BestScore.Reps));
        }


        public double CreateChartedScore(Entities.ExerciseToSave exercise)
        {
            return exercise.Reps * exercise.Weight;
        }
    }
}
