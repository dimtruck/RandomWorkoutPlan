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
    public class WeatherSetting
    {
        private string _weatherType;
        public String WeatherType
        {
            get
            {
                return _weatherType;
            }
            set
            {
                IList<WeatherSettingDTO> weatherSettingList = new List<WeatherSettingDTO>();
                using (StreamReader reader = File.OpenText("weather_setting.json"))
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    var serializer = new JsonSerializer();
                    weatherSettingList = serializer.Deserialize<List<WeatherSettingDTO>>(jsonReader);
                }

                foreach (WeatherSettingDTO item in weatherSettingList)
                    if (item.WeatherType.Equals(_weatherType))
                        item.WeatherType = value;

                using (TextWriter writer = new StreamWriter("weather_setting.json"))
                using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Converters.Add(new JavaScriptDateTimeConverter());
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    serializer.Serialize(jsonWriter, weatherSettingList);
                } 
                _weatherType = value;
            }
        }

        private int _numberOfExercises;
        public int NumberOfExercises
        {
            get
            {
                return _numberOfExercises;
            }
            set
            {
                _numberOfExercises = value;
                IList<WeatherSettingDTO> templist = new List<WeatherSettingDTO>();
                using (StreamReader reader = File.OpenText("weather_setting.json"))
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    var serializer = new JsonSerializer();
                    templist = serializer.Deserialize<List<WeatherSettingDTO>>(jsonReader);
                }

                foreach (WeatherSettingDTO item in templist)
                    if (item.WeatherType.Equals(_weatherType))
                        item.NumberOfExercises = _numberOfExercises;

                using (TextWriter writer = new StreamWriter("weather_setting.json"))
                using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Converters.Add(new JavaScriptDateTimeConverter());
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    serializer.Serialize(jsonWriter, templist);
                }
            }
        }

    }
}
