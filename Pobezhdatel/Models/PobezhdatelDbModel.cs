using System;
using log4net;
using Pobezhdatel.DB;

namespace Pobezhdatel.Models
{
    /// <summary>
    /// Model for communication with database.
    /// </summary>
    public class PobezhdatelDbModel
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PobezhdatelDbModel));

        /// <summary>
        /// Constant of maximum player name length.
        /// </summary>
        public static int MaxNameLength = 50;

        /// <summary>
        /// Constant of maximum mesage length.
        /// </summary>
        public static int MaxMessageLength = 1000;

        /// <summary>
        /// Add a new message to the DB.
        /// </summary>
        /// <param name="message">Object with data of the message.</param>
        /// <returns>True if operation is successfull.</returns>
        public bool AddMessage(MessageModel message)
        {
            Log.Debug("AddMessage");

            // Check parameters before addition
            if (!message.IsCorrect() || message.PlayerName.Length > MaxNameLength ||
                message.Text.Length > MaxMessageLength)
            {
                return false;
            }

            try
            {
                using (var db = new PobezhdatelDbDataContext())
                {
                    db.T_Messages.InsertOnSubmit(new T_Message
                                                 {
                                                     Text = message.Text,
                                                     Timestamp = message.Timestamp,
                                                     PlayerName = message.PlayerName,
                                                     RoomId = 1         // TODO: hardcoded for now
                                                 });

                    db.SubmitChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("AddMessage", ex);
                return false;
            }
        }
    }
}