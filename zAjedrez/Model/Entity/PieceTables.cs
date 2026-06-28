using System;
using System.Collections.Generic;
using System.Text;

namespace zAjedrez.Model.Entity
{
    /// <summary>
    /// Representa la clase de pieza de ajedrez, en parte virtual (datos)
    /// y en parte concreta (implementación de la clase PiecesSphere).
    /// intentando aplicar herencia y polimorfismo,
    /// para que las piezas de ajedrez puedan compartir propiedades y métodos comunes.
    /// debe tener nombre y ruta de imagen, y ser heredada por la clase PiecesSphere.
    /// la interfaz de la pieza que hereda esta plantilla debe implementar contratos simples
    /// para la ejecución de movimientos y validación de reglas del juego.
    /// y cada objeto de pieceSphere sobreescribe los métodos de la plantilla para su comportamiento específico.
    /// debe tener color y tipo de pieza, valor y posición en el tablero.
    /// </summary>
    internal abstract class PieceTables
    {
        public string Name { get; set; } = string.Empty;
        public string pathImg { get; set; } = string.Empty;
        
    }
}
