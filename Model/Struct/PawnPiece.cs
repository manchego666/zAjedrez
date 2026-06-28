using zAjedrez.Model.Enums;
using zAjedrez.Model.Contract;

namespace zAjedrez.Model.Struct
{
    // Pieza Peón
    // Mueve 1 casilla hacia adelante, 2 casillas en el primer movimiento
    // Captura en diagonal
    public class PawnPiece : Piece
    {
        #region Constructor

        public PawnPiece(PieceColor color, int row, int column) : base(color, row, column)
        {
            Type = PieceType.Pawn;
        }

        #endregion

        #region Methods

        public override bool IsMoveLegal(int destRow, int destColumn, Piece[,] board)
        {
            // Validar rango del tablero
            if (destRow < 0 || destRow > 7 || destColumn < 0 || destColumn > 7)
                return false;

            // No puede capturarse a sí mismo
            if (IsFriendlyPieceAtPosition(destRow, destColumn, board))
                return false;

            int rowDirection = (Color == PieceColor.White) ? -1 : 1;
            int rowDifference = destRow - Row;

            // Movimiento normal: 1 casilla adelante
            if (destColumn == Column && rowDifference == rowDirection)
            {
                return IsPositionEmpty(destRow, destColumn, board);
            }

            // Movimiento inicial: 2 casillas adelante
            if (destColumn == Column && rowDifference == rowDirection * 2 && !HasMoved)
            {
                return IsPositionEmpty(Row + rowDirection, Column, board) &&
                       IsPositionEmpty(destRow, destColumn, board);
            }

            // Captura diagonal
            if (System.Math.Abs(destColumn - Column) == 1 && rowDifference == rowDirection)
            {
                return IsEnemyPieceAtPosition(destRow, destColumn, board);
            }

            return false;
        }

        protected override string GetImagePath()
        {
            return $"/Assets/Pieces/{(Color == PieceColor.White ? "White" : "Black")}_Pawn.png";
        }

        #endregion
    }
}
