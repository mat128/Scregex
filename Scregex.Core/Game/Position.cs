using System.Text;

namespace Scregex.Core.Game;


public record Position(int X, int Y)
{
    // Represents a position on a typical 15x15 board.
    // Coordinates are 0-based, starting top left.

    public static readonly Position Center = new(7, 7);
    public const int BoardWidth = 15;
    public const int BoardHeight = 15;

    public int FlatCoordinate()
    {
        return Y * BoardWidth + X;
    }

    public bool IsWithinBounds()
    {
        if (X > (BoardWidth - 1)) return false;
        if (Y > (BoardHeight - 1)) return false;

        return true;
    }

    public static Position FromCoordinates(string coordinates)
    {
        if (coordinates.Length is < 2 or > 3) throw new ArgumentException("Invalid coordinates", nameof(coordinates));

        var rowLetter = coordinates[0];
        var rowNumber = Encoding.ASCII.GetBytes(new[] { rowLetter })[0];
        if (rowNumber is < 0x41 or > 0x4F) throw new ArgumentException("Invalid coordinates", nameof(coordinates));

        int row = rowNumber - 0x41;

        var columnPos = int.Parse(coordinates[1..]);
        if (columnPos is < 1 or > BoardWidth) throw new ArgumentException("Invalid coordinates", nameof(coordinates));

        return new Position(columnPos - 1, row);
    }

    public static Position FromFlatCoordinate(int flatCoordinate)
    {
        return new Position(flatCoordinate % BoardWidth, flatCoordinate / BoardWidth);
    }
}