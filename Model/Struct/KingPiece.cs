using zAjedrez.Model.Enums;
using zAjedrez.Model.Contract;
using System;

namespace zAjedrez.Model.Struct
{
    // Pieza Rey
    // Se mueve 1 casilla en cualquier dirección: horizontal, vertical o diagonal
    // Es la pieza más importante, perder el rey significa perder la partida
    public class KingPiece : Piece
    {
        #region Constructor

        public KingPiece(PieceColor color, int row, int column) : base(color, row, column)
        {
            Type = PieceType.King;
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

            // El rey se mueve 1 casilla en cualquier dirección
            return (rowDiff <= 1 && colDiff <= 1) && !(rowDiff == 0 && colDiff == 0);
        }

        protected override string GetImagePath()
        {
            return $"/Assets/Pieces/{(Color == PieceColor.White ? "White" : "Black")}_King.png";
        }

        #endregion
    }
}
