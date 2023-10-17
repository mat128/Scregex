namespace Scregex.Core.Game;

public record PlayedWord(string Word, Position Position, Direction Direction)
{
    public Position StartPosition()
    {
        return Position;
    }

    public Position LastPosition()
    {
        return Direction switch
        {
            Direction.Horizontal => Position with { X = Position.X + Word.Length - 1 },
            Direction.Vertical => Position with { Y = Position.Y + Word.Length - 1 },
            _ => throw new ArgumentException()
        };
    }

    public bool Crosses(Position position)
    {
        return Direction switch
        {
            Direction.Horizontal => StartPosition().Y == position.Y && StartPosition().X <= position.X && position.X <= LastPosition().X,
            Direction.Vertical => StartPosition().X == position.X && StartPosition().Y <= position.Y && position.Y <= LastPosition().Y,
            _ => false
        };
    }

    public IEnumerable<PlayedCharacter> Characters()
    {
        var chars = new PlayedCharacter[Word.Length];

        for (int i = 0; i < Word.Length; i++)
        {
            Position position = Direction switch
            {
                Direction.Horizontal => StartPosition() with { X = StartPosition().X + i },
                Direction.Vertical => StartPosition() with { Y = StartPosition().Y + i },
            };
            chars[i] = new PlayedCharacter(Word[i], position);
        }

        return chars;
    }
}