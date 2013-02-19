using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPicker.Template;

namespace WorkoutPicker.Entities
{
    public class PowerliftingExercise : IPowerlifting
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TimeSpan Time { get; set; }

        public float Distance { get; set; }

        public int Reps { get; set; }

        public int Sets { get; set; }

        public float Weight { get; set; }

        public ExerciseType ExerciseType { get; set; }

        public float ExerciseWeight { get; set; }

        public string Output
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                //options
                if (!String.IsNullOrEmpty(Name))
                {
                    //weight/sets/reps/time/distance - NA
                    //weight/sets/reps/time - Name - lift weight for s sets of r reps in t time
                    //weight/sets/reps/distance - NA
                    //weight/sets/reps - Name - lift weight for s sets of r reps
                    //weight/sets/distance - NA (strongman with laps)
                    //weight/sets/time - NA (strongman with laps)
                    //weight/sets - NA
                    //weight/reps - 
                    //weight/
                    if (Sets > 0)
                    {
                        if (Reps > 0)
                        {
                            if (Distance > 0)
                            {
                            }
                            else if (Time.TotalMilliseconds > 0)
                            {
                            }
                            else if (Weight > 0)
                            {
                            }
                            else
                            {
                                builder.Append(new SetRepTemplate().Create(Sets, Reps));
                            }
                        }
                        else
                        {
                            builder.Append(new SetTemplate().Create(Sets));
                        }
                    }

                }
                return builder.ToString();
            }
        }


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
