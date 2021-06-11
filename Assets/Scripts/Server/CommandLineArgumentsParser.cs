using System.Collections.Generic;

namespace Balloondle.Server
{
    public class CommandLineArgumentsParser
    { 
        public Dictionary<string, string> GetExpectedCommandLineArguments(string[] commandLineArguments, int expectedArguments)
        {
            Dictionary<string, string> arguments = new Dictionary<string, string>();

            if (commandLineArguments.Length < expectedArguments)
            {
                throw new
                    System.InvalidOperationException("Failed to acquire required arguments.");
            }

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

        private bool IsArgumentAParameterIndicator(string argument)
        {
            return argument.StartsWith("-");
        }
    }
}
