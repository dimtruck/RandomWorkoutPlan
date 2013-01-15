using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPicker.Template
{
    public class TimeTemplate : ITemplate
    {
        /// <summary>
        /// Name - Run ${distance}
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Create(params dynamic[] value)
        {
            return "Run/Execute for " + value[0] + ".";
        }
    }
}
