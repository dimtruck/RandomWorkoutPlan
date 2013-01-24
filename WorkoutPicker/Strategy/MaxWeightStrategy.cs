﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WorkoutPicker.Strategy
{
    class MaxWeightStrategy : ITemplateStrategy
    {
        public System.Windows.Controls.StackPanel Build()
        {
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock weightText = new TextBlock() { Text = "Weight: ", Width = 100 };
            TextBox weightBox = new TextBox() { Width = 100, Name="Weight" };
            panel.Children.Add(weightText);
            panel.Children.Add(weightBox);
            return panel;
        }


        public dynamic CreateBestScore(Entities.ExerciseToSave exercise)
        {
            return exercise.Weight;
        }


        public Entities.BestExercise CompareExercisesByTopScore(Entities.BestExercise oldExercise, Entities.BestExercise newExercise)
        {
            if (newExercise.BestScore > oldExercise.BestScore)
                return newExercise;
            else
                return oldExercise;
        }


        public Paragraph BuildParagraph(Entities.BestExercise exercise)
        {
            return new Paragraph(new Run(exercise.BestScore.ToString()));
        }
    }
}
