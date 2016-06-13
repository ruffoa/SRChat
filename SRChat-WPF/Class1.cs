using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
        using System.Windows.Input;

namespace SRChat_WPF
{

    public static class Commands
    {
        /// <summary>
        /// Represents the Foo feature of the program.
        /// </summary>
        public static readonly RoutedCommand EnterSend = new RoutedCommand();
    }
}
   