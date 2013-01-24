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

        Random r = new Random(352333);
        public MainWindow()
        {
            InitializeComponent();
            Exercise.Text = "Exercises for " + DateTime.Today;
        }

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
            //store everything here when clicked
            IList<ExerciseToSave> exerciseListToSave = new List<ExerciseToSave>();
            Store(ExerciseList, exerciseListToSave);

            MessageBox.Show("Completion successful.", "This has been stored.  Ready for querying!!");
            WeatherType.SelectedItem = null;
        }

        private static void Store(StackPanel exerciseList, IList<ExerciseToSave> exerciseListToSave)
        {
            using (TextReader writer = new StreamReader("exercises.json"))
            using (JsonTextReader jsonWriter = new JsonTextReader(writer))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
                exerciseListToSave = serializer.Deserialize<IList<ExerciseToSave>>(jsonWriter);
            }

            PopulateList(exerciseList, exerciseListToSave);
            
            using (TextWriter writer = new StreamWriter("exercises.json"))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Serialize(jsonWriter, exerciseListToSave);
            }
        }

        private static void PopulateList(StackPanel exerciseList, IList<ExerciseToSave> exerciseListToSave)
        {
            foreach (StackPanel exercise in exerciseList.Children)
            {
                string exerciseName = ((TextBlock)exercise.Children[0]).Text;

                if (!"REST".Equals(exerciseName.ToUpper()))
                {
                    int exerciseId = Int32.Parse(((TextBlock)exercise.Children[0]).Name.Split('_')[1]);
                    ExerciseType exerciseType = (ExerciseType)Enum.Parse(typeof(ExerciseType), ((TextBlock)exercise.Children[2]).Text.Replace(' ', '_'));

                    ExerciseToSave exerciseToSave = new ExerciseToSave()
                    {
                        DateToSave = DateTime.Today,
                        Id = exerciseId,
                        Name = exerciseName,
                        ExerciseType = exerciseType
                    };

                    foreach (var item in ((StackPanel)exercise.Children[3]).Children)
                    {
                        if (item.GetType() == typeof(TextBox))
                        {
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
                        }
                    }
                    exerciseListToSave.Add(exerciseToSave);
                }
            }
        }

        private void Settings_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("settings clicked");
        }

        private void Trends_Click_1(object sender, RoutedEventArgs e)
        {
            //show a all workouts in a table.  show # of completions, goal, best score, and date best score was achieved.
            //show weighted list
            if (ExerciseListPanel.Visibility == System.Windows.Visibility.Visible)
            {
                TrendsToggle.Content = "Exercise List";
                ExerciseListPanel.Visibility = System.Windows.Visibility.Collapsed;
                SavePanel.Visibility = System.Windows.Visibility.Collapsed;
                TrendDataPanel.Visibility = System.Windows.Visibility.Visible;
                WeatherType.IsEnabled = false;

                IList<ExerciseToSave> exerciseList = RetrieveSavedExercises();

                List<IExercise> tempList = FlattenExerciseListDictionary();

                //here combine by id, get type for each id and based on type, look at highest value
                IDictionary<int, BestExercise> bestExerciseDictionary = new Dictionary<int, BestExercise>();
                foreach (ExerciseToSave item in exerciseList)
                {
                    BestExercise bestExercise = new BestExercise();
                    bestExercise = new BestExercise() { 
                        Combination = tempList.First(t => t.Id == item.Id).Output(), 
                        ExerciseType = item.ExerciseType,
                        Name = item.Name,
                        BestScore = templateDictionary[item.ExerciseType].CreateBestScore(item),
                        Date = item.DateToSave,
                        Id = item.Id,
                        Count = 1
                    };

                    if (!bestExerciseDictionary.ContainsKey(item.Id))
                        bestExerciseDictionary[item.Id] = bestExercise;
                    else
                    {
                        BestExercise currentBestExercise = bestExerciseDictionary[item.Id];
                        bestExerciseDictionary[item.Id] = templateDictionary[item.ExerciseType].CompareExercisesByTopScore(currentBestExercise, bestExercise);
                        bestExerciseDictionary[item.Id].Count++;
                    }
                }

                TableRowGroup group = new TableRowGroup();
                group.Name = "TrendDataDetails";
                foreach (KeyValuePair<int, BestExercise> exerciseKeyValuePair in bestExerciseDictionary)
                {
                    TableRow row = new TableRow();
                    BestExercise item = exerciseKeyValuePair.Value;
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.Name))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1, 1, 1, 0), Padding = new Thickness(5), FontSize = 10 });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.ExerciseType.ToString().Replace('_', ' ') + " " + item.Combination))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1, 1, 1, 0), Padding = new Thickness(5), FontSize = 10 });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.Count.ToString()))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1, 1, 1, 0), Padding = new Thickness(5), FontSize = 10 });
                    row.Cells.Add(new TableCell(templateDictionary[item.ExerciseType].BuildParagraph(item)) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1, 1, 1, 0), Padding = new Thickness(5), FontSize = 10 });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.Date.ToShortDateString()))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1, 1, 1, 0), Padding = new Thickness(5), FontSize = 10 });
                    group.Rows.Add(row);
                }
                TrendDataTable.RowGroups.Add(group);

            }
            else
            {
                WeatherType.IsEnabled = true;
                TrendsToggle.Content = "Historical Trend";
                ExerciseListPanel.Visibility = System.Windows.Visibility.Visible;
                TrendDataPanel.Visibility = System.Windows.Visibility.Collapsed;
                SavePanel.Visibility = System.Windows.Visibility.Visible;
                TrendDataTable.RowGroups.RemoveAt(1);
            }

        }

        private static List<IExercise> FlattenExerciseListDictionary()
        {
            List<IExercise> tempList = new List<IExercise>();
            foreach (var item in Entities.ExerciseList.GetExerciseList())
            {
                tempList.AddRange(item.Value);
            }
            return tempList;
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
}
