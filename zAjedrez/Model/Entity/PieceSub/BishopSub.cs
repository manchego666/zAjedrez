using zAjedrez.Model.Enums;
using zAjedrez.Model.Contract;
using System;

namespace zAjedrez.Model.Entity.PieceSub
{
    /// <summary>
    /// Bishop subclass - polymorphic piece implementation
    /// Move diagonally any number of squares
    /// Cannot jump over pieces
    /// </summary>
    public class BishopSub : Piece
    {
        #region Properties

        /// <summary>
        /// Bishops can capture enemy pieces
        /// </summary>
        public override bool CanCapture => true;

        /// <summary>
        /// Bishop can advance up to 7 squares diagonally
        /// </summary>
        public override int AdvanceSquares => 7;

        /// <summary>
        /// Bishop description for UI display
        /// </summary>
        public override string Description => "Bishop - Moves diagonally any distance";

        /// <summary>
        /// Bishop sprite path
        /// </summary>
        public override string ImagePath => $"/Assets/Pieces/{(Color == PieceColor.White ? "White" : "Black")}_Bishop.png";

        #endregion

        #region Constructor

        /// <summary>
        /// CTOR BishopSub with color and position
        /// PUSH Type = Bishop
        /// </summary>
        public BishopSub(PieceColor color, int row, int column) : base(color, row, column)
        {
            Type = PieceType.Bishop;
        }

        #endregion

        #region Methods

        /// <summary>
        /// BISHOP_MOVE_VALIDATION
        /// MOV AX, ABS(destRow - Row) // rowDiff
        /// MOV BX, ABS(destColumn - Column) // colDiff
        /// CMP AX, BX // JNE invalid_diagonal
        /// CALL IsPathClear() // RET status
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

            if (rowDiff != colDiff)
                return false;

            return IsPathClear(Row, Column, destRow, destColumn, board);
        }

        #endregion
    }
}
