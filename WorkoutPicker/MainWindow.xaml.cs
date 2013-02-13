﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using WorkoutPicker.Entities;
using WorkoutPicker.Factory;
using WorkoutPicker.Strategy;
using System.Linq;
using WorkoutPicker.Command;
using System.Collections.ObjectModel;
using WorkoutPicker.Utils;

namespace WorkoutPicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDictionary<ExerciseType, ITemplateStrategy> templateDictionary = Entities.ExerciseList.SetupTemplateDictionary();
        private readonly IDictionary<String, IFactory> factoryDictionary = Entities.ExerciseList.SetupFactoryDictionary();
        private readonly IDictionary<String, IList<IExercise>> exerciseDictionary = Entities.ExerciseList.GetExerciseList();
        private ObservableCollection<WeatherSetting> _weatherTypeSettingCollection = Entities.ExerciseList.SetupWeatherSettingList();
        private ObservableCollection<IExercise> _exerciseCollection = new ObservableCollection<IExercise>(Entities.ExerciseList.ExerciseListUnique());
        private ObservableCollection<BestExercise> _bestExerciseCollection = new ObservableCollection<BestExercise>();

        Random r = new Random(352333);
        public MainWindow()
        {
            InitializeComponent();
            Exercise.Text = "Exercises for " + DateTime.Today;
            //here combine by id, get type for each id and based on type, look at highest value
            IList<BestExercise> bestExerciseList = Entities.ExerciseList.CompileBestExerciseList();
            _bestExerciseCollection.Clear();
            foreach (var item in bestExerciseList)
                _bestExerciseCollection.Add(item);
            this.DataContext = this;
        }

        public ObservableCollection<WeatherSetting> WeatherTypeSettingCollection { get { return _weatherTypeSettingCollection; } }

        public ObservableCollection<IExercise> ExerciseCollection { get { return _exerciseCollection; } }

        public ObservableCollection<BestExercise> BestExerciseCollection { get { return _bestExerciseCollection; } }

        private void WeatherType_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ExerciseList.Children.RemoveRange(0, ExerciseList.Children.Count);

            if (WeatherType.SelectedItem != null)
            {
                String selectedItem = ((System.Windows.Controls.ListBoxItem)WeatherType.SelectedItem).Content.ToString().ToUpper();
                if (factoryDictionary != null && factoryDictionary.ContainsKey(selectedItem))
                    factoryDictionary[selectedItem].SetStrategyDictionary(templateDictionary).BuildStackPanel(ExerciseList);
            }
        }

        private void Save_Click_1(object sender, RoutedEventArgs e)
        {
            ICommand command = new SaveCommand(ExerciseList);
            command.Execute();

            MessageBox.Show("Completion successful.", "This has been stored.  Ready for querying!!");
            WeatherType.SelectedItem = null;
        }
    }

    public enum ExerciseType
    {
        NONE,
        MAX_REPS_PER_WEIGHT,
        MAX_WEIGHT,
        FASTEST_FOR_DISTANCE,
        COMPLETION,
        FASTEST_FOR_REPS,
        LONGEST_FOR_WEIGHT,
        MOST_REPS_FOR_TIME
    }


    public enum WeatherSettingEnum
    {
        HOT,
        COLD,
        RAIN,
        NORMAL,
        SNOW
    }
}
