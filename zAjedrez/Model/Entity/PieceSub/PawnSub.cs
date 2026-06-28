using zAjedrez.Model.Enums;
using zAjedrez.Model.Contract;

namespace zAjedrez.Model.Entity.PieceSub
{
    /// <summary>
    /// Pawn subclass - polymorphic piece implementation
    /// Advance 1 square forward, 2 on initial move
    /// Capture diagonal only
    /// Promotion on reaching end rank
    /// </summary>
    public class PawnSub : Piece
    {
        #region Properties

        /// <summary>
        /// Pawns can capture enemy pieces
        /// </summary>
        public override bool CanCapture => true;

        /// <summary>
        /// Pawn advance: 1 square normally, 2 on first move (handled in IsMoveLegal)
        /// </summary>
        public override int AdvanceSquares => 1;

        /// <summary>
        /// Pawn description for UI display
        /// </summary>
        public override string Description => "Pawn - Advances one square, captures diagonally";

        /// <summary>
        /// Pawn sprite path
        /// </summary>
        public override string ImagePath => $"/Assets/Pieces/{(Color == PieceColor.White ? "White" : "Black")}_Pawn.png";

        #endregion

        #region Constructor

        /// <summary>
        /// CTOR PawnSub with color and position
        /// PUSH Type = Pawn
        /// </summary>
        public PawnSub(PieceColor color, int row, int column) : base(color, row, column)
        {
            Type = PieceType.Pawn;
        }

        #endregion

        #region Methods

        /// <summary>
        /// PAWN_MOVE_VALIDATION
        /// rowDirection = (White ? -1 : +1) // forward vector
        /// CMP destColumn, Column // JNZ check_diagonal_capture
        /// MOV diff = destRow - Row
        /// CMP diff, rowDirection // JE single_move
        /// CMP diff, rowDirection*2 AND !HasMoved // JE double_move
        /// JMP check_diagonal_capture
        /// RET status
        /// </summary>
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

        #endregion
    }
}
