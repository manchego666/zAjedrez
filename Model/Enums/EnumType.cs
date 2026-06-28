namespace zAjedrez.Model.Enums
{
    // POP PieceType ENUM FROM STACK
    // PUSH CamelCase Type Identifiers
    public enum PieceType
    {
        None,      // INT 0x00
        Pawn,      // INT 0x01
        Rook,      // INT 0x02
        Knight,    // INT 0x03
        Bishop,    // INT 0x04
        Queen,     // INT 0x05
        King       // INT 0x06
    }

    // MOV AL, Color // MOV BH, Color
    // CMP AL, BH // PUSH FLAG
    public enum PieceColor
    {
        White,     // 0x00 - Player 1
        Black      // 0x01 - Player 2
    }

    // POP MoveResult FROM EXECUTION STACK
    // MOV AX, [MoveResult] // PUSH Status
    public enum MoveResult
    {
        Success,                   // INT 0x00 - JMP to next turn
        InvalidSourceSquare,       // INT 0x01 - MOV error
        InvalidDestinationSquare,  // INT 0x02 - MOV error
        NotYourTurn,              // INT 0x03 - CMP turn flag failed
        InvalidMove,              // INT 0x04 - Validation failed
        KingInCheck,              // INT 0x05 - CRIT status
        GameOver                  // INT 0x06 - HLT execution
    }
}
