using Scregex.Core.Game;

namespace Scregex.Core.Tests.Game;

public class GameEngineTest
{
    [Fact]
    public void TracksCurrentlyAvailableWords()
    {
        var game = new GameEngine(new [] { "compiler", "linker" });

        var words = game.GetCurrentlyAvailableWords().ToList();

        Assert.Contains("compiler", words);
        Assert.Contains("linker", words);
    }

    [Fact]
    public void TracksCurrentlyAvailableWords_AfterValidPlay_WordIsNotAvailableAnymore()
    {
        var game = new GameEngine(new [] { "compiler", "linker" });

        var result = game.PlayWord("compiler", Position.Center, Direction.Horizontal);

        Assert.Equal(PlayResult.Valid, result);

        var words = game.GetCurrentlyAvailableWords().ToList();

        Assert.DoesNotContain("compiler", words);
        Assert.Contains("linker", words);
    }

    [Fact]
    public void RefusesToPlayWordNotContainedInDictionary()
    {
        var game = new GameEngine(new [] { "compiler" });

        var result = game.PlayWord("source", Position.Center, Direction.Horizontal);

        Assert.Equal(PlayResult.Invalid, result);
    }

    [Fact]
    public void RefusesToPlayAlreadyPlayedWords()
    {
        var game = new GameEngine(new [] { "compiler" });

        game.PlayWord("compiler", Position.Center, Direction.Horizontal);
        var result = game.PlayWord("compiler", Position.Center, Direction.Horizontal);

        Assert.Equal(PlayResult.Invalid, result);
    }

    [Fact]
    public void RequiresFirstMoveAtStartingPosition_WordStartsAtCenter_IsAccepted()
    {
        var game = new GameEngine(new [] { "compiler" });

        var result = game.PlayWord("compiler", Position.Center, Direction.Horizontal);

        Assert.Equal(PlayResult.Valid, result);
    }

    [Fact]
    public void RequiresFirstMoveAtStartingPosition_WordTouchesCenter_IsAccepted()
    {
        var game = new GameEngine(new [] { "compiler" });

        var result = game.PlayWord("compiler", new Position(0, 7), Direction.Horizontal);

        Assert.Equal(PlayResult.Valid, result);
    }

    [Fact]
    public void RequiresFirstMoveAtStartingPosition_WordStartsAtA1_IsRejected()
    {
        var game = new GameEngine(new [] { "compiler" });

        var result = game.PlayWord("compiler", Position.FromCoordinates("A1"), Direction.Horizontal);

        Assert.Equal(PlayResult.Invalid, result);
    }

    [Fact]
    public void GameBoardReflectsCurrentState_WithoutPlaying()
    {
        var game = new GameEngine(new [] { "compiler" });

        var board = game.CurrentBoardSnapshot();

        Assert.Equal(
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               ", board.AsText());
    }

    [Fact]
    public void GameBoardReflectsCurrentState_AfterPlayingFirstWord_Horizontal()
    {
        var game = new GameEngine(new [] { "compiler" });

        game.PlayWord("compiler", new Position(0, 7), Direction.Horizontal);

        var board = game.CurrentBoardSnapshot();

        Assert.Equal(
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "compiler       " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               ", board.AsText());
    }

    [Fact]
    public void GameBoardReflectsCurrentState_AfterPlayingFirstWord_Vertical()
    {
        var game = new GameEngine(new [] { "compiler" });

        game.PlayWord("compiler", new Position(7, 7), Direction.Vertical);

        var board = game.CurrentBoardSnapshot();

        Assert.Equal(
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "               " + "\n" +
            "       c       " + "\n" +
            "       o       " + "\n" +
            "       m       " + "\n" +
            "       p       " + "\n" +
            "       i       " + "\n" +
            "       l       " + "\n" +
            "       e       " + "\n" +
            "       r       ", board.AsText());
    }
}