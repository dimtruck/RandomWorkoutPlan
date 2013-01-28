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
    public abstract class AbstractFactory: IFactory
    {
        protected IDictionary<ExerciseType, ITemplateStrategy> _strategyDictionary;

        public void BuildStackPanel(StackPanel panel, IList<IExercise> exerciseList, int exerciseCount)
        {
            int ticks = (int)(DateTime.Now.Ticks % 32676);
            Random r = new Random(ticks);
            for (int i = 0; i < exerciseCount; i++)
            {
                IExercise pickedExercise = exerciseList[r.Next(exerciseList.Count)];
                StackPanel newPanel = new StackPanel() { Name = pickedExercise.Name.Replace(' ', '_') };
                newPanel.Orientation = Orientation.Horizontal;
                TextBlock exerciseName = new TextBlock() { Text = pickedExercise.Name, Width = 100, Name = "Id_" + pickedExercise.Id.ToString() };
                TextBlock exerciseOutput = new TextBlock() { Text = pickedExercise.Output, Width = 200 };
                TextBlock exerciseType = new TextBlock() { Text = pickedExercise.ExerciseType.ToString().Replace('_', ' '), Width = 200 };

                newPanel.Children.Add(exerciseName);
                newPanel.Children.Add(exerciseOutput);
                newPanel.Children.Add(exerciseType);

                if (_strategyDictionary != null && _strategyDictionary.ContainsKey(pickedExercise.ExerciseType))
                    newPanel.Children.Add(new TemplateContext(_strategyDictionary[pickedExercise.ExerciseType]).Build());
                panel.Children.Add(newPanel);
            }
        }

        public abstract IList<IExercise> CreateList();

        public IFactory SetStrategyDictionary(IDictionary<ExerciseType, ITemplateStrategy> strategyDictionary)
        {
            _strategyDictionary = strategyDictionary;
            return this;
        }

        public abstract void BuildStackPanel(StackPanel panel);
    }
}
