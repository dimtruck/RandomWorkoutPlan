using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPicker.Entities;

namespace WorkoutPicker.Command
{
    class GetSavedExerciseCommand : ICommand
    {
        List<ExerciseToSave> exerciseList = new List<ExerciseToSave>();

        public GetSavedExerciseCommand(List<ExerciseToSave> exerciseList)
        {
            this.exerciseList = exerciseList;
        }

        public void Execute()
        {
           
            //show all workouts here
            using (TextReader writer = new StreamReader("exercises.json"))
            using (JsonTextReader jsonWriter = new JsonTextReader(writer))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
                exerciseList.AddRange(serializer.Deserialize<IList<ExerciseToSave>>(jsonWriter));
            }
        }

        public void UnExecute()
        {
            throw new NotImplementedException();
        }
    }
}
