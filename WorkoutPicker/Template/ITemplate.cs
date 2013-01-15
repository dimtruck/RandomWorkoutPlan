using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPicker.Template
{
    public interface ITemplate
    {
        String Create(params dynamic[] value);
    }
}
