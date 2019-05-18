using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KC.WPF_Kanban.Model
{
    internal class JsonBoard
    {
        public string Title { get; set; }

        public string ColumnPath { get; set; }

        public string SwimlanePath { get; set; }


        public IList<JsonColumn> Columns { get; set; } = new List<JsonColumn>();

        public IList<JsonSwimlane> Swimlanes { get; set; } = new List<JsonSwimlane>();

        internal void FixParentColumnSpan()
        {
            foreach (JsonColumn col in Columns)
            {
                col.FixParentColumnSpan();
            }
        }
    }
}
