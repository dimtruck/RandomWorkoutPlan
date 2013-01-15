using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPicker.Entities
{
    public interface ICrossfitInside : IExercise
    {
        TimeSpan Time { get; set; }
        int Sets { get; set; }
        int Reps { get; set; }
    }
}
