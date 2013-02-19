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
        protected IList<int> _allowedExerciseList;

        public void BuildStackPanel(StackPanel panel, IList<IExercise> exerciseList, int exerciseCount)
        {
            int ticks = (int)(DateTime.Now.Ticks % 32676);
            Random r = new Random(ticks);
            for (int i = 0; i < exerciseCount; i++)
            {
                bool isValid = false;
                IExercise pickedExercise = null;
                while (!isValid)
                {
                    pickedExercise = exerciseList[r.Next(exerciseList.Count)];
                    isValid = true;
                    if (_allowedExerciseList != null && _allowedExerciseList.Count > 0)
                        isValid = _allowedExerciseList.Contains(pickedExercise.Id);
                }
                StackPanel newPanel = new StackPanel() { Margin = new System.Windows.Thickness(5), Name = pickedExercise.Name.Replace(' ', '_') };
                newPanel.Orientation = Orientation.Horizontal;
                TextBlock exerciseName = new TextBlock() { Text = pickedExercise.Name, Width = 100, Name = "Id_" + pickedExercise.Id.ToString(), FontSize = 12};
                TextBlock exerciseOutput = new TextBlock() { Text = pickedExercise.Output, Width = 220, FontSize = 12 };
                TextBlock exerciseType = new TextBlock() { Text = pickedExercise.ExerciseTypeString, Width = 200, FontSize = 12 };
                TextBlock bestExerciseOutput = new TextBlock() { Text = pickedExercise.Result, TextWrapping= System.Windows.TextWrapping.Wrap, Margin= new System.Windows.Thickness(10,0,0,0), Width= 200, FontSize = 12 };

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

        public virtual void BuildStackPanel(StackPanel panel, IList<string> allowedEquipmentList)
        {
            if (allowedEquipmentList != null && allowedEquipmentList.Count > 0)
            {
                //generate exercise list that's allowed
                IList<Equipment> equipmentList = ExerciseList.RetrieveEquipment();
                List<int> temp = new List<int>();
                foreach(Equipment e in equipmentList.Where(t => allowedEquipmentList.Contains(t.Name)))
                    temp.AddRange(e.Exercises);
                temp.AddRange(equipmentList.First(t => t.Name.Equals("none")).Exercises);
                _allowedExerciseList = temp;
            }
            BuildStackPanel(panel);
        }
    }
}
