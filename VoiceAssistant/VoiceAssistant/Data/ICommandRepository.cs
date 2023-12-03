using VoiceAssistant.Models;

namespace VoiceAssistant.Data
{
    public interface ICommandRepository
    {
        List<CommandDataModel>? Commands { get; set; }

        List<CommandDataModel> GetCommands();
        void Refresh();
    }
}