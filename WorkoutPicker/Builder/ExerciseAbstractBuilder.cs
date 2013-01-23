using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPicker.Entities;

namespace WorkoutPicker.Builder
{
    public abstract class ExerciseAbstractBuilder : IExerciseBuilder
    {
        private IList<IExercise> _exerciseList;

        public IList<IExercise> BuildAerobicExercises()
        {
            IList<IExercise> exerciseList = new List<IExercise>();
            using (StreamReader reader = File.OpenText("aerobic_exercises.json"))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                var serializer = new JsonSerializer();
                IList<AerobicExercise> aerobicExercises = serializer.Deserialize<IList<AerobicExercise>>(jsonReader);

                foreach (var item in aerobicExercises)
                    exerciseList.Add(item);
                return exerciseList;
            }
        }

        public IList<IExercise> BuildPowerliftingExercises()
        {
            IList<IExercise> exerciseList = new List<IExercise>();
            using (StreamReader reader = File.OpenText("powerlifting_exercises.json"))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                var serializer = new JsonSerializer();
                IList<PowerliftingExercise> powerliftingExercises = serializer.Deserialize<IList<PowerliftingExercise>>(jsonReader);

                foreach (var item in powerliftingExercises)
                    exerciseList.Add(item);
                return exerciseList;
            }
        }

        public IList<IExercise> BuildStrongmanExercises()
        {
            IList<IExercise> exerciseList = new List<IExercise>();
            using (StreamReader reader = File.OpenText("strongman_exercises.json"))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                var serializer = new JsonSerializer();
                IList<StrongmanExercise> strongmanExercises = serializer.Deserialize<IList<StrongmanExercise>>(jsonReader);

                foreach (var item in strongmanExercises)
                    exerciseList.Add(item);
                return exerciseList;
            }
        }

        public IList<IExercise> BuildCrossfitIndoorExercises()
        {
            IList<IExercise> exerciseList = new List<IExercise>();
            using (StreamReader reader = File.OpenText("xfit_indoor_exercises.json"))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                var serializer = new JsonSerializer();
                IList<CrossfitInsideExercise> powerliftingExercises = serializer.Deserialize<IList<CrossfitInsideExercise>>(jsonReader);

                foreach (var item in powerliftingExercises)
                    exerciseList.Add(item);
                return exerciseList;
            }
        }

        public IList<IExercise> BuildCrossfitOutdoorExercises()
        {
            return new List<IExercise>();
        }

        public abstract IList<IExercise> BuildExercises();
    }
}
