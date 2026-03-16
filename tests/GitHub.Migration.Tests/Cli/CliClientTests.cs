using FluentAssertions;
using GitHub.Migration.Cli;
using GitHub.Migration.Services;
using Moq;

namespace GitHub.Migration.Tests.Cli;

public class CliClientTests
{
    [Theory]
    [MemberData(nameof(ConstructWithInvalidParameters))]
    public void Should_Throw_On_Construct_With_Invalid_Parameters(Action act, string parameterName)
    {
        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Theory]
    [MemberData(nameof(RunCommandWithInvalidParameters))]
    public async Task Should_Throw_On_Run_Command_With_Invalid_Parameters(Func<Task> act, string parameterName)
    {
        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName(parameterName);
    }

    public static TheoryData<Action, string> ConstructWithInvalidParameters
    {
        get
        {
            var processRunner = Mock.Of<IProcessRunner>();

            return new TheoryData<Action, string>
            {
                { () => _ = new CliClient(null!, null!), "processRunner" },
                { () => _ = new CliClient(processRunner, null!), "logger" }
            };
        }
    }

    public static TheoryData<Func<Task>, string> RunCommandWithInvalidParameters
    {
        get
        {
            var client = new CliClient(
                Mock.Of<IProcessRunner>(),
                Mock.Of<ICliLogger>());

            return new TheoryData<Func<Task>, string>
            {
                { async () => await client.RunCommandAsync(null!), "command" }
            };
        }
    }
}
