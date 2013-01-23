using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WorkoutPicker.Builder;
using WorkoutPicker.Entities;
using WorkoutPicker.Strategy;

namespace WorkoutPicker.Factory
{
    public class RainFactory : AbstractFactory
    {
        private readonly int _exerciseCount = 5;

        public override IList<IExercise> CreateList()
        {
            return ExerciseList.GetExerciseList()[typeof(RainExerciseBuilder).Name];
        }


        public override void BuildStackPanel(System.Windows.Controls.StackPanel panel)
        {
            base.BuildStackPanel(panel, CreateList(), _exerciseCount);
        }
    }
}
