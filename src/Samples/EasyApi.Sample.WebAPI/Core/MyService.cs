namespace EasyApi.Sample.WebAPI.Core
{
    public class MyService : IMyService
    {
        public string Process(string param1)
        {
            return $"Processed {param1}";
        }
    }
}