using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPicker.Template
{
    public class SetRepTemplate : ITemplate
    {
        public string Create(params dynamic[] value)
        {
            return "Lift " + value[0] + " sets of " + value[1] + " reps.";
        }
    }
}
