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
        /// String with results of dices rolling.
        /// </summary>
        public string DicesRollResult { get; set; }

        /// <summary>
        /// Create an empty message.
        /// </summary>
        public MessageModel() { }

        /// <summary>
        /// Create a specified message.
        /// </summary>
        /// <param name="timestamp">Date and time of the message sending.</param>
        /// <param name="playerName">Name of player that sent the message.</param>
        /// <param name="text">Text of the message.</param>
        /// <param name="dicesRollResult">String with results of dices rolling.</param>
        /// <param name="id">Id of the message (used only for DB).</param>
        public MessageModel(DateTime timestamp, string playerName, string text, string dicesRollResult, int id = 0)
        {
            Timestamp = timestamp;
            PlayerName = playerName;
            Text = text;
            DicesRollResult = dicesRollResult;
            Id = id;
        }

        /// <summary>
        /// Check is data of this message correct.
        /// </summary>
        /// <returns>True if correct.</returns>
        public bool IsCorrect()
        {
            return !(Timestamp < new DateTime(2010, 01, 01) || string.IsNullOrWhiteSpace(PlayerName) ||
                   string.IsNullOrWhiteSpace(Text));
        }
    }
}