using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPicker.Entities
{
    public class WeatherSettingDTO
    {
        public String WeatherType { get; set; }
        public int NumberOfExercises { get; set; }
    }
}
