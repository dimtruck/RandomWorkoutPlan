using Newtonsoft.Json;
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

namespace WorkoutPicker.Entities
{
    /// <summary>
    /// Load once from JSON file
    /// </summary>
    public class ExerciseList
    {
        private static IDictionary<string, IList<IExercise>> _exerciseList = new Dictionary<string, IList<IExercise>>();
        
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
            return new ObservableCollection<WeatherSetting>()
            {
                new WeatherSetting { NumberOfExercises=5, WeatherType = "HOT", Weight=1},
                new WeatherSetting(){ NumberOfExercises=5, WeatherType = "NORMAL", Weight=1},
                new WeatherSetting(){ NumberOfExercises=5, WeatherType = "COLD", Weight=1},
                new WeatherSetting(){ NumberOfExercises=5, WeatherType = "SNOW", Weight=1},
                new WeatherSetting(){ NumberOfExercises=5, WeatherType = "RAIN", Weight=1}
            };
        }

    }
}
