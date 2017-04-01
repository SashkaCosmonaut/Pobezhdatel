using log4net;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Pobezhdatel.Hubs
{
    /// <summary>
    /// Static class with utilities for game chat.
    /// </summary>
    public static class Utilities
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Utilities));

        /// <summary>
        /// Find requests for dices rolling and roll dices.
        /// </summary>
        /// <param name="message">Message with possible dice rolling requests.</param>
        /// <returns>Result of dice rolling or null if there are no requests.</returns>
        public static string DiceRoll(string message)
        {
            Log.Debug("DiceRoll");

            try
            {
                var matches = Regex.Matches(message, "//[1-9]?(d|u)(100|[1-9][0-9]?)");

                // Roll every dice roll request and aggreagte all results to one string
                return matches.Cast<Match>()
                    .Select(q => GetDiceRollText(q.Value))
                    .Aggregate((current, next) => current + "\n" + next).TrimEnd('\n');
            }
            catch (Exception ex)
            {
                Log.Error("", ex);
                return null;
            }
        }

        /// <summary>
        /// Get text of dice roll in accordance with a requiest.
        /// Parse dice requests in a message text and append it to text.
        /// Requiest pattern: //XdY, where X - number of dices, Y - number of dice edges. 
        /// Number of dices can be 1-9, number of edges can be 1-100 (from 1d1 to 9d100).
        /// </summary>
        /// <param name="diceRequest">Text with dice request.</param>
        private static string GetDiceRollText(string diceRequest)
        {
            Log.Debug("GetDiceRollText");

            try
            {
                var numberOfDices = 1;                          // How many times to roll the dice
                var numberOfEdges = 1;                          // Range of rolled numbers
                var rangeStart = 1;                             // Start of the range of rolled numbers
                var isNegative = diceRequest.Contains('u');     // Flag is dice should contain zero and negative results

                // Parse request
                var requestParts = diceRequest.TrimStart('/').Split(new[] { 'd', 'u' }, StringSplitOptions.RemoveEmptyEntries);

                // If there is more than one dice, take the second part for edges else take the first
                if (requestParts.Length > 1)
                {
                    numberOfDices = int.Parse(requestParts[0]);
                    numberOfEdges = int.Parse(requestParts[1]);
                }
                else
                {
                    numberOfEdges = int.Parse(requestParts[0]);
                }

                var random = new Random(Guid.NewGuid().GetHashCode());
                var result = diceRequest + ": ";

                if (isNegative)
                    rangeStart = -numberOfEdges;

                for (var i = 0; i < numberOfDices; i++)         // Roll each dice in this request
                {
                    result += random.Next(rangeStart, numberOfEdges + 1) + ", ";
                }

                return result.TrimEnd(' ', ',');
            }
            catch (Exception ex)
            {
                Log.Error("GetDiceRollText", ex);
                return "";
            }
        }
    }
}