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
                var matches = Regex.Matches(message, "//[1-9]?(d|u)(100|[1-9][0-9]?)(( *)(\\+|\\-)( *)[0-9]+)?");

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
                var sum = 0;                                    // Sum of all dice rolls
                var isNegative = diceRequest.Contains('u');     // Flag is dice should contain zero and negative results
                var additionValue = 0;                          // Addition value that is added to the dice roll result

                // Get index of "+" or "-" sign for addition value and positive or negative addition value itself
                var indexOfAdditionSign = GetAdditionValue(diceRequest, out additionValue);

                // Parse request from start till possible addition sign
                var requestParts = diceRequest.Substring(0, indexOfAdditionSign)
                                              .TrimStart('/')
                                              .TrimEnd(' ')
                                              .Split(new[] { 'd', 'u' }, StringSplitOptions.RemoveEmptyEntries);

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
                var result = diceRequest + ":\t";

                if (isNegative)
                    rangeStart = -numberOfEdges;

                for (var i = 0; i < numberOfDices; i++)         // Roll each dice in this request
                {
                    var newRoll = random.Next(rangeStart, numberOfEdges + 1);

                    sum += newRoll;
                    result += newRoll + ", ";
                }

                // Prepare result
                result = result.TrimEnd(' ', ',');                  // Remove trailing symbols 

                if (indexOfAdditionSign != diceRequest.Length)      // Show addition value if it is
                {
                    result += (additionValue > 0 ? " + " : " - ") + Math.Abs(additionValue);
                }

                return result + " = " + (sum + additionValue);      // Add the roll sum
            }
            catch (Exception ex)
            {
                Log.Error("GetDiceRollText", ex);
                return "";
            }
        }

        /// <summary>
        /// Get value that is added to a dice rolling
        /// </summary>
        /// <param name="diceRequest">Text with dice request.</param>
        /// <param name="additionValue">Positive or negative integer addition value.</param>
        /// <returns>Index of addition symbol (+ or -) or request string length if there is no such symbols.</returns>
        private static int GetAdditionValue(string diceRequest, out int additionValue)
        {
            var plusIndex = diceRequest.IndexOf('+');       // Index of plus sign for addition a value to dice roll 

            if (plusIndex != -1)    // If there is plus sign then there is positive addition value
            {
                additionValue = int.Parse(diceRequest.Substring(plusIndex + 1).Trim(' '));
                return plusIndex;
            }

            var minusIndex = diceRequest.IndexOf('-');      // Index of minus sign for subtraction a value from dice roll

            if (minusIndex != -1)   // If there is minus sign then there is negative addition value
            {
                // With minus sign
                additionValue = -int.Parse(diceRequest.Substring(minusIndex + 1).Trim(' '));
                return minusIndex;
            }

            additionValue = 0;

            return diceRequest.Length;
        }
    }
}