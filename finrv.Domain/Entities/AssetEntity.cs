namespace finrv.Domain.Entities;
public class AssetEntity : BaseEntity
{
    public uint Id { get; set; }
    public string Ticker { get; set; }
    public string Name { get; set; }

    private AssetEntity() { }

    public AssetEntity(string ticker, string name)
    {
        Ticker = ticker;
        Name = name;
    }
}
