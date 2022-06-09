using System.Collections.Generic;

namespace Core
{
   public class payloadVM
   {
      //Muss string sein da der UNIX Timestamp nicht direkt beim deserialisieren in ein Datetime geparst werden kann
      public string timestamp { get; set; }

      public List<modulesVM> modules { get; set; }
   }
}