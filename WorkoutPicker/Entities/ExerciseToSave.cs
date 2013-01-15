using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPicker.Entities
{
    class ExerciseToSave
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TimeSpan Time { get; set; }

        public float Distance { get; set; }

        public int Reps { get; set; }

        public int Sets { get; set; }

        public float Weight { get; set; }

        public DateTime DateToSave { get; set; }
    }
}
