using Scregex.Core.Game;

namespace Scregex.Core.Tests.Game;

public class PositionTest
{
    [InlineData("A1", 0, 0)]
    [InlineData("A15", 14, 0)]
    [InlineData("B1", 0, 1)]
    [InlineData("B15", 14, 1)]
    [InlineData("C1", 0, 2)]
    [InlineData("D1", 0, 3)]
    [InlineData("E1", 0, 4)]
    [InlineData("O15", 14, 14)]
    [Theory]
    public void CanTellIts2DCoordinates(string coordinates, int x, int y)
    {
        var pos = Position.FromCoordinates(coordinates);

        Assert.Equal(x, pos.X);
        Assert.Equal(y, pos.Y);
    }

    [InlineData("@1")] // before lower row bound
    [InlineData("P1")] // past upper row bound
    [InlineData("A0")] // before lower column bound
    [InlineData("A16")] // past upper column bound
    [InlineData("a1")] // lowercase
    [InlineData("")] // too short
    [InlineData("A")] // missing column
    [InlineData("1")] // missing row
    [InlineData("A2147483648")] // int overflow
    [Theory]
    public void RejectInvalidCoordinates(string coordinates)
    {
        Assert.Throws<ArgumentException>(() => Position.FromCoordinates(coordinates));
    }

    [InlineData("A1", 0)]
    [InlineData("A2", 1)]
    [InlineData("A15", 14)]
    [InlineData("B1", 15)]
    [InlineData("B15", 29)]
    [InlineData("O15", 224)]
    [Theory]
    public void CanTellIts1DCoordinate(string coordinates, int expected)
    {
        Assert.Equal(expected, Position.FromCoordinates(coordinates).OneDimensionCoordinate());
    }
}