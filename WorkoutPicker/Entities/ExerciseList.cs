using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPicker.Builder;
using WorkoutPicker.Factory;
using WorkoutPicker.Strategy;
using WorkoutPicker.Utils;

namespace WorkoutPicker.Entities
{
    /// <summary>
    /// Load once from JSON file
    /// </summary>
    public class ExerciseList
    {
        private static IDictionary<string, IList<IExercise>> _exerciseList = new Dictionary<string, IList<IExercise>>();
        private static ObservableCollection<WeatherSetting> weatherSettingList = new ObservableCollection<WeatherSetting>();
        private static IList<IExercise> _exerciseListUnique = new List<IExercise>();
        
        private ExerciseList()
        {
            IList<IExerciseBuilder> builderList = new List<IExerciseBuilder>() { 
                new HotExerciseBuilder(), new ColdExerciseBuilder(), new NormalExerciseBuilder(), new RainExerciseBuilder(), new SnowExerciseBuilder() };

            foreach (IExerciseBuilder item in builderList)
                _exerciseList[item.GetType().Name] = item.BuildExercises();
        }

        public static IDictionary<string, IList<IExercise>> GetExerciseList()
        {
            if (_exerciseList.Count == 0)
                new ExerciseList();

            return _exerciseList;

        }

        public static IDictionary<String, IFactory> SetupFactoryDictionary()
        {
            IDictionary<String, IFactory> factoryDictionary = new Dictionary<String, IFactory>();
            factoryDictionary["NORMAL"] = new NormalFactory();
            factoryDictionary["COLD"] = new ColdFactory();
            factoryDictionary["RAIN"] = new RainFactory();
            factoryDictionary["SNOW"] = new SnowFactory();
            factoryDictionary["HOT"] = new HotFactory();

            return factoryDictionary;
        }

        public static IDictionary<ExerciseType, ITemplateStrategy> SetupTemplateDictionary()
        {
            IDictionary<ExerciseType, ITemplateStrategy> templateDictionary = new Dictionary<ExerciseType, ITemplateStrategy>();
            templateDictionary[ExerciseType.NONE] = new NoneStrategy();
            templateDictionary[ExerciseType.MAX_REPS_PER_WEIGHT] = new MaxRepsForWeightStrategy();
            templateDictionary[ExerciseType.FASTEST_FOR_REPS] = new FastestForRepsStrategy();
            templateDictionary[ExerciseType.COMPLETION] = new CompletionStrategy();
            templateDictionary[ExerciseType.FASTEST_FOR_DISTANCE] = new FastestForDistanceStrategy();
            templateDictionary[ExerciseType.MAX_WEIGHT] = new MaxWeightStrategy();
            templateDictionary[ExerciseType.LONGEST_FOR_WEIGHT] = new LongestForWeightStrategy();
            templateDictionary[ExerciseType.MOST_REPS_FOR_TIME] = new MostRepsForTimeStrategy();
            return templateDictionary;
        }

        public static IDictionary<ExerciseType, dynamic> SetupValueToCompareDictionary()
        {
            IDictionary<ExerciseType, dynamic> dictionary = new Dictionary<ExerciseType, dynamic>();
            dictionary[ExerciseType.FASTEST_FOR_DISTANCE] = "Time";
            dictionary[ExerciseType.FASTEST_FOR_REPS] = "Time";
            dictionary[ExerciseType.LONGEST_FOR_WEIGHT] = "Distance";
            dictionary[ExerciseType.MAX_REPS_PER_WEIGHT] = (new Dictionary<string,string>(){{"Weight","Reps"}});
            dictionary[ExerciseType.MAX_WEIGHT] = "Weight";
            dictionary[ExerciseType.MOST_REPS_FOR_TIME] = "Reps";
            return dictionary;
        }

        public static ObservableCollection<WeatherSetting> SetupWeatherSettingList()
        {
            if (weatherSettingList == null || weatherSettingList.Count == 0)
            {
                IList<WeatherSettingDTO> tempList = new List<WeatherSettingDTO>();
                using (StreamReader reader = File.OpenText("weather_setting.json"))
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    var serializer = new JsonSerializer();
                    tempList = serializer.Deserialize<ObservableCollection<WeatherSettingDTO>>(jsonReader);
                }

                foreach (var item in tempList)
                {
                    weatherSettingList.Add(new WeatherSetting
                    {
                        WeatherType = item.WeatherType,
                        NumberOfExercises = item.NumberOfExercises
                    });
                }

            }
            return weatherSettingList;
        }

        public static IList<IExercise> ExerciseListUnique()
        {
            if (_exerciseListUnique == null || _exerciseListUnique.Count == 0)
            {
                ISet<IExercise> tempList = new HashSet<IExercise>(new ExerciseComparer());
                foreach (var exerciseListDictionary in Entities.ExerciseList.GetExerciseList())
                {
                    IList<IExercise> exerciseList = exerciseListDictionary.Value;
                    foreach (var exercise in exerciseList)
                        tempList.Add(exercise);
                }
                _exerciseListUnique = tempList.ToList();
            }
            return _exerciseListUnique;
        }

        public static IList<BestExercise> CompileBestExerciseList()
        {
            IList<IExercise> tempList = ExerciseListUnique();
            IList<BestExercise> bestExerciseList = new List<BestExercise>();
            IDictionary<ExerciseType, ITemplateStrategy> templateDictionary = SetupTemplateDictionary();
            foreach (ExerciseToSave item in ExerciseList.RetrieveSavedExercises())
            {
                if (bestExerciseList.FirstOrDefault(t => t.Id == item.Id) != null)
                {
                    //already exists
                    BestExercise currentBestExercise = bestExerciseList.FirstOrDefault(t => t.Id == item.Id);
                    BestExercise bestExercise = templateDictionary[item.ExerciseType].CompareExercisesByTopScore(currentBestExercise, new BestExercise()
                    {
                        Combination = tempList.First(t => t.Id == item.Id).Output,
                        ExerciseType = item.ExerciseType,
                        Name = item.Name,
                        BestScore = templateDictionary[item.ExerciseType].CreateBestScore(item),
                        Date = item.DateToSave,
                        Id = item.Id,
                        Count = 1
                    });
                    bestExercise.Count = currentBestExercise.Count + 1;
                    bestExerciseList.Remove(currentBestExercise);
                    bestExerciseList.Add(bestExercise);
                }
                else
                {
                    //doesn't exist
                    bestExerciseList.Add(new BestExercise()
                    {
                        Combination = tempList.First(t => t.Id == item.Id).Output,
                        ExerciseType = item.ExerciseType,
                        Name = item.Name,
                        BestScore = templateDictionary[item.ExerciseType].CreateBestScore(item),
                        Date = item.DateToSave,
                        Id = item.Id,
                        Count = 1
                    });
                }
            }
            return bestExerciseList;
        }

        public static IList<ExerciseToSave> RetrieveSavedExercises()
        {
            IList<ExerciseToSave> exerciseList = new List<ExerciseToSave>();
            //show all workouts here
            using (TextReader writer = new StreamReader("exercises.json"))
            using (JsonTextReader jsonWriter = new JsonTextReader(writer))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
                exerciseList = serializer.Deserialize<IList<ExerciseToSave>>(jsonWriter);
            }
            return exerciseList;
        }

        public static IList<Equipment> RetrieveEquipment()
        {
            IList<Equipment> equipmentList = new List<Equipment>();
            using (TextReader writer = new StreamReader("equipment.json"))
            using (JsonTextReader jsonWriter = new JsonTextReader(writer))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
                equipmentList = serializer.Deserialize<IList<Equipment>>(jsonWriter);
            }
            return equipmentList;
        }
    }
}
