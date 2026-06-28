using zAjedrez.Model.Enums;
using zAjedrez.Model.Contract;
using System;

namespace zAjedrez.Model.Entity.PieceSub
{
    /// <summary>
    /// King subclass - polymorphic piece implementation
    /// Move 1 square in any direction: horizontal, vertical, or diagonal
    /// Most critical piece: loss = game over
    /// TODO: Castling special move
    /// </summary>
    public class KingSub : Piece
    {
        #region Properties

        /// <summary>
        /// Kings can capture enemy pieces
        /// </summary>
        public override bool CanCapture => true;

        /// <summary>
        /// King advance: 1 square only
        /// </summary>
        public override int AdvanceSquares => 1;

        /// <summary>
        /// King description for UI display
        /// </summary>
        public override string Description => "King - Moves one square in any direction, most important piece";

        /// <summary>
        /// King sprite path
        /// </summary>
        public override string ImagePath => $"/Assets/Pieces/{(Color == PieceColor.White ? "White" : "Black")}_King.png";

        #endregion

        #region Constructor

        /// <summary>
        /// CTOR KingSub with color and position
        /// PUSH Type = King
        /// </summary>
        public KingSub(PieceColor color, int row, int column) : base(color, row, column)
        {
            Type = PieceType.King;
        }

        #endregion

        #region Methods

        /// <summary>
        /// KING_MOVE_VALIDATION
        /// MOV AX, ABS(destRow - Row) // rowDiff
        /// MOV BX, ABS(destColumn - Column) // colDiff
        /// CMP AX, 1 // JLE valid_row
        /// JMP invalid_move
        /// valid_row: CMP BX, 1 // JLE valid_col
        /// JMP invalid_move
        /// valid_col: RET true (move valid)
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

            return (rowDiff <= 1 && colDiff <= 1) && !(rowDiff == 0 && colDiff == 0);
        }

        #endregion
    }
}
