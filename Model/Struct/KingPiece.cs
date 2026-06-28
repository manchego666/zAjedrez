using zAjedrez.Model.Enums;
using zAjedrez.Model.Contract;
using System;

namespace zAjedrez.Model.Struct
{
    // KING: MOV 1 square any direction (H,V,D)
    // Most critical piece: loss == game over
    // CMP |rowDiff|, 1 AND |colDiff|, 1 // JLE move_valid
    // Attack vector: 8 adjacent squares (radius 1)
    // TODO: Castling logic (separate from basic move)
    public class KingPiece : Piece
    {
        #region Constructor

        public KingPiece(PieceColor color, int row, int column) : base(color, row, column)
        {
            Type = PieceType.King;
        }

        #endregion

        #region Methods

        // KING MOVEMENT VALIDATION
        // MOV AX, ABS(destRow - Row) // rowDiff
        // MOV BX, ABS(destColumn - Column) // colDiff
        // CMP AX, 1 // JLE valid_row
        // JMP invalid_move
        // valid_row: CMP BX, 1 // JLE valid_col
        // JMP invalid_move
        // valid_col: MOV CX, rowDiff XOR colDiff // CMP CX, 0 // JE same_square
        // RET true (move valid)
        public override bool IsMoveLegal(int destRow, int destColumn, Piece[,] board)
        {
            if (destRow < 0 || destRow > 7 || destColumn < 0 || destColumn > 7)
                return false;

            if (destRow == Row && destColumn == Column)
                return false;

            if (IsFriendlyPieceAtPosition(destRow, destColumn, board))
                return false;

            int rowDiff = Math.Abs(destRow - Row);
            int colDiff = Math.Abs(destColumn - Column);

            return (rowDiff <= 1 && colDiff <= 1) && !(rowDiff == 0 && colDiff == 0);
        }

        protected override string GetImagePath()
        {
            return $"/Assets/Pieces/{(Color == PieceColor.White ? "White" : "Black")}_King.png";
        }

        #endregion
    }
}
