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
    public class NormalExerciseBuilder : ExerciseAbstractBuilder
    {
        /// <summary>
        /// this will build RunExercise
        /// </summary>
        /// <returns></returns>
        public override IList<Entities.IExercise> BuildExercises()
        {
            return BuildAerobicExercises().
                Union(BuildCrossfitIndoorExercises()).
                Union(BuildCrossfitOutdoorExercises()).
                Union(BuildPowerliftingExercises()).
                Union(BuildStrongmanExercises()).ToList();
        }
    }
}
