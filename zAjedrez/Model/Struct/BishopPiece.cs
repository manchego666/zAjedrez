using zAjedrez.Model.Enums;
using zAjedrez.Model.Contract;
using System;

namespace zAjedrez.Model.Struct
{
    // BISHOP: MOV diagonal (|row_delta| == |col_delta|)
    // NO jump: validate path clear via IsPathClear()
    // Capture: any color != this.Color
    // Attack vector: 4 diagonal directions (NE, NW, SE, SW) at 45 degrees
    public class BishopPiece : Piece
    {
        #region Constructor

        public BishopPiece(PieceColor color, int row, int column) : base(color, row, column)
        {
            Type = PieceType.Bishop;
        }

        #endregion

        #region Methods

        // BISHOP MOVEMENT VALIDATION
        // MOV AX, ABS(destRow - Row) // rowDiff
        // MOV BX, ABS(destColumn - Column) // colDiff
        // CMP AX, BX // JNE invalid_diagonal
        // CALL IsPathClear(Row, Column, destRow, destColumn, board)
        // RET status (JZ blocked, else clear)
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

            if (rowDiff != colDiff)
                return false;

            return IsPathClear(Row, Column, destRow, destColumn, board);
        }

        protected override string GetImagePath()
        {
            return $"/Assets/Pieces/{(Color == PieceColor.White ? "White" : "Black")}_Bishop.png";
        }

        #endregion
    }
}
