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
            throw new NotImplementedException();
        }


        public double CreateChartedScore(Entities.ExerciseToSave exercise)
        {
            return 0;
        }
    }
}
