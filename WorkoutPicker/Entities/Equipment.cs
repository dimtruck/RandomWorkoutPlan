using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPicker.Entities
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<int> Exercises { get; set; }

        private string _flattenedExercises = string.Empty; 
        public string FlattenedExercises
        {
            get
            {
                if (String.IsNullOrEmpty(_flattenedExercises))
                {
                    IList<IExercise> exerciseList = ExerciseList.ExerciseListUnique();
                    IList<String> exerciseStringList = new List<String>();
                    foreach (int item in Exercises)
                        exerciseStringList.Add(exerciseList.First(t => t.Id == item).Name + " " + exerciseList.First(t => t.Id == item).ExerciseType + " " + exerciseList.First(t => t.Id == item).Output);
                    return string.Join("\r\n", exerciseStringList);
                }
                return string.Empty;
            }
        }

    }
}
