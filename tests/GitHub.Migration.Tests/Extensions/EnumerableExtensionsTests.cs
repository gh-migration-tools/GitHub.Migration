using FluentAssertions;

namespace GitHub.Migration.Tests.Extensions;

public class EnumerableExtensionsTests
{
    [Fact]
    public void ToKeyValueList_ReturnsCorrectFormat()
    {
        // Arrange
        var keyValuePairs = new List<KeyValuePair<string, string>>
        {
            new("abc", "123"),
            new("def", "456")
        };

        var expectedResult = new List<string>
        {
            "abc=123",
            "def=456"
        };

        // Act
        var result = keyValuePairs.ToKeyValueList();

        // Assert
        result.Should().HaveCount(expectedResult.Count);
        result.Should().BeEquivalentTo(expectedResult);
    }
}
