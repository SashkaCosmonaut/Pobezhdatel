using System.ComponentModel.DataAnnotations;

namespace Pobezhdatel.Models
{
    /// <summary>
    /// Model of a player for login to the system.
    /// </summary>
    public class PlayerLoginModel
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