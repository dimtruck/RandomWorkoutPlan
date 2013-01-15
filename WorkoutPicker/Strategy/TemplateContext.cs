using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WorkoutPicker.Strategy
{
    public class TemplateContext
    {
        private readonly ITemplateStrategy _strategy;

        public TemplateContext(ITemplateStrategy strategy)
        {
            this._strategy = strategy;
        }

        public StackPanel Build()
        {
            return this._strategy.Build();
        }
    }
}
