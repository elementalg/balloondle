using System.Collections.Generic;
using Balloondle.Server;
using NUnit.Framework;

public class CommandLineArgumentsParserTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void RetrievesArgumentsCorrectlyFromCommandLine()
    {
        string[] testCommandLineArguments = new string[] { "program.exe", "-port", "7777" };
        CommandLineArgumentsParser parser = new CommandLineArgumentsParser();
        Dictionary<string, string> arguments = parser.GetExpectedCommandLineArguments(testCommandLineArguments, testCommandLineArguments.Length);

        Assert.IsTrue(arguments.ContainsKey("port"));
    }
}
