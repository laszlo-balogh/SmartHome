using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
namespace VoiceAssistant.Models
{
    public class HomeCondition : IHomeCondition
    {
        public int Temperature { get; set; } = 16;
        public bool AirConditionerOn { get; set; } = false;
        public bool LightsOn { get; set; } = false;

        public bool TemperatureBasicSituation = false;

        private readonly IConfiguration config;

        public HomeCondition()
        {
            config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }

        public void TurnLightsOn(string command)
        {
            if (!this.LightsOn)
            {
                SendApiCall(MapCommandToApiCall(command));
                this.LightsOn = true;
            }
        }

        public void TurnLightsOff(string command)
        {
            if (this.LightsOn)
            {
                SendApiCall(MapCommandToApiCall(command));
                this.LightsOn = false;
            }
        }

        public string TellAJoke(string command)
        {
            SendApiCall(MapCommandToApiCall(command));
            return "haha";
        }

        public string WhatsTheTime(string command)
        {
            SendApiCall(MapCommandToApiCall(command));
            var hour = DateTime.Now.Hour.ToString();
            var minute = DateTime.Now.Minute.ToString();

            return $"{hour}:{minute}";

        }

        public void TurnAirConditionerOn(string command)
        {
            if (!this.AirConditionerOn)
            {
                SendApiCall(MapCommandToApiCall(command));
                this.AirConditionerOn = true;
            }
        }

        public void TurnAirConditionerOff(string command)
        {
            if (this.AirConditionerOn)
            {
                SendApiCall(MapCommandToApiCall(command));
                this.AirConditionerOn = false;
            }
        }

        public void OpenBrowser()
        {
            string chromePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            string url = "https://www.google.com";
            Process.Start(chromePath, url);
        }

        public void TemperatureToBasicSituation(string command)
        {
            if (!this.TemperatureBasicSituation)
            {
                SendApiCall(MapCommandToApiCall(command));
                var xmlDoc = new XmlDocument();
                var homeXMlPath = config.GetValue<string>("HomeXmlPath");
                xmlDoc.Load(homeXMlPath);

                var root = xmlDoc.DocumentElement;

                var temperatureElement = root.SelectSingleNode(nameof(this.Temperature));
                var airConditionerOnElement = root.SelectSingleNode(nameof(this.AirConditionerOn));
                var lightsOnElement = root.SelectSingleNode(nameof(this.LightsOn));
                var temperatureBasicSituationElement = root.SelectSingleNode(nameof(this.TemperatureBasicSituation));

                temperatureElement.InnerText = this.Temperature.ToString();
                airConditionerOnElement.InnerText = this.AirConditionerOn.ToString();
                lightsOnElement.InnerText = this.LightsOn.ToString();
                temperatureBasicSituationElement.InnerText = this.TemperatureBasicSituation.ToString();

                xmlDoc.Save(homeXMlPath);
                this.TemperatureBasicSituation = true;
                this.Temperature = 16;
            }
        }

        public void ResetBackTemperature(string command)
        {
            SendApiCall(MapCommandToApiCall(command));
            var xmlDoc = new XmlDocument();
            var homeXMlPath = config.GetValue<string>("HomeXmlPath");
            xmlDoc.Load(homeXMlPath);

            var root = xmlDoc.DocumentElement;

            var temperatureElement = root.SelectSingleNode(nameof(this.Temperature)).InnerText;
            var airConditionerOnElement = root.SelectSingleNode(nameof(this.AirConditionerOn)).InnerText;
            var lightsOnElement = root.SelectSingleNode(nameof(this.LightsOn)).InnerText;
            var temperatureBasicSituationElement = root.SelectSingleNode(nameof(this.TemperatureBasicSituation)).InnerText;

            this.TemperatureBasicSituation = bool.Parse(temperatureBasicSituationElement);
            this.Temperature = int.Parse(temperatureElement);
        }

        public void Warmer(string command)
        {
            SendApiCall(MapCommandToApiCall(command));
            this.Temperature += 1;
        }

        public void Colder(string command)
        {
            SendApiCall(MapCommandToApiCall(command));
            this.Temperature -= 1;
        }

        string MapCommandToApiCall(string command)
        {

            return $"api/services/{command}";
        }

        async Task SendApiCall(string apiCall)
        {
            if (string.IsNullOrEmpty(apiCall))
            {
                Console.WriteLine("Invalid command");
                return;
            }

            string homeAssistantUrl = "http://homeassistant.local:8123";
            string accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiI5Y2Q5YzAxZjA1NGI0NTdiOTQyZDQ3MjhiOGM2YzQ3OCIsImlhdCI6MTcwMTQ2ODQyMCwiZXhwIjoyMDE2ODI4NDIwfQ.h9EM249ORGPyhs230ez0FTxJMHaWOJMjEo-TXFnKBME";

            string apiUrl = $"{homeAssistantUrl}/{apiCall}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Command done{apiCall}!");
                    }
                    else
                    {
                        Console.WriteLine($"Request error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}

