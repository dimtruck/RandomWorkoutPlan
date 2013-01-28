using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WorkoutPicker.Entities;

namespace WorkoutPicker.Command
{
    class SaveCommand : ICommand
    {
        private StackPanel _exerciseList;

        public SaveCommand(StackPanel exerciseList)
        {
            this._exerciseList = exerciseList;
        }

        public void Execute()
        {
            IList<ExerciseToSave> exerciseListToSave = new List<ExerciseToSave>();
            using (TextReader writer = new StreamReader("exercises.json"))
            using (JsonTextReader jsonWriter = new JsonTextReader(writer))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
                exerciseListToSave = serializer.Deserialize<IList<ExerciseToSave>>(jsonWriter);
            }

            PopulateList(_exerciseList, exerciseListToSave);

            using (TextWriter writer = new StreamWriter("exercises.json"))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Serialize(jsonWriter, exerciseListToSave);
            }
        }

        public void UnExecute()
        {
            throw new NotImplementedException();
        }


        private void PopulateList(StackPanel exerciseList, IList<ExerciseToSave> exerciseListToSave)
        {
            foreach (StackPanel exercise in exerciseList.Children)
            {
                string exerciseName = ((TextBlock)exercise.Children[0]).Text;

                if (!"REST".Equals(exerciseName.ToUpper()))
                {
                    int exerciseId = Int32.Parse(((TextBlock)exercise.Children[0]).Name.Split('_')[1]);
                    ExerciseType exerciseType = (ExerciseType)Enum.Parse(typeof(ExerciseType), ((TextBlock)exercise.Children[2]).Text.Replace(' ', '_'));

                    ExerciseToSave exerciseToSave = new ExerciseToSave()
                    {
                        DateToSave = DateTime.Today,
                        Id = exerciseId,
                        Name = exerciseName,
                        ExerciseType = exerciseType
                    };

                    foreach (var item in ((StackPanel)exercise.Children[3]).Children)
                    {
                        if (item.GetType() == typeof(TextBox))
                        {
                            switch (((TextBox)item).Name)
                            {
                                case "Weight":
                                    exerciseToSave.Weight = String.IsNullOrEmpty(((TextBox)item).Text) ? 0 : Int32.Parse(((TextBox)item).Text);
                                    break;
                                case "Reps":
                                    exerciseToSave.Reps = String.IsNullOrEmpty(((TextBox)item).Text) ? 0 : Int32.Parse(((TextBox)item).Text);
                                    break;
                                case "Time":
                                    exerciseToSave.Time = String.IsNullOrEmpty(((TextBox)item).Text) ? new TimeSpan() : TimeSpan.Parse(((TextBox)item).Text);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    exerciseListToSave.Add(exerciseToSave);
                }
            }
        }

    }
}
