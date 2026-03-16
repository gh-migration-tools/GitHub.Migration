namespace GitHub.Migration.Cli;

public interface ICliClient
{
    Task<int> RunCommandAsync(
        ICliCommand command,
        string? workingDirectory = null,
        IReadOnlyList<KeyValuePair<string, string>>? environmentVariables = null,
        Action<string>? outputDataReceived = null,
        Action<string>? errorDataReceived = null);
}
