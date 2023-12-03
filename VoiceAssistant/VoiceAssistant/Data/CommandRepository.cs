using System.Xml.Linq;
using VoiceAssistant.Models;

namespace VoiceAssistant.Data
{
    public class CommandRepository : ICommandRepository
    {
        public List<CommandDataModel>? Commands { get; set; }
        private readonly IConfiguration config;
        public CommandRepository()
        {
            config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            Refresh();
        }

        public List<CommandDataModel> GetCommands()
        {
            return this.Commands;
        }

        public void Refresh()
        {
            Commands = new List<CommandDataModel>();
            var commandXmlPath = config.GetValue<string>("CommandXmlPath");
            var xdoc = XDocument.Load("Data/commands.xml");
            foreach (var item in xdoc.Descendants("Command"))
            {
                Commands.Add(new CommandDataModel()
                {
                    CommandRec = item.Value,
                    CommandCode = item.Attribute("toSend").Value
                });
            }
        }
    }
}
