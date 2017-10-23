using CommandLine;

namespace Test.Listener
{
    class CmdOptions
    {
        [Option('n', "name", DefaultValue = "test",  HelpText = "Name of the client.")]
        public string Name { get; set; }
    }
}
