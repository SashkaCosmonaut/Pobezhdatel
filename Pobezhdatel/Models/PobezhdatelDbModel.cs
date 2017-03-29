using System;
using System.Linq;
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
                    // Find room in DB
                    var dbRoom = db.T_Rooms.SingleOrDefault(q => q.Name == message.RoomName);

                    if (dbRoom == null)     // If such room doesn't exist, create it
                    {
                        dbRoom = new T_Room
                        {
                            Name = message.RoomName,
                            Password = ""
                        };

                        db.T_Rooms.InsertOnSubmit(dbRoom);
                    }

                    db.T_Messages.InsertOnSubmit(new T_Message
                                                 {
                                                     Text = message.Text,
                                                     Timestamp = message.Timestamp,
                                                     PlayerName = message.PlayerName,
                                                     DicesRollResult = message.DicesRollResult,
                                                     T_Room = dbRoom
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

        /// <summary>
        /// Get all existed messages from DB.
        /// </summary>
        /// <param name="roomName">Current game room name.</param>
        /// <returns>Array of message models from DB.</returns>
        public MessageModel[] GetMessages(string roomName)
        {
            Log.Debug("GetMessages");

            try
            {
                using (var db = new PobezhdatelDbDataContext())
                {
                    return
                        db.T_Messages.Where(q => q.T_Room.Name == roomName)
                            .Select(
                                q =>
                                    new MessageModel(q.Timestamp, q.T_Room.Name, q.PlayerName, q.Text, q.DicesRollResult,
                                        q.Id))
                            .ToArray();
                }
            }
            catch (Exception ex)
            {
                Log.Error("GetMessages", ex);
                return new MessageModel[0];
            }
        }
    }
}