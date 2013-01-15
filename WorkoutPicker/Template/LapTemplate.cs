using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPicker.Template
{
    public class LapTemplate : ITemplate
    {
        /// <summary>
        /// Name - Run ${laps} laps 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Create(params dynamic[] value)
        {
            return "Run " + value[0] + " laps.";
        }
    }
}
