namespace Core
{
   public class JsonResponse
   {
      public string status { get; set; }
      public string uuid { get; set; }
      public int? sun { get; set; }
      public int? wind { get; set; }
      public int? energyBalance { get; set; }
   }
}