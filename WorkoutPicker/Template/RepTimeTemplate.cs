using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPicker.Template
{
    public class RepTimeTemplate : ITemplate
    {
        public string Create(params dynamic[] value)
        {
            return "Perform " + value[0] + " reps in " + value[1] + ".";
        }
    }
}
