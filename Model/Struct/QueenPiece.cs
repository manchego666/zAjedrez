using zAjedrez.Model.Enums;
using zAjedrez.Model.Contract;
using System;

namespace zAjedrez.Model.Struct
{
    // QUEEN: MOV horizontal OR vertical (Rook) OR diagonal (Bishop)
    // Combines Rook + Bishop attack patterns
    // NO jump: validate path clear
    // Capture: any color != this.Color
    // Attack vector: 8 directions (N,S,E,W,NE,NW,SE,SW)
    public class QueenPiece : Piece
    {
        #region Constructor

        public QueenPiece(PieceColor color, int row, int column) : base(color, row, column)
        {
            Type = PieceType.Queen;
        }

        #endregion

        #region Methods

        // QUEEN MOVEMENT VALIDATION (Rook OR Bishop logic)
        // isHorizontalOrVertical: destRow == Row OR destColumn == Column
        // isDiagonal: |rowDiff| == |colDiff| AND rowDiff > 0
        // CMP isHorizontalOrVertical OR isDiagonal // JE direction_valid
        // JMP invalid_move
        // direction_valid: CALL IsPathClear() // RET
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

            bool isHorizontalOrVertical = (destRow == Row) || (destColumn == Column);
            bool isDiagonal = (rowDiff == colDiff && rowDiff > 0);

            if (!isHorizontalOrVertical && !isDiagonal)
                return false;

            return IsPathClear(Row, Column, destRow, destColumn, board);
        }

        protected override string GetImagePath()
        {
            return $"/Assets/Pieces/{(Color == PieceColor.White ? "White" : "Black")}_Queen.png";
        }

        #endregion
    }
}
