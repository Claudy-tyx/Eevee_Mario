public class UpgradeOption
{
    public UpgradeType type;
    public string displayName;
    public string description;

    public UpgradeOption(
        UpgradeType type,
        string displayName,
        string description
    )
    {
        this.type = type;
        this.displayName = displayName;
        this.description = description;
    }
}