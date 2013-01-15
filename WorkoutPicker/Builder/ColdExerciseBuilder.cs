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
    public class ColdExerciseBuilder : ExerciseAbstractBuilder
    {
        /// <summary>
        /// this will build RunExercise
        /// </summary>
        /// <returns></returns>
        public override IList<Entities.IExercise> BuildExercises()
        {
            return BuildCrossfitIndoorExercises().
                Union(BuildPowerliftingExercises()).
                Union(BuildStrongmanExercises()).ToList();
        }
    }
}
