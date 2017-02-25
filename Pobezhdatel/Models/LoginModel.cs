using System.ComponentModel.DataAnnotations;

namespace Pobezhdatel.Models
{
    /// <summary>
    /// Simple login model.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Name of created room or room to log in
        /// </summary>
        [Required]
        public string RoomName { get; set; }

        /// <summary>
        /// Name of a player in the room.
        /// </summary>
        [Required]
        public string PlayerName { get; set; }
    }
}