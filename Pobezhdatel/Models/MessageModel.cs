using System;

namespace Pobezhdatel.Models
{
    /// <summary>
    /// Model of game chat message.
    /// </summary>
    public class MessageModel
    {
        /// <summary>
        /// Identifier of the message.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date and time of the message sending.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Name of player that sent the message.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Text of the message.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Check is data of this message correct.
        /// </summary>
        /// <returns>True if correct.</returns>
        public bool IsCorrect()
        {
            return Timestamp > new DateTime(2010, 01, 01) && string.IsNullOrWhiteSpace(PlayerName) &&
                   string.IsNullOrWhiteSpace(Text);
        }
    }
}