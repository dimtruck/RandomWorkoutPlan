using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPicker.Factory;

namespace WorkoutPicker.Entities
{
    /// <summary>
    /// Load once from JSON file
    /// </summary>
    public class ExerciseList
    {
        private static IDictionary<string,IList<IExercise>> _exerciseList = new Dictionary<string,IList<IExercise>>();

        private ExerciseList()
        {
            IList<IFactory> factoryList = new List<IFactory>() { new HotFactory() };

            foreach (IFactory item in factoryList)
                _exerciseList[item.GetType().Name] = item.CreateList();
        }

        public static IDictionary<string, IList<IExercise>> GetExerciseList()
        {
            if (_exerciseList.Count == 0)
                new ExerciseList();

            return _exerciseList;

        }
    }
}
