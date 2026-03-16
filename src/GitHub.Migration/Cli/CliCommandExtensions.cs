using System.Reflection;

namespace GitHub.Migration.Cli;

public static class CliCommandExtensions
{
    public static string ToCommandString(
        this ICliCommand command,
        bool redacted = false,
        string? separator = null,
        char? quote = null)
    {
        ArgumentNullException.ThrowIfNull(command);

        var type = command.GetType();
        var propertyInfos = type.GetProperties();

        var commandValues = new List<string>(propertyInfos.Length) { command.CliCommand };

        foreach (var propertyInfo in propertyInfos)
        {
            var argumentAttribute = propertyInfo.GetCustomAttribute<CliCommandArgumentAttribute>();
            var optionAttribute = propertyInfo.GetCustomAttribute<CliCommandOptionAttribute>();
            var propertyValue = propertyInfo.GetValue(command);

            // Arguments
            if (argumentAttribute is not null)
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    var argumentValue = (string)propertyValue!;
                    commandValues.Add(argumentValue);
                }
                else if (propertyInfo.PropertyType == typeof(IReadOnlyList<string>))
                {
                    var argumentValues = (IReadOnlyList<string>?)propertyValue;

                    if (argumentValues is null)
                    {
                        continue;
                    }

                    foreach (var argumentValue in argumentValues)
                    {
                        commandValues.Add(argumentValue);
                    }
                }
            }

            // Options
            if (optionAttribute is not null)
            {
                if (propertyInfo.PropertyType == typeof(bool) && (bool)(propertyValue ?? false))
                {
                    commandValues.Add(optionAttribute.Option);
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    var optionValue = propertyValue as string;

                    if (string.IsNullOrWhiteSpace(optionValue) && !optionAttribute.IsRequired)
                    {
                        continue;
                    }

                    if (optionAttribute.IsSecret && redacted)
                    {
                        optionValue = "***";
                    }

                    commandValues.Add($"{optionAttribute.Option} {quote}{optionValue}{quote}");
                }
                else if (propertyInfo.PropertyType == typeof(int))
                {
                    var optionValue = propertyInfo.GetValue(command) as int?;

                    if (optionValue is null && !optionAttribute.IsRequired)
                    {
                        continue;
                    }

                    commandValues.Add($"{optionAttribute.Option} {optionValue}");
                }
                else if (propertyInfo.PropertyType == typeof(IReadOnlyList<string>))
                {
                    var optionValues = (IReadOnlyList<string>?)propertyValue;

                    if (optionValues is null)
                    {
                        continue;
                    }

                    foreach (var optionValue in optionValues)
                    {
                        commandValues.Add($"{optionAttribute.Option} {quote}{optionValue}{quote}");
                    }

                }
            }
        }

        return string.Join(separator ?? " ", commandValues).Trim();
    }
}
