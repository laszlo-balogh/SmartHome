using VoiceAssistant.Models;

namespace VoiceAssistant.Helpers
{
    public interface ICommandBuffer
    {
        void AddOrUpdateCommand(Command command);
        string GetLatestCommandSentence();
    }
}