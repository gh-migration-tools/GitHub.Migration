using FluentAssertions;
using GitHub.Migration.Cli;

namespace GitHub.Migration.Tests.Cli;

public class CliCommandExtensionsTests
{
    [Fact]
    public void Should_Return_Redacted_Command()
    {
        // Arrange
        var command = GetCommand();

        // Act
        var result = command.ToCommandString(redacted: true);

        // Assert
        result.Should().Be($"login --username {command.Username} --password ***");
    }

    [Fact]
    public void Should_Return_Unredacted_Command()
    {
        // Arrange
        var command = GetCommand();

        // Act
        var result = command.ToCommandString();

        // Assert
        result.Should().Be($"login --username {command.Username} --password {command.Password}");
    }

    private static LoginCommand GetCommand() =>
        new()
        {
            Username = "username",
            Password = "password"
        };

    private sealed class LoginCommand : ICliCommand
    {
        public string CliCommand => "login";

        [CliCommandOption("--username")]
        public string Username { get; init; } = null!;

        [CliCommandOption("--password", isSecret: true)]
        public string Password { get; init; } = null!;
    }
}
