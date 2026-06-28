using System;
using System.Collections.Generic;
using System.Text;
using zAjedrez.Model.Enums;

namespace zAjedrez.Model.Entity
{
    /// <summary>
    /// Representa una clase para la pieza de ajedrez,
    /// que hereda de plantilla y debe aplicar Contratos (interfaces).
    /// </summary>
    internal class PiecesSphere : PieceTables
    {
        public ColorType Color { get; set; }
        public PieceType Type { get; set; }
        public int Value { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }

    }
}
