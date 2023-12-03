namespace VoiceAssistant.Models
{
    public interface IHomeCondition
    {
        bool AirConditionerOn { get; set; }
        bool LightsOn { get; set; }
        int Temperature { get; set; }

        void Colder(string command);
        void OpenBrowser();
        void ResetBackTemperature(string command);
        string TellAJoke(string command);
        void TemperatureToBasicSituation(string command);
        void TurnAirConditionerOff(string command);
        void TurnAirConditionerOn(string command);
        void TurnLightsOff(string command);
        void TurnLightsOn(string command);
        void Warmer(string command);
        string WhatsTheTime(string command);
    }
}