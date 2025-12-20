namespace WebApi.Application.Core.Logging.LoggerData
{
    public class UseCaseLoggerData
    {
        public string UseCaseId { get; set; }
        public bool IsAuthorized { get; set; }
        public DateTime ExecutionDateTime { get; set; }
        public string Data { get; set; }
    }
}
