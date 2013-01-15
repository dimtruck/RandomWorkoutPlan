using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPicker.Entities;

namespace WorkoutPicker.Builder
{
    public interface IExerciseBuilder
    {
        IList<IExercise> BuildExercises();
    }
}
