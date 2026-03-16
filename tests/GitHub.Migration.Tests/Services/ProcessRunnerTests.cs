using FluentAssertions;
using GitHub.Migration.Services;

namespace GitHub.Migration.Tests.Services;

public class ProcessRunnerTests : BaseTest
{
    [Theory]
    [MemberData(nameof(RunCommandWithInvalidParameters))]
    public async Task Should_Throw_On_Run_Command_With_Invalid_Parameters(Func<Task> act, string parameterName, Type exceptionType)
    {
        // Assert
        if (exceptionType == Types.ArgumentException)
        {
            await act.Should().ThrowAsync<ArgumentException>().WithParameterName(parameterName);
        }
        else if (exceptionType == Types.ArgumentNullException)
        {
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName(parameterName);
        }
    }

    [Fact]
    public async Task Should_Run_Command()
    {
        // Assert
        var processRunner = new ProcessRunner();
        var outputData = new List<string>();

        // Act
        var result = await processRunner.StartAsync(
            "echo 'Hello World'",
            outputDataReceived: output => outputData.Add(output));

        // Assert
        result.Should().Be(0);

        outputData.Should().ContainSingle().Which.Should().Be("Hello World");
    }

    public static TheoryData<Func<Task>, string, Type> RunCommandWithInvalidParameters
    {
        get
        {
            var processRunner = new ProcessRunner();

            return new TheoryData<Func<Task>, string, Type>
            {
                { async () => await processRunner.StartAsync(StrNull), "command", Types.ArgumentNullException },
                { async () => await processRunner.StartAsync(StrEmpty), "command", Types.ArgumentException },
                { async () => await processRunner.StartAsync(StrWhitespace), "command", Types.ArgumentException },
            };
        }
    }
}
