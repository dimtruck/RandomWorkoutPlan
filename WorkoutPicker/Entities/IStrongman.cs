using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPicker.Entities
{
    public interface IStrongman : IExercise
    {
        int Laps { get; set; }
        TimeSpan Time { get; set; }
    }
}
