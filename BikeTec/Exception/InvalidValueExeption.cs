using System;
using System.Collections.Generic;
using System.Text;

namespace Tool
{
    class InvalidValueException:Exception
    {
        string message;

        public InvalidValueException(string val, string wf)
        {
            message = string.Format("{0} darf nicht auch den Wert {1} gesezt werden", wf, val);

        }

        public InvalidValueException(string msg)
        {
            message = msg;

        }

        public override string Message
        {
            get
            {
                return this.message;
            }
        }
    }
}
