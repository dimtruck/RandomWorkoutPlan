using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPicker.Template
{
    public class LapTimeDistanceTemplate : ITemplate
    {
        public string Create(params dynamic[] value)
        {
            return "Run " + value[0] + " laps in " + value[1] + " for " + value[2] + ".";
        }
    }
}
