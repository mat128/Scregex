using System.Collections.Immutable;

namespace Scregex.Core.Game;

public class GameEngine
{
    private readonly ImmutableList<string> _dictionary;
    private readonly List<PlayedWord> _playedWords = new();

    public GameEngine(IEnumerable<string> dictionary)
    {
        _dictionary = dictionary.ToImmutableList();
    }

    public IEnumerable<string> GetCurrentlyAvailableWords()
    {
        return _dictionary.Where(dictionaryWord => !_playedWords.Exists(it => it.Word == dictionaryWord));
    }

    public PlayResult PlayWord(string word, Position position, Direction direction)
    {
        var playedWord = new PlayedWord(word, position, direction);

        if (WordIsAlreadyUsed(playedWord)) return PlayResult.Invalid;
        if (!PlacementIsValid(playedWord)) return PlayResult.Invalid;

        _playedWords.Add(playedWord);
        return PlayResult.Valid;
    }

    private bool PlacementIsValid(PlayedWord playedWord)
    {
        if (_playedWords.Count == 0)
        {
            return playedWord.Crosses(Position.Center);
        }

        return true;
    }

    private bool WordIsAlreadyUsed(PlayedWord word)
    {
        return !GetCurrentlyAvailableWords().Contains(word.Word);
    }
}