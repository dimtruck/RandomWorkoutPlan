using Newtonsoft.Json;
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
        private ObservableCollection<IExercise> _exerciseCollection = new ObservableCollection<IExercise>(FlattenExerciseListDictionary());
        private ObservableCollection<BestExercise> _bestExerciseCollection = new ObservableCollection<BestExercise>();

        Random r = new Random(352333);
        public MainWindow()
        {
            InitializeComponent();
            Exercise.Text = "Exercises for " + DateTime.Today;
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

        private void Settings_Click_1(object sender, RoutedEventArgs e)
        {
            ExerciseListPanel.Visibility = System.Windows.Visibility.Collapsed;
            SavePanel.Visibility = System.Windows.Visibility.Collapsed;
            SettingsPanel.Visibility = System.Windows.Visibility.Visible;
            TrendDataPanel.Visibility = System.Windows.Visibility.Collapsed;
            WeatherType.IsEnabled = false;
            this.DataContext = this;
        }

        private void Trends_Click_1(object sender, RoutedEventArgs e)
        {
            //show a all workouts in a table.  show # of completions, goal, best score, and date best score was achieved.
            //show weighted list
            if (SettingsPanel.Visibility == System.Windows.Visibility.Visible && TrendsToggle.Content.Equals("Exercise List"))
                ViewExerciseListPanel();
            else if (SettingsPanel.Visibility == System.Windows.Visibility.Visible && TrendsToggle.Content.Equals("Historical Trend"))
                ViewTrendPanel();
            else if (ExerciseListPanel.Visibility == System.Windows.Visibility.Visible)
                ViewTrendPanel();
            else
                ViewExerciseListPanel();
        }

        private void ViewExerciseListPanel()
        {
            WeatherType.IsEnabled = true;
            TrendsToggle.Content = "Historical Trend";
            ExerciseListPanel.Visibility = System.Windows.Visibility.Visible;
            TrendDataPanel.Visibility = System.Windows.Visibility.Collapsed;
            SavePanel.Visibility = System.Windows.Visibility.Visible;
            SettingsPanel.Visibility = System.Windows.Visibility.Collapsed;
        }



        private void ViewTrendPanel()
        {
            TrendsToggle.Content = "Exercise List";
            ExerciseListPanel.Visibility = System.Windows.Visibility.Collapsed;
            SavePanel.Visibility = System.Windows.Visibility.Collapsed;
            SettingsPanel.Visibility = System.Windows.Visibility.Collapsed;
            TrendDataPanel.Visibility = System.Windows.Visibility.Visible;
            WeatherType.IsEnabled = false;

            //here combine by id, get type for each id and based on type, look at highest value
            IList<BestExercise> bestExerciseList = CompileBestExerciseList( RetrieveSavedExercises(), FlattenExerciseListDictionary());
            _bestExerciseCollection.Clear();
            foreach (var item in bestExerciseList)
                _bestExerciseCollection.Add(item);
            this.DataContext = this;
        }
        private IList<BestExercise> CompileBestExerciseList(IList<ExerciseToSave> exerciseList, List<IExercise> tempList)
        {
            IList<BestExercise> bestExerciseList = new List<BestExercise>();
            foreach (ExerciseToSave item in exerciseList)
            {
                if (bestExerciseList.FirstOrDefault(t => t.Id == item.Id) != null)
                {
                    //already exists
                    BestExercise currentBestExercise = bestExerciseList.FirstOrDefault(t => t.Id == item.Id);
                    BestExercise bestExercise = templateDictionary[item.ExerciseType].CompareExercisesByTopScore(currentBestExercise, new BestExercise()
                    {
                        Combination = tempList.First(t => t.Id == item.Id).Output,
                        ExerciseType = item.ExerciseType,
                        Name = item.Name,
                        BestScore = templateDictionary[item.ExerciseType].CreateBestScore(item),
                        Date = item.DateToSave,
                        Id = item.Id,
                        Count = 1
                    });
                    bestExercise.Count = currentBestExercise.Count + 1;
                    bestExerciseList.Remove(currentBestExercise);
                    bestExerciseList.Add(bestExercise);
                }
                else
                {
                    //doesn't exist
                    bestExerciseList.Add(new BestExercise()
                    {
                        Combination = tempList.First(t => t.Id == item.Id).Output,
                        ExerciseType = item.ExerciseType,
                        Name = item.Name,
                        BestScore = templateDictionary[item.ExerciseType].CreateBestScore(item),
                        Date = item.DateToSave,
                        Id = item.Id,
                        Count = 1
                    });
                }
            }
            return bestExerciseList;
        }

        private static List<IExercise> FlattenExerciseListDictionary()
        {
            List<IExercise> tempList = new List<IExercise>();
            foreach (var item in Entities.ExerciseList.GetExerciseList())
                tempList.AddRange(item.Value);
            return tempList.Distinct(new ExerciseComparer()).ToList();
        }

        private static IList<ExerciseToSave> RetrieveSavedExercises()
        {
            IList<ExerciseToSave> exerciseList = new List<ExerciseToSave>();
            //show all workouts here
            using (TextReader writer = new StreamReader("exercises.json"))
            using (JsonTextReader jsonWriter = new JsonTextReader(writer))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
                exerciseList = serializer.Deserialize<IList<ExerciseToSave>>(jsonWriter);
            }
            return exerciseList;
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
