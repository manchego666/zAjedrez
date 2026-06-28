using zAjedrez.Model.Enums;
using zAjedrez.Model.Contract;
using System;

namespace zAjedrez.Model.Struct
{
    // KNIGHT: MOV in L-shape (2,1) or (1,2) deltas
    // JUMP: only piece that ignores path obstruction
    // CMP |row_delta|, 2 AND |col_delta|, 1 OR |row_delta|, 1 AND |col_delta|, 2
    // NO IsPathClear() call needed
    // Attack vector: (2,1), (2,-1), (-2,1), (-2,-1), (1,2), (1,-2), (-1,2), (-1,-2)
    public class KnightPiece : Piece
    {
        #region Constructor

        public KnightPiece(PieceColor color, int row, int column) : base(color, row, column)
        {
            Type = PieceType.Knight;
        }

        #endregion

        #region Methods

        // KNIGHT MOVEMENT VALIDATION
        // MOV AX, ABS(destRow - Row) // rowDiff
        // MOV BX, ABS(destColumn - Column) // colDiff
        // CMP AX, 2 AND BX, 1 // JE valid_l_shape
        // CMP AX, 1 AND BX, 2 // JE valid_l_shape
        // JMP invalid_move
        // valid_l_shape: RET true
        public override bool IsMoveLegal(int destRow, int destColumn, Piece[,] board)
        {
            if (destRow < 0 || destRow > 7 || destColumn < 0 || destColumn > 7)
                return false;

            if (destRow == Row && destColumn == Column)
                return false;

            if (IsFriendlyPieceAtPosition(destRow, destColumn, board))
                return false;

            int rowDiff = Math.Abs(destRow - Row);
            int colDiff = Math.Abs(destColumn - Column);

            return (rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2);
        }

        protected override string GetImagePath()
        {
            return $"/Assets/Pieces/{(Color == PieceColor.White ? "White" : "Black")}_Knight.png";
        }

        #endregion
    }
}
