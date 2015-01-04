using System;
using System.Collections.Generic;
using System.Text;

namespace Tool //.Exception
{
    public class UnknownFileException:Exception
    {
        private string message;

        public UnknownFileException(string msg)
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