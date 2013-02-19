using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPicker.Template;

namespace WorkoutPicker.Entities
{
    public class AerobicExercise : IExercise, IAerobic
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Laps { get; set; }

        public TimeSpan Time { get; set; }

        public float Distance { get; set; }

        public float Weight { get; set; }

        public ExerciseType ExerciseType { get; set; }

        /// <summary>
        /// output for run exercise
        /// </summary>
        /// <returns></returns>
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
                            if (Distance != 0)
                                builder.Append(new LapTimeDistanceTemplate().Create(Laps, Time, Distance));
                            else
                                builder.Append(new LapTimeTemplate().Create(Laps, Time));
                        }
                        else
                        {
                            if (Distance != 0)
                                builder.Append(new LapDistanceTemplate().Create(Laps, Distance));
                            else
                                builder.Append(new LapTemplate().Create(Laps));
                        }
                    }
                    else if (Time.TotalMilliseconds > 0)
                    {
                        if (Distance != 0)
                            builder.Append(new TimeDistanceTemplate().Create(Time, Distance));
                        else
                            builder.Append(new TimeTemplate().Create(Time));
                    }
                    else if (Distance != 0)
                        builder.Append(new DistanceTemplate().Create(Distance));

                }

                //Name - Run ${distance}
                //Name - Run ${distance} laps in ${time}
                return builder.ToString();
            }
        }

        public float ExerciseWeight { get; set; }

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
