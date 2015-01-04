using System;
using System.Collections.Generic;
using System.Text;

namespace Tool
{
    public class InputException:Exception
    {
        private string message;

        public InputException(string msg)
        {
            this.message = msg;
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
