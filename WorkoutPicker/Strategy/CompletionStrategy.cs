using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WorkoutPicker.Strategy
{
    class CompletionStrategy : ITemplateStrategy
    {
        public System.Windows.Controls.StackPanel Build()
        {
            return new StackPanel() { Orientation = Orientation.Horizontal, Width = 200 };

        }


        public dynamic CreateBestScore(Entities.ExerciseToSave exercise)
        {
            return "";
        }


        public Entities.BestExercise CompareExercisesByTopScore(Entities.BestExercise oldExercise, Entities.BestExercise newExercise)
        {
            return oldExercise;
        }


        public System.Windows.Documents.Paragraph BuildParagraph(Entities.BestExercise exercise)
        {
            return new Paragraph(new Run(""));
        }


        public double CreateChartedScore(Entities.ExerciseToSave exercise)
        {
            return 0;
        }
    }
}
