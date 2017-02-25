using System.ComponentModel.DataAnnotations;

namespace Pobezhdatel.Models
{
    /// <summary>
    /// Simple model of game settings.
    /// </summary>
    public class GameModel
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