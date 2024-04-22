using System.Collections.Generic;
using System.Linq;

namespace KC.WPF_Kanban.Model;

internal class JsonColumn
{
    public string Caption { get; set; }

    public int CardLimit { get; set; } = -1;

    public int ColumnSpan { get; set; }

    public object ColumnValue { get; set; }

    public bool IsCollapsed { get; set; }

    public string Color { get; set; }

    public IList<JsonColumn> Columns { get; set; } = new List<JsonColumn>();

    internal int ChildColumnSpan => Columns?.Count > 0 ?
        Columns.Sum(c => c.ChildColumnSpan) :
        ColumnSpan;

    internal void FixParentColumnSpan()
    {
        if (Columns?.Count > 0)
        {
            foreach (JsonColumn col in Columns)
            {
                col.FixParentColumnSpan();
            }
            ColumnSpan = ChildColumnSpan;
        }
    }
}
