using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KC.WPF_Kanban
{
    public interface ICollapsible
    {
        bool IsCollapsed { get; }
    }
}
