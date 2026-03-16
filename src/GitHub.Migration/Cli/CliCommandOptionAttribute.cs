namespace GitHub.Migration.Cli;

[AttributeUsage(AttributeTargets.Property)]
public sealed class CliCommandOptionAttribute : Attribute
{
    public string Option { get; }
    public bool IsRequired { get; }
    public bool IsSecret { get; }

    public CliCommandOptionAttribute(
        string option,
        bool isRequired = false,
        bool isSecret = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(option);

        Option = option;
        IsRequired = isRequired;
        IsSecret = isSecret;
    }
}
