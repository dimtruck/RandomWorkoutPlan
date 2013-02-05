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
    public class SnowFactory : AbstractFactory
    {
        public override IList<IExercise> CreateList()
        {
            return ExerciseList.GetExerciseList()[typeof(SnowExerciseBuilder).Name];
        }


        public override void BuildStackPanel(System.Windows.Controls.StackPanel panel)
        {
            base.BuildStackPanel(panel, CreateList(), 
                ExerciseList.SetupWeatherSettingList().FirstOrDefault(t => t.WeatherType.Equals("SNOW")).NumberOfExercises);
        }
    }
}
