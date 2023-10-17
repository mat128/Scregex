namespace Scregex.Core.Game;

public class GameBoard
{
    private List<PlayedWord> _playedWords = new();
    public GameBoard(IEnumerable<PlayedWord> playedWords)
    {
        _playedWords.AddRange(playedWords);
    }

    public string AsText()
    {
        return string.Join("\n", CurrentValues().Chunk(15).Select(row => string.Join("", row)));
    }

    public char[] CurrentValues()
    {
        char[] values = new char[225];

        Array.Fill(values, ' ');

        foreach (var playedCharacter in _playedWords.SelectMany(word => word.Characters()))
        {
            values[playedCharacter.position.FlatCoordinate()] = playedCharacter.character;
        }

        return values;
    }
}