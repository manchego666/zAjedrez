using zAjedrez.Model.Enums;

namespace zAjedrez.Model.Contract
{
    // PUSH RBP // MOV RBP, RSP
    // Abstract base contract for all piece types
    // Each derived class implements IsMoveLegal() logic
    // Polymorphic dispatch on piece type
    public abstract class Piece
    {
        #region Properties

        public PieceType Type { get; protected set; }      // MOV AX, Type
        public PieceColor Color { get; protected set; }    // MOV BX, Color
        public int Row { get; set; }                       // MOV CX, Row
        public int Column { get; set; }                    // MOV DX, Column
        public bool HasMoved { get; set; }                 // MOV AL, HasMoved
        public string ImagePath { get; protected set; }    // PUSH ImagePath

        #endregion

        #region Constructor

        // CALL __ctor__ // POP return address
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

        // CALL virtual IsMoveLegal(destRow, destColumn, board)
        // JMP to derived implementation via v-table
        // RET with AL=1 (legal) or AL=0 (illegal)
        public abstract bool IsMoveLegal(int destRow, int destColumn, Piece[,] board);

        // CALL virtual GetImagePath()
        // PUSH path string to stack
        // RET
        protected abstract string GetImagePath();

        #endregion

        #region Methods

        // TEST board[row,col].Type != None AND board[row,col].Color != this.Color
        // RET ZF if enemy detected
        protected bool IsEnemyPieceAtPosition(int row, int column, Piece[,] board)
        {
            if (row < 0 || row > 7 || column < 0 || column > 7) return false;
            var piece = board[row, column];
            return piece != null && piece.Type != PieceType.None && piece.Color != this.Color;
        }

        // CMP board[row,col].Color == this.Color AND Type != None
        // JZ friendly_piece // RET
        protected bool IsFriendlyPieceAtPosition(int row, int column, Piece[,] board)
        {
            if (row < 0 || row > 7 || column < 0 || column > 7) return false;
            var piece = board[row, column];
            return piece != null && piece.Type != PieceType.None && piece.Color == this.Color;
        }

        // TEST board[row,col] == NULL OR Type == None
        // RET CF if empty
        protected bool IsPositionEmpty(int row, int column, Piece[,] board)
        {
            if (row < 0 || row > 7 || column < 0 || column > 7) return false;
            return board[row, column] == null || board[row, column].Type == PieceType.None;
        }

        // LOOP through path from source to dest
        // MOV CX, distance // DEC CX // JNZ loop_check_empty
        // RET CF if path obstructed
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

        // PUSH Type descriptor to output stream
        // PUSH Color descriptor
        public override string ToString()
        {
            return $"{Type} ({Color})";
        }

        #endregion
    }
}
