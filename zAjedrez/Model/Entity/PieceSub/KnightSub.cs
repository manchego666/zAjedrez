using zAjedrez.Model.Enums;
using zAjedrez.Model.Contract;
using System;

namespace zAjedrez.Model.Entity.PieceSub
{
    /// <summary>
    /// Knight subclass - polymorphic piece implementation
    /// Move in L-shape: 2 squares one direction, 1 square perpendicular
    /// Only piece that can jump over other pieces
    /// </summary>
    public class KnightSub : Piece
    {
        #region Properties

        /// <summary>
        /// Knights can capture enemy pieces
        /// </summary>
        public override bool CanCapture => true;

        /// <summary>
        /// Knight move distance is fixed: 2+1 = 3 squares total
        /// </summary>
        public override int AdvanceSquares => 3;

        /// <summary>
        /// Knight description for UI display
        /// </summary>
        public override string Description => "Knight - Moves in L-shape, only piece that jumps";

        /// <summary>
        /// Knight sprite path
        /// </summary>
        public override string ImagePath => $"/Assets/Pieces/{(Color == PieceColor.White ? "White" : "Black")}_Knight.png";

        #endregion

        #region Constructor

        /// <summary>
        /// CTOR KnightSub with color and position
        /// PUSH Type = Knight
        /// </summary>
        public KnightSub(PieceColor color, int row, int column) : base(color, row, column)
        {
            Type = PieceType.Knight;
        }

        #endregion

        #region Methods

        /// <summary>
        /// KNIGHT_MOVE_VALIDATION
        /// MOV AX, ABS(destRow - Row) // rowDiff
        /// MOV BX, ABS(destColumn - Column) // colDiff
        /// CMP AX, 2 AND BX, 1 // JE valid_l_shape
        /// CMP AX, 1 AND BX, 2 // JE valid_l_shape
        /// JMP invalid_move
        /// RET status (knights jump, no path check needed)
        /// </summary>
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

            return (rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2);
        }

        #endregion
    }
}
