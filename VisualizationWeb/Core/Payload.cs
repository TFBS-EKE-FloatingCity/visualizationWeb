using System.Collections.Generic;

namespace Core
{
   public class Payload
   {
      //Muss string sein da der UNIX Timestamp nicht direkt beim deserialisieren in ein Datetime geparst werden kann
      public string timestamp { get; set; }

      public List<Module> modules { get; set; }
   }
}