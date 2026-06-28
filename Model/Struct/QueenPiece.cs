using zAjedrez.Model.Enums;
using zAjedrez.Model.Contract;
using System;

namespace zAjedrez.Model.Struct
{
    // Pieza Reina
    // Se mueve horizontalmente, verticalmente o diagonalmente cualquier número de casillas
    // Combina movimientos de Torre y Alfil
    // No puede saltar sobre otras piezas
    public class QueenPiece : Piece
    {
        #region Constructor

        public QueenPiece(PieceColor color, int row, int column) : base(color, row, column)
        {
            Type = PieceType.Queen;
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

            // Movimiento horizontal o vertical (como Torre)
            bool isHorizontalOrVertical = (destRow == Row) || (destColumn == Column);

            // Movimiento diagonal (como Alfil)
            bool isDiagonal = (rowDiff == colDiff && rowDiff > 0);

            if (!isHorizontalOrVertical && !isDiagonal)
                return false;

            // Verificar que el camino esté despejado
            return IsPathClear(Row, Column, destRow, destColumn, board);
        }

        protected override string GetImagePath()
        {
            return $"/Assets/Pieces/{(Color == PieceColor.White ? "White" : "Black")}_Queen.png";
        }

        #endregion
    }
}
