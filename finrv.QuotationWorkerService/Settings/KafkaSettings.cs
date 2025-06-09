namespace finrv.QuotationWorkerService.Settings;

public class KafkaSettings
{
    public string Host { get; set; }
    public string GroupId { get; set; }
    public Dictionary<string, string> Topics { get; set; }
}

