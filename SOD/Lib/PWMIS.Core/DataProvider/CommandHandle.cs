using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PWMIS.DataProvider.Data
{
    public class CommandHandle
    {
        public CommandHandle(IDbCommand cmd)
        {
            CurrCommandLog = new CommandLog(true);
        }

        public CommandLog CurrCommandLog { get; private set; }

        public event EventHandler<EventArgs> Executing;
        public event EventHandler<EventArgs> Executed;
    }
}
