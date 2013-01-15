using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPicker.Entities
{
    public interface IExercise
    {
        int Id { get; set; }
        String Name { get; set; }
        String Output();
        ExerciseType ExerciseType { get; set; }
    }
}
