namespace VoiceAssistant.Models
{
    public class Command
    {
        public string? Id { get; set; }
        public string? Source { get; set; }
        public string? CommandSentence { get; set; }

        public Command(string? id, string? commandSentence,string? source)
        {
            this.Id = id;
            this.CommandSentence = commandSentence;
            this.Source = source;
        }
    }
}
