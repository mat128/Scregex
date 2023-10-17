using Scregex.Core.Game;

namespace Scregex.Core.Tests.Game;

public class PlayedWordTest
{
    [Fact]
    public void CanTellItsStartingPosition()
    {
        var word = new PlayedWord("hello", new Position(1, 1), Direction.Horizontal);

        Assert.Equal(new Position(1, 1), word.StartPosition());
    }

    [Fact]
    public void CanTellItsLastPosition_Horizontal()
    {
        var word = new PlayedWord("hello", new Position(1, 1), Direction.Horizontal);

        Assert.Equal(new Position(5, 1), word.LastPosition());
    }

    [Fact]
    public void CanTellIfItCrossesAPosition_Horizontal()
    {
        var word = new PlayedWord("hello", new Position(1, 1), Direction.Horizontal);

        Assert.True(word.Crosses(new Position(1, 1)));
        Assert.True(word.Crosses(new Position(2, 1)));
        Assert.True(word.Crosses(new Position(3, 1)));
        Assert.True(word.Crosses(new Position(4, 1)));
        Assert.True(word.Crosses(new Position(5, 1)));
        Assert.False(word.Crosses(new Position(6, 1)));
        Assert.False(word.Crosses(new Position(1, 2)));
    }

    [Fact]
    public void CanTellItsLastPosition_Vertical()
    {
        var word = new PlayedWord("hello", new Position(1, 1), Direction.Vertical);

        Assert.Equal(new Position(1, 5), word.LastPosition());
    }

    [Fact]
    public void CanTellIfItCrossesAPosition_Vertical()
    {
        var word = new PlayedWord("hello", new Position(1, 1), Direction.Vertical);

        Assert.True(word.Crosses(new Position(1, 1)));
        Assert.True(word.Crosses(new Position(1, 2)));
        Assert.True(word.Crosses(new Position(1, 3)));
        Assert.True(word.Crosses(new Position(1, 4)));
        Assert.True(word.Crosses(new Position(1, 5)));
        Assert.False(word.Crosses(new Position(1, 6)));
        Assert.False(word.Crosses(new Position(2, 1)));
    }
}