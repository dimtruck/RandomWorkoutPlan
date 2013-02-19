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
        String Output { get; }
        ExerciseType ExerciseType { get; set; }
        String ExerciseTypeString { get; }
        String Result { get; }
        String Description { get; set; }
        float ExerciseWeight { get; set; }
    }
}
