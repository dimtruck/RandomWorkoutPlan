using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WorkoutPicker.Entities;
using WorkoutPicker.Strategy;

namespace WorkoutPicker.Factory
{
    public interface IFactory
    {
        IList<IExercise> CreateList();

        IFactory SetStrategyDictionary(IDictionary<ExerciseType, ITemplateStrategy> strategyDictionary);

        void BuildStackPanel(StackPanel panel, IList<string> selectedEquipmentList);
    }
}
