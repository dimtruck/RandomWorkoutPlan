using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPicker.Entities
{
    public class BestExercise
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public dynamic BestScore { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public ExerciseType ExerciseType { get; set; }
        public string Combination { get; set; }
    }
}
