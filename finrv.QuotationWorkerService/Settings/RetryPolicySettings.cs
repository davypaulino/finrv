namespace finrv.QuotationWorkerService.Settings;

public class RetryPolicySettings
{
    public int RetryCount { get; set; }
    public double SleepDurationPower { get; set; }
}