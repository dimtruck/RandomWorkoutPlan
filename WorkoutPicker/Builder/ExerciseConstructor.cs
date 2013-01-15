using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPicker.Entities;

namespace WorkoutPicker.Builder
{
    public class ExerciseConstructor
    {
        public static IList<IExercise> Construct(IExerciseBuilder builder)
        {
            return builder.BuildExercises();
        }
    }
}
