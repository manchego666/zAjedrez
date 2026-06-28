using zAjedrez.Model.Enums;
using zAjedrez.Model.Contract;
using System;

namespace zAjedrez.Model.Entity.PieceSub
{
    /// <summary>
    /// Queen subclass - polymorphic piece implementation
    /// Move horizontally, vertically, or diagonally any number of squares
    /// Combines Rook and Bishop movement patterns
    /// Most powerful piece except King
    /// </summary>
    public class QueenSub : Piece
    {
        #region Properties

        /// <summary>
        /// Queens can capture enemy pieces
        /// </summary>
        public override bool CanCapture => true;

        /// <summary>
        /// Queen can advance up to 7 squares in any direction
        /// </summary>
        public override int AdvanceSquares => 7;

        /// <summary>
        /// Queen description for UI display
        /// </summary>
        public override string Description => "Queen - Moves horizontally, vertically, or diagonally";

        /// <summary>
        /// Queen sprite path
        /// </summary>
        public override string ImagePath => $"/Assets/Pieces/{(Color == PieceColor.White ? "White" : "Black")}_Queen.png";

        #endregion

        #region Constructor

        /// <summary>
        /// CTOR QueenSub with color and position
        /// PUSH Type = Queen
        /// </summary>
        public QueenSub(PieceColor color, int row, int column) : base(color, row, column)
        {
            Type = PieceType.Queen;
        }

        #endregion

        #region Methods

        /// <summary>
        /// QUEEN_MOVE_VALIDATION (Rook OR Bishop logic)
        /// isHorizontalOrVertical: destRow == Row OR destColumn == Column
        /// isDiagonal: |rowDiff| == |colDiff| AND rowDiff > 0
        /// CMP isHorizontalOrVertical OR isDiagonal // JE direction_valid
        /// JMP invalid_move
        /// direction_valid: CALL IsPathClear() // RET status
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

            bool isHorizontalOrVertical = (destRow == Row) || (destColumn == Column);
            bool isDiagonal = (rowDiff == colDiff && rowDiff > 0);

            if (!isHorizontalOrVertical && !isDiagonal)
                return false;

            return IsPathClear(Row, Column, destRow, destColumn, board);
        }

        #endregion
    }
}
