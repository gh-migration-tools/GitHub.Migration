using GitHub.Migration.Services;

namespace GitHub.Migration.Cli;

public sealed class CliClient : ICliClient
{
    private readonly IProcessRunner _processRunner;
    private readonly ICliLogger _logger;

    public CliClient(
        IProcessRunner processRunner,
        ICliLogger logger)
    {
        ArgumentNullException.ThrowIfNull(processRunner);
        ArgumentNullException.ThrowIfNull(logger);

        _processRunner = processRunner;
        _logger = logger;
    }

    public async Task<int> RunCommandAsync(
        ICliCommand command,
        string? workingDirectory = null,
        IReadOnlyList<KeyValuePair<string, string>>? environmentVariables = null,
        Action<string>? outputDataReceived = null,
        Action<string>? errorDataReceived = null)
    {
        ArgumentNullException.ThrowIfNull(command);

        _logger.Debug($"Process start with command '{command.ToCommandString(redacted: true)}'");

        var exitCode = await _processRunner.StartAsync(
                command.ToCommandString(),
                workingDirectory,
                environmentVariables,
                outputData =>
                {
                    outputDataReceived?.Invoke(outputData);
                    _logger.Debug(outputData);
                },
                errorData =>
                {
                    errorDataReceived?.Invoke(errorData);
                    _logger.Debug(errorData);
                })
            .ConfigureAwait(false);

        _logger.Debug($"Process completed with exit code {exitCode}");

        return exitCode;
    }
}
