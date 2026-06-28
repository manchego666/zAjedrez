namespace zAjedrez.Model.Contract
{
    /// <summary>
    /// IPiece contract interface
    /// Defines polymorphic behavior for all chess piece types
    /// </summary>
    public interface IPiece
    {
        #region Properties

        /// <summary>
        /// Validate if move from source to destination is legal per piece rules
        /// </summary>
        bool IsMoveLegal(int destRow, int destColumn, Piece[,] board);

        /// <summary>
        /// Get image path for piece sprite rendering
        /// </summary>
        string GetImagePath();

        #endregion
    }
}
