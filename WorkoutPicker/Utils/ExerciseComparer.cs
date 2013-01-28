using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPicker.Entities;

namespace WorkoutPicker.Utils
{
    class ExerciseComparer : IEqualityComparer<IExercise>
    {
        public bool Equals(IExercise x, IExercise y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(IExercise obj)
        {
            return obj.GetHashCode();
        }
    }
}
