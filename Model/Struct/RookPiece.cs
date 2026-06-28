using zAjedrez.Model.Enums;
using zAjedrez.Model.Contract;

namespace zAjedrez.Model.Struct
{
    // ROOK: MOV horizontal OR vertical
    // NO jump: validate path clear (LOOP through cells)
    // Capture: any color != this.Color
    // Linear attack vector 0/90/180/270 degrees
    public class RookPiece : Piece
    {
        #region Constructor

        public RookPiece(PieceColor color, int row, int column) : base(color, row, column)
        {
            Type = PieceType.Rook;
        }

        #endregion

        #region Methods

        // ROOK MOVEMENT VALIDATION
        // CMP destRow, Row // JE horizontal_move
        // CMP destColumn, Column // JE vertical_move
        // JMP invalid_move // RET false
        // horizontal_move: CALL IsPathClear() // JZ blocked
        // vertical_move: CALL IsPathClear() // JZ blocked
        // RET true if path clear
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

        protected override string GetImagePath()
        {
            return $"/Assets/Pieces/{(Color == PieceColor.White ? "White" : "Black")}_Rook.png";
        }

        #endregion
    }
}
