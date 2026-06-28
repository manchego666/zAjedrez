using zAjedrez.Model.Enums;

namespace zAjedrez.Model.Contract
{
    // Clase abstracta base para todas las piezas
    // Define el contrato que todas las piezas deben cumplir
    public abstract class Piece
    {
        #region Properties

        public PieceType Type { get; protected set; }
        public PieceColor Color { get; protected set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public bool HasMoved { get; set; }
        public string ImagePath { get; protected set; }

        #endregion

        #region Constructor

        protected Piece(PieceColor color, int row, int column)
        {
            Color = color;
            Row = row;
            Column = column;
            HasMoved = false;
            ImagePath = GetImagePath();
        }

        #endregion

        #region Abstract Methods

        // Método abstracto que cada pieza debe implementar
        // Valida si el movimiento es legal según las reglas de la pieza
        public abstract bool IsMoveLegal(int destRow, int destColumn, Piece[,] board);

        // Retorna la ruta de imagen de la pieza
        // Cada pieza implementa su propia lógica de rutas
        protected abstract string GetImagePath();

        #endregion

        #region Methods

        // Verifica si hay una pieza enemiga en la posición destino
        protected bool IsEnemyPieceAtPosition(int row, int column, Piece[,] board)
        {
            if (row < 0 || row > 7 || column < 0 || column > 7) return false;
            var piece = board[row, column];
            return piece != null && piece.Type != PieceType.None && piece.Color != this.Color;
        }

        // Verifica si hay una pieza amiga en la posición destino
        protected bool IsFriendlyPieceAtPosition(int row, int column, Piece[,] board)
        {
            if (row < 0 || row > 7 || column < 0 || column > 7) return false;
            var piece = board[row, column];
            return piece != null && piece.Type != PieceType.None && piece.Color == this.Color;
        }

        // Verifica si la posición está vacía
        protected bool IsPositionEmpty(int row, int column, Piece[,] board)
        {
            if (row < 0 || row > 7 || column < 0 || column > 7) return false;
            return board[row, column] == null || board[row, column].Type == PieceType.None;
        }

        // Verifica si el camino entre dos posiciones está despejado (para piezas que se mueven en línea)
        protected bool IsPathClear(int fromRow, int fromCol, int toRow, int toCol, Piece[,] board)
        {
            int rowDirection = System.Math.Sign(toRow - fromRow);
            int colDirection = System.Math.Sign(toCol - fromCol);

            int currentRow = fromRow + rowDirection;
            int currentCol = fromCol + colDirection;

            while (currentRow != toRow || currentCol != toCol)
            {
                if (!IsPositionEmpty(currentRow, currentCol, board))
                    return false;
                currentRow += rowDirection;
                currentCol += colDirection;
            }

            return true;
        }

        // Retorna representación textual de la pieza para debugging
        public override string ToString()
        {
            return $"{Type} ({Color})";
        }

        #endregion
    }
}
