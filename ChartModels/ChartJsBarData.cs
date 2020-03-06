using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bug_tracker.ChartModels
{
    public class ChartJsBarData
    {
        public List<string> Labels { get; set; }
        public List<int> Data { get; set; }
        public ChartJsBarData()
        {
            Labels = new List<string>();
            Data = new List<int>();
        }
    }
}