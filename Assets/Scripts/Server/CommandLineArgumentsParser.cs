using System.Collections.Generic;

namespace Balloondle.Server
{
    /// <summary>
    /// Gets the arguments from the command line, and proceeds to generate a map of them
    /// through a dictionary, being the keys the name of the arguments, and the values the
    /// values of said arguments.
    /// </summary>
    public class CommandLineArgumentsParser
    { 
        /// <summary>
        /// Retrieves the expected amount of command line arguments.
        /// </summary>
        /// <param name="commandLineArguments">Arguments from the command line.</param>
        /// <param name="expectedArguments">Amount of arguments which are expected.</param>
        /// <returns></returns>
        public Dictionary<string, string> GetExpectedCommandLineArguments(string[] commandLineArguments,
            int expectedArguments)
        {
            Dictionary<string, string> arguments = new Dictionary<string, string>();

            if (commandLineArguments.Length < expectedArguments)
            {
                throw new
                    System.InvalidOperationException("Failed to acquire required arguments.");
            }

            // Go one by one argument from the command line.
            for (int i = 0; i < commandLineArguments.Length; i++)
            {
                string argument = commandLineArguments[i].ToLower();

                if (IsArgumentAParameterIndicator(argument))
                {
                    argument = argument.Replace("-", "");

                    if (i + 1 < commandLineArguments.Length)
                    {
                        string nextArgument = commandLineArguments[i + 1];

                        if (IsArgumentAParameterIndicator(nextArgument))
                        {
                            arguments.Add(argument, "");
                        }
                        else
                        {
                            arguments.Add(argument, nextArgument);
                        }
                    }
                }
            }

            return arguments;
        }

        /// <summary>
        /// Detect whether the argument is a parameter or a value.
        /// </summary>
        /// <param name="argument">string value from the command line.</param>
        /// <returns>True if it starts with '-'.</returns>
        private bool IsArgumentAParameterIndicator(string argument)
        {
            return argument.StartsWith("-");
        }
    }
}
