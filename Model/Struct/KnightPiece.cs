using zAjedrez.Model.Enums;
using zAjedrez.Model.Contract;
using System;

namespace zAjedrez.Model.Struct
{
    // Pieza Caballo
    // Se mueve en forma de L: 2 casillas en una dirección y 1 en perpendicular
    // Es la única pieza que puede saltar sobre otras
    public class KnightPiece : Piece
    {
        #region Constructor

        public KnightPiece(PieceColor color, int row, int column) : base(color, row, column)
        {
            Type = PieceType.Knight;
        }

        #endregion

        #region Methods

        public override bool IsMoveLegal(int destRow, int destColumn, Piece[,] board)
        {
            // Validar rango del tablero
            if (destRow < 0 || destRow > 7 || destColumn < 0 || destColumn > 7)
                return false;

            // No puede ser el mismo cuadrado
            if (destRow == Row && destColumn == Column)
                return false;

            // No puede capturarse a sí mismo
            if (IsFriendlyPieceAtPosition(destRow, destColumn, board))
                return false;

            int rowDiff = Math.Abs(destRow - Row);
            int colDiff = Math.Abs(destColumn - Column);

            // Movimiento en L: (2,1) o (1,2)
            return (rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2);
        }

        protected override string GetImagePath()
        {
            return $"/Assets/Pieces/{(Color == PieceColor.White ? "White" : "Black")}_Knight.png";
        }

        #endregion
    }
}
