namespace zAjedrez.Model.Enums
{
    // Define los tipos de piezas del ajedrez
    public enum PieceType
    {
        None,
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King
    }

    // Define los colores de las piezas
    public enum PieceColor
    {
        White,
        Black
    }

    // Define el resultado de un movimiento
    public enum MoveResult
    {
        Success,
        InvalidSourceSquare,
        InvalidDestinationSquare,
        NotYourTurn,
        InvalidMove,
        KingInCheck,
        GameOver
    }
}
