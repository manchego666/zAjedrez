using zAjedrez.Model.Enums;
using zAjedrez.Model.Contract;

namespace zAjedrez.Model.Entity.PieceSub
{
    /// <summary>
    /// Rook subclass - polymorphic piece implementation
    /// Move horizontal or vertical any number of squares
    /// Cannot jump over pieces
    /// </summary>
    public class RookSub : Piece
    {
        #region Properties

        /// <summary>
        /// Rooks can capture enemy pieces
        /// </summary>
        public override bool CanCapture => true;

        /// <summary>
        /// Rook can advance up to 8 squares (board width)
        /// </summary>
        public override int AdvanceSquares => 8;

        /// <summary>
        /// Rook description for UI display
        /// </summary>
        public override string Description => "Rook - Moves horizontally or vertically any distance";

        /// <summary>
        /// Rook sprite path
        /// </summary>
        public override string ImagePath => $"/Assets/Pieces/{(Color == PieceColor.White ? "White" : "Black")}_Rook.png";

        #endregion

        #region Constructor

        /// <summary>
        /// CTOR RookSub with color and position
        /// PUSH Type = Rook
        /// </summary>
        public RookSub(PieceColor color, int row, int column) : base(color, row, column)
        {
            Type = PieceType.Rook;
        }

        #endregion

        #region Methods

        /// <summary>
        /// ROOK_MOVE_VALIDATION
        /// CMP destRow, Row // JE horizontal_move
        /// CMP destColumn, Column // JE vertical_move
        /// JMP invalid_move
        /// horizontal_move: CALL IsPathClear() // RET status
        /// vertical_move: CALL IsPathClear() // RET status
        /// </summary>
        public override bool IsMoveLegal(int destRow, int destColumn, Piece[,] board)
        {
            if (destRow < 0 || destRow > 7 || destColumn < 0 || destColumn > 7)
                return false;

            if (destRow == Row && destColumn == Column)
                return false;

            if (IsFriendlyPieceAtPosition(destRow, destColumn, board))
                return false;

            bool isHorizontal = (destRow == Row);
            bool isVertical = (destColumn == Column);

            if (!isHorizontal && !isVertical)
                return false;

            return IsPathClear(Row, Column, destRow, destColumn, board);
        }

        #endregion
    }
}
