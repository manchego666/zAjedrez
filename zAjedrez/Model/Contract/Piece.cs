using zAjedrez.Model.Enums;

namespace zAjedrez.Model.Contract
{
    /// <summary>
    /// Abstract Piece base class
    /// Implements polymorphic chess piece behavior
    /// Each subclass overrides movement rules and properties
    /// </summary>
    public abstract class Piece : IPiece
    {
        #region Properties

        /// <summary>
        /// Piece type (set by concrete subclass)
        /// </summary>
        public PieceType Type { get; protected set; }

        /// <summary>
        /// Piece color: White or Black
        /// </summary>
        public PieceColor Color { get; protected set; }

        /// <summary>
        /// Current row position on board (0-7)
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Current column position on board (0-7)
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Track if piece has moved (for castling, pawn double-move)
        /// </summary>
        public bool HasMoved { get; set; }

        /// <summary>
        /// Can this piece capture enemy pieces
        /// </summary>
        public abstract bool CanCapture { get; }

        /// <summary>
        /// Maximum squares piece can advance per move
        /// 1 = King/Pawn initial, 8+ = Rook/Bishop/Queen (unlimited)
        /// </summary>
        public abstract int AdvanceSquares { get; }

        /// <summary>
        /// Human-readable piece description
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Path to piece sprite/icon image
        /// </summary>
        public abstract string ImagePath { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// CTOR: Initialize piece with color and position
        /// PUSH Color, Row, Column onto stack
        /// </summary>
        protected Piece(PieceColor color, int row, int column)
        {
            Color = color;
            Row = row;
            Column = column;
            HasMoved = false;
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Validate if move is legal per piece-specific rules
        /// Implemented by concrete subclasses
        /// RET true if legal, false if invalid
        /// </summary>
        public abstract bool IsMoveLegal(int destRow, int destColumn, Piece[,] board);

        /// <summary>
        /// Get image path for rendering
        /// Each piece type has unique sprite
        /// </summary>
        public abstract string GetImagePath();

        #endregion

        #region Methods

        /// <summary>
        /// TEST if enemy piece at destination
        /// CMP piece.Color != this.Color AND Type != None
        /// RET true if enemy detected
        /// </summary>
        protected bool IsEnemyPieceAtPosition(int row, int column, Piece[,] board)
        {
            if (row < 0 || row > 7 || column < 0 || column > 7) return false;
            var piece = board[row, column];
            return piece != null && piece.Type != PieceType.None && piece.Color != this.Color;
        }

        /// <summary>
        /// TEST if friendly piece at destination
        /// CMP piece.Color == this.Color AND Type != None
        /// RET true if friendly detected
        /// </summary>
        protected bool IsFriendlyPieceAtPosition(int row, int column, Piece[,] board)
        {
            if (row < 0 || row > 7 || column < 0 || column > 7) return false;
            var piece = board[row, column];
            return piece != null && piece.Type != PieceType.None && piece.Color == this.Color;
        }

        /// <summary>
        /// TEST if square is empty
        /// CMP board[row,col] == NULL OR Type == None
        /// RET true if empty
        /// </summary>
        protected bool IsPositionEmpty(int row, int column, Piece[,] board)
        {
            if (row < 0 || row > 7 || column < 0 || column > 7) return false;
            return board[row, column] == null || board[row, column].Type == PieceType.None;
        }

        /// <summary>
        /// LOOP through path from source to dest
        /// MOV CX, distance // DEC CX // JNZ loop_check_empty
        /// RET true if path clear, false if obstructed
        /// </summary>
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

        /// <summary>
        /// PUSH formatted piece descriptor to output
        /// </summary>
        public override string ToString()
        {
            return $"{Type} ({Color})";
        }

        #endregion
    }
}
