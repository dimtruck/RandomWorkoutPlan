using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using WorkoutPicker.Entities;
using WorkoutPicker.Factory;
using WorkoutPicker.Strategy;

namespace WorkoutPicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDictionary<ExerciseType, ITemplateStrategy> templateDictionary =
            new Dictionary<ExerciseType, ITemplateStrategy>();
        private readonly IDictionary<String, IFactory> factoryDictionary =
            new Dictionary<String, IFactory>();

        Random r = new Random(352333);
        public MainWindow()
        {
            SetupTemplateDictionary();
            SetupFactoryDictionary();

            InitializeComponent();
            Exercise.Text = "Exercises for " + DateTime.Today;
        }

        private void SetupTemplateDictionary()
        {
            templateDictionary[ExerciseType.NONE] = new NoneStrategy();
            templateDictionary[ExerciseType.MAX_REPS_PER_WEIGHT] = new MaxRepsForWeightStrategy();
            templateDictionary[ExerciseType.FASTEST_FOR_REPS] = new FastestForRepsStrategy();
            templateDictionary[ExerciseType.COMPLETION] = new CompletionStrategy();
            templateDictionary[ExerciseType.FASTEST_FOR_DISTANCE] = new FastestForDistanceStrategy();
            templateDictionary[ExerciseType.MAX_WEIGHT] = new MaxWeightStrategy();
            templateDictionary[ExerciseType.LONGEST_FOR_WEIGHT] = new LongestForWeightStrategy();
            templateDictionary[ExerciseType.MOST_REPS_FOR_TIME] = new MostRepsForTimeStrategy();
        }

        private void SetupFactoryDictionary()
        {
            factoryDictionary["NORMAL"] = new NormalFactory();
            factoryDictionary["COLD"] = new ColdFactory();
            factoryDictionary["RAIN"] = new RainFactory();
            factoryDictionary["SNOW"] = new SnowFactory();
            factoryDictionary["HOT"] = new HotFactory();
        }

        private void WeatherType_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ExerciseList.Children.RemoveRange(0, ExerciseList.Children.Count);

            String selectedItem = ((System.Windows.Controls.ListBoxItem)WeatherType.SelectedItem).Content.ToString().ToUpper();
            if (factoryDictionary != null && factoryDictionary.ContainsKey(selectedItem))
                factoryDictionary[selectedItem].SetStrategyDictionary(templateDictionary).BuildStackPanel(ExerciseList);
        }

        private void Save_Click_1(object sender, RoutedEventArgs e)
        {
            //store everything here when clicked
            IList<ExerciseToSave> exerciseListToSave = new List<ExerciseToSave>();
            PopulateList(exerciseListToSave);

            Store(exerciseListToSave);

            MessageBox.Show("Completion successful.", "This has been stored.  Ready for querying!!");
        }

        private static void Store(IList<ExerciseToSave> exerciseListToSave)
        {
            using (TextWriter writer = new StreamWriter("exercises.json", true))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
                foreach (var item in exerciseListToSave)
                    serializer.Serialize(jsonWriter, item);
            }
        }

        private void PopulateList(IList<ExerciseToSave> exerciseListToSave)
        {
            foreach (StackPanel exercise in ExerciseList.Children)
            {
                string exerciseName = ((TextBlock)exercise.Children[0]).Text;
                int exerciseId = Int32.Parse(((TextBlock)exercise.Children[0]).Name.Split('_')[1]);
                ExerciseType exerciseType = (ExerciseType)Enum.Parse(typeof(ExerciseType), ((TextBlock)exercise.Children[2]).Text.Replace(' ', '_'));


                foreach (var item in ((StackPanel)exercise.Children[3]).Children)
                {
                    if (item.GetType() == typeof(TextBox))
                    {
                        ExerciseToSave exerciseToSave = new ExerciseToSave()
                        {
                            DateToSave = DateTime.Today,
                            Id = exerciseId,
                            Name = exerciseName

                        };
                        switch (((TextBox)item).Name)
                        {
                            case "Weight":
                                exerciseToSave.Weight = String.IsNullOrEmpty(((TextBox)item).Text) ? 0 : Int32.Parse(((TextBox)item).Text);
                                break;
                            case "Reps":
                                exerciseToSave.Reps = String.IsNullOrEmpty(((TextBox)item).Text) ? 0 : Int32.Parse(((TextBox)item).Text);
                                break;
                            case "Time":
                                exerciseToSave.Time = String.IsNullOrEmpty(((TextBox)item).Text) ? new TimeSpan() : TimeSpan.Parse(((TextBox)item).Text);
                                break;
                            default:
                                break;
                        }
                        exerciseListToSave.Add(exerciseToSave);
                    }
                }
            }
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
}
