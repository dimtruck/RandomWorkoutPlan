﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPicker.Template;

namespace WorkoutPicker.Entities
{
    public class StrongmanExercise : IStrongman
    {
        public int Id { get; set; }

        public int Laps { get; set; }

        public TimeSpan Time { get; set; }

        public string Name { get; set; }

        public float ExerciseWeight { get; set; }

        public string Output
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                //options
                if (!String.IsNullOrEmpty(Name))
                {
                    if (Laps != 0)
                    {
                        if (Time.TotalMilliseconds > 0)
                        {
                            builder.Append(new LapTimeTemplate().Create(Laps, Time));
                        }
                        else
                        {
                            builder.Append(new LapTemplate().Create(Laps));
                        }
                    }
                    else
                        if (Time.TotalMilliseconds > 0)
                        {
                            builder.Append(new TimeTemplate().Create(Time));
                        }

                }

                //Name - Run ${distance}
                //Name - Run ${distance} laps in ${time}
                return builder.ToString();
            }
        }

        public ExerciseType ExerciseType { get; set; }

        public string ExerciseTypeString
        {
            get { return ExerciseType.ToString().Replace('_', ' '); }
        }

        public string Result
        {
            get
            {
                BestExercise bestExercise = Entities.ExerciseList.CompileBestExerciseList().FirstOrDefault(t => t.Id == Id);
                if (bestExercise != null)
                    return bestExercise.BestScore + " completed on " + bestExercise.Date.ToString();
                else
                    return "Not yet completed/doesn't apply";
            }
        }

        public string Description
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
