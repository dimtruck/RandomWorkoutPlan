﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPicker.Template
{
    public class RepTemplate : ITemplate
    {
        public string Create(params dynamic[] value)
        {
            return "Perform " + value[0] + " reps.";
        }
    }
}
