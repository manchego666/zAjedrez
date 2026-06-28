using zAjedrez.Model.Enums;
using zAjedrez.Model.Contract;
using System;

namespace zAjedrez.Model.Struct
{
    // Pieza Alfil
    // Se mueve diagonalmente cualquier número de casillas
    // No puede saltar sobre otras piezas
    public class BishopPiece : Piece
    {
        #region Constructor

        public BishopPiece(PieceColor color, int row, int column) : base(color, row, column)
        {
            Type = PieceType.Bishop;
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

            // Movimiento diagonal: diferencia en filas = diferencia en columnas
            if (rowDiff != colDiff)
                return false;

            // Verificar que el camino esté despejado
            return IsPathClear(Row, Column, destRow, destColumn, board);
        }

        protected override string GetImagePath()
        {
            return $"/Assets/Pieces/{(Color == PieceColor.White ? "White" : "Black")}_Bishop.png";
        }

        #endregion
    }
}
