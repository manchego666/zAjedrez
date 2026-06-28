using System;
using System.Collections.Generic;
using System.Text;
/// <summary>
/// This namespace contains the User entity class for the chess game model,
/// which represents a user with properties such as Name, Id, PassHash, and RuthImg.
/// </summary>
namespace zAjedrez.Model.Entity
{
    /// <summary>
    /// Represent a user in the chess game model, with properties for Name, Id, PassHash, and RuthImg.
    /// </summary>
    internal class User
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        #region Properties
        public string Name { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public string PassHash { get; set; } = string.Empty;
        public string RuthImg { get; set; } = string.Empty;
        #endregion ¨Properties
    }
}
