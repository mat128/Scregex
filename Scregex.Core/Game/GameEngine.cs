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

    public PlayResult PlayWord(PlayedWord playedWord)
    {
        if (WordIsAlreadyUsed(playedWord)) return PlayResult.Invalid;
        if (!PlacementIsValid(playedWord)) return PlayResult.Invalid;

        _playedWords.Add(playedWord);
        return PlayResult.Valid;
    }

    public PlayResult PlayWord(string word, Position position, Direction direction) => PlayWord(new PlayedWord(word, position, direction));

    private bool PlacementIsValid(PlayedWord word)
    {
        return TryPlayWord(word) == PlayResult.Valid;
    }

    private bool WordIsAlreadyUsed(PlayedWord word)
    {
        return !GetCurrentlyAvailableWords().Contains(word.Word);
    }

    public GameBoard CurrentBoardSnapshot()
    {
        return new GameBoard(_playedWords);
    }

    public PlayResult TryPlayWord(PlayedWord word)
    {
        var tentativeWords = new List<PlayedWord>(_playedWords) { word };

        if (!FirstPlayedWordCrossesCenter(tentativeWords)) return PlayResult.Invalid;
        if (!EachWordHasOnlyBeenUsedOnce(tentativeWords)) return PlayResult.Invalid;
        if (!OverlappingCharactersAreTheSame(tentativeWords)) return PlayResult.Invalid;
        if (!WordIsWithinBounds(word)) return PlayResult.Invalid;
        if (!WordAddsPlayedCharacters(_playedWords, tentativeWords)) return PlayResult.Invalid;
        if (!OneCharacterOverlaps(_playedWords, word)) return PlayResult.Invalid;

        return PlayResult.Valid;
    }

    private bool OneCharacterOverlaps(List<PlayedWord> playedWords, PlayedWord word)
    {
        if (playedWords.Count == 0) return true;

        var alreadyPlayedPositions = playedWords.SelectMany(it => it.Characters().Select(c => c.Position));
        var wordPositions = word.Characters().Select(it => it.Position);

        return wordPositions.Any(it => alreadyPlayedPositions.Contains(it));
    }

    private static bool WordAddsPlayedCharacters(List<PlayedWord> previouslyPlayedWords, List<PlayedWord> tentativeWords)
    {
        var playedPositions = previouslyPlayedWords.SelectMany(it => it.Characters().Select(c => c.Position)).Distinct().ToList();
        var tentativePositions = tentativeWords.SelectMany(it => it.Characters().Select(c => c.Position)).Distinct().ToList();

        return tentativePositions.Count() > playedPositions.Count();
    }


    private bool WordIsWithinBounds(PlayedWord word)
    {
        return word.Characters().All(it => it.Position.IsWithinBounds());
    }

    private static bool EachWordHasOnlyBeenUsedOnce(List<PlayedWord> tentativeWords)
    {
        return tentativeWords.GroupBy(it => it.Word).All(it => it.Count() == 1);
    }

    private static bool OverlappingCharactersAreTheSame(List<PlayedWord> tentativeWords)
    {
        return tentativeWords
            .SelectMany(it => it.Characters())
            .GroupBy(it => it.Position)
            .All(it => it.DistinctBy(pc => pc.Character).Count() == 1);
    }
    private static bool FirstPlayedWordCrossesCenter(List<PlayedWord> tentativeWords)
    {
        if (tentativeWords.Count == 1)
        {
            return tentativeWords.Single().Crosses(Position.Center);
        }

        return true;
    }
}