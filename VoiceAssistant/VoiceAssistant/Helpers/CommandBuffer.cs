using VoiceAssistant.Models;

namespace VoiceAssistant.Helpers
{
    public class CommandBuffer : ICommandBuffer
    {
        private readonly Dictionary<string, (DateTime Timestamp, string CommandSentence)> buffer = new Dictionary<string, (DateTime, string)>();
        private readonly object lockObject = new object();

        public void AddOrUpdateCommand(Command command)
        {
            lock (lockObject)
            {
                buffer[command.Source] = (DateTime.UtcNow, command.CommandSentence);
            }
        }

        public string GetLatestCommandSentence()
        {
            lock (lockObject)
            {
                DateTime now = DateTime.UtcNow;
                DateTime threshold = now.AddSeconds(-1.5);

                if (buffer.TryGetValue("react", out var reactCommand) && reactCommand.Timestamp > threshold)
                {
                    return reactCommand.CommandSentence;
                }
                else if (buffer.TryGetValue("python", out var pythonCommand) && pythonCommand.Timestamp > threshold)
                {
                    return pythonCommand.CommandSentence;
                }

                return null;
            }
        }
    }
}
