using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPicker.Entities
{
    public interface IPowerlifting : IExercise
    {
        TimeSpan Time { get; set; }
        float Distance { get; set; }
        int Reps { get; set; }
        int Sets { get; set; }
        float Weight { get; set; }
    }
}
