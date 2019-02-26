using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KC.WPF_Kanban.Model
{
    internal class JsonSwimlane
    {
        public string Foreground { get; set; }

        public string Color { get; set; }

        public string Caption { get; set; }

        public object LaneValue { get; set; }

        public bool IsCollapsed { get; set; }
    }
}
