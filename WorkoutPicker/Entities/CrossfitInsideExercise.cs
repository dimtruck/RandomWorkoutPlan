using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPicker.Template;

namespace WorkoutPicker.Entities
{
    public class CrossfitInsideExercise : ICrossfitInside
    {
        public int Id { get; set; }

        public TimeSpan Time { get; set; }

        public int Sets { get; set; }

        public int Reps { get; set; }

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
                    if (Reps > 0)
                    {
                        if (Time.TotalMilliseconds > 0)
                        {
                            if (Sets > 0)
                            {
                            }
                            else
                                builder.Append(new RepTimeTemplate().Create(Reps, Time));
                        }
                        else
                        {
                            if (Sets > 0)
                                builder.Append(new SetRepTemplate().Create(Reps, Time));
                            else
                                builder.Append(new RepTemplate().Create(Reps));
                        }
                    }
                    else
                    {
                        if (Time.TotalMilliseconds > 0)
                        {
                            builder.Append(new TimeTemplate().Create(Time));
                        }

                    }

                }
                return builder.ToString();
            }
        }

        public ExerciseType ExerciseType { get; set; }
    }
}
