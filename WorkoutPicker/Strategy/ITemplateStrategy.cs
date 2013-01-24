using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using WorkoutPicker.Entities;

namespace WorkoutPicker.Strategy
{
    public interface ITemplateStrategy
    {
        StackPanel Build();
        dynamic CreateBestScore(ExerciseToSave exercise);
        BestExercise CompareExercisesByTopScore(BestExercise oldExercise, BestExercise newExercise);
        Paragraph BuildParagraph(BestExercise exercise);
    }
}
