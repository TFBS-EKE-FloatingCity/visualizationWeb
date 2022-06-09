using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
   public class InvalidDurationException : Exception
   {
      public InvalidDurationException(TimeSpan maxTime) 
         : base($"Duration is invalid. The Duration has to be a value greater than 0 and less than {maxTime}!") { }
   }
}
