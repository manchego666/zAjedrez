using zAjedrez.Model.Enums;
using zAjedrez.Model.Contract;

namespace zAjedrez.Model.Struct
{
    // Pieza Torre
    // Se mueve horizontalmente o verticalmente cualquier número de casillas
    // No puede saltar sobre otras piezas
    public class RookPiece : Piece
    {
        #region Constructor

        public RookPiece(PieceColor color, int row, int column) : base(color, row, column)
        {
            Type = PieceType.Rook;
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

            // Movimiento horizontal o vertical
            bool isHorizontal = (destRow == Row);
            bool isVertical = (destColumn == Column);

            if (!isHorizontal && !isVertical)
                return false;

            // Verificar que el camino esté despejado
            return IsPathClear(Row, Column, destRow, destColumn, board);
        }

        protected override string GetImagePath()
        {
            return $"/Assets/Pieces/{(Color == PieceColor.White ? "White" : "Black")}_Rook.png";
        }

        #endregion
    }
}
