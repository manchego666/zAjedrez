using zAjedrez.Model.Enums;
using zAjedrez.Model.Contract;

namespace zAjedrez.Model.Struct
{
    // PAWN: MOV 1 square forward, 2 on initial move
    // CAPTURE: diagonal only (attack vector ~45 deg)
    // PROMOTION: row 0 or 7 = QUEEN (upgrade flag)
    public class PawnPiece : Piece
    {
        #region Constructor

        public PawnPiece(PieceColor color, int row, int column) : base(color, row, column)
        {
            Type = PieceType.Pawn;
        }

        #endregion

        #region Methods

        // PAWN MOVEMENT VALIDATION
        // rowDirection = (White ? -1 : +1) // forward vector
        // CMP destColumn, Column // JNZ check_diagonal_capture
        // MOV diff = destRow - Row
        // CMP diff, rowDirection // JE single_move
        // CMP diff, rowDirection*2 AND !HasMoved // JE double_move
        // JMP check_diagonal_capture
        // check_diagonal_capture: CMP |destColumn-Column|, 1 AND diff == rowDirection // JE capture_valid
        // RET status
        public override bool IsMoveLegal(int destRow, int destColumn, Piece[,] board)
        {
            if (destRow < 0 || destRow > 7 || destColumn < 0 || destColumn > 7)
                return false;

            if (IsFriendlyPieceAtPosition(destRow, destColumn, board))
                return false;

            int rowDirection = (Color == PieceColor.White) ? -1 : 1;
            int rowDifference = destRow - Row;

            if (destColumn == Column && rowDifference == rowDirection)
            {
                return IsPositionEmpty(destRow, destColumn, board);
            }

            if (destColumn == Column && rowDifference == rowDirection * 2 && !HasMoved)
            {
                return IsPositionEmpty(Row + rowDirection, Column, board) &&
                       IsPositionEmpty(destRow, destColumn, board);
            }

            if (System.Math.Abs(destColumn - Column) == 1 && rowDifference == rowDirection)
            {
                return IsEnemyPieceAtPosition(destRow, destColumn, board);
            }

            return false;
        }

        protected override string GetImagePath()
        {
            return $"/Assets/Pieces/{(Color == PieceColor.White ? "White" : "Black")}_Pawn.png";
        }

        #endregion
    }
}
