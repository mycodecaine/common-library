using Codecaine.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Common.Date
{
    /// <summary>
    /// MachineDateTime provides the current date and time based on the system clock.
    /// </summary>
    public class MachineDateTime : IDateTime
    {
        public DateTime UtcNow => DateTime.UtcNow;

        public DateTime Now =>DateTime.Now;
    }
}
