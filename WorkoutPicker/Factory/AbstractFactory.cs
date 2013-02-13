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
            IList<BestExercise> bestExerciseList = Entities.ExerciseList.CompileBestExerciseList();
            int ticks = (int)(DateTime.Now.Ticks % 32676);
            Random r = new Random(ticks);
            for (int i = 0; i < exerciseCount; i++)
            {
                IExercise pickedExercise = exerciseList[r.Next(exerciseList.Count)];
                StackPanel newPanel = new StackPanel() { Margin = new System.Windows.Thickness(5), Name = pickedExercise.Name.Replace(' ', '_') };
                newPanel.Orientation = Orientation.Horizontal;
                TextBlock exerciseName = new TextBlock() { Text = pickedExercise.Name, Width = 100, Name = "Id_" + pickedExercise.Id.ToString(), FontSize = 12 };
                TextBlock exerciseOutput = new TextBlock() { Text = pickedExercise.Output, Width = 220, FontSize = 12 };
                TextBlock exerciseType = new TextBlock() { Text = pickedExercise.ExerciseType.ToString().Replace('_', ' '), Width = 200, FontSize = 12 };
                String bestExerciseString = "Not yet completed/doesn't apply";
                BestExercise bestExercise = bestExerciseList.FirstOrDefault(t => t.Id == pickedExercise.Id);
                if (bestExercise != null)
                    bestExerciseString = bestExercise.BestScore + " completed on " + bestExercise.Date.ToString();
                TextBlock bestExerciseOutput = new TextBlock() { Text = bestExerciseString, TextWrapping= System.Windows.TextWrapping.Wrap, Width= 200, FontSize = 12 };

                newPanel.Children.Add(exerciseName);
                newPanel.Children.Add(exerciseOutput);
                newPanel.Children.Add(exerciseType);

                if (_strategyDictionary != null && _strategyDictionary.ContainsKey(pickedExercise.ExerciseType))
                    newPanel.Children.Add(new TemplateContext(_strategyDictionary[pickedExercise.ExerciseType]).Build());
                newPanel.Children.Add(bestExerciseOutput);
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
