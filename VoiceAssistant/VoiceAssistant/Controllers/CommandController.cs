using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using VoiceAssistant.Data;
using VoiceAssistant.Helpers;
using VoiceAssistant.Models;

namespace VoiceAssistant.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class CommandController : ControllerBase
    {
        private readonly ICommandRepository commandRepository;
        private readonly HttpClient _httpClient;
        private readonly ICommandBuffer commandBuffer;
        public IHomeCondition HomeCondition { get; set; }
        public CommandController(ICommandRepository commandRepository, IHttpClientFactory httpClientFactory, ICommandBuffer commandBuffer, IHomeCondition homeCondition)
        {
            this.commandRepository = commandRepository;
            this.commandBuffer = commandBuffer;
            this._httpClient = httpClientFactory.CreateClient();
            this.HomeCondition = homeCondition;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var commandsList = this.commandRepository.GetCommands();
                return Ok(commandsList);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Command command)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");            

            if (command != null && command.CommandSentence != null && command.Id != null && command.Source != null && (command.Source == "react" || command.Source =="python"))
            {
                commandBuffer.AddOrUpdateCommand(command);

                string latestCommandSentence = commandBuffer.GetLatestCommandSentence();

                if (!string.IsNullOrEmpty(latestCommandSentence))
                {                    
                    await Task.Delay(1500);
                }

                switch (latestCommandSentence)
                {
                    case "bongeszo":
                        HomeCondition.OpenBrowser();
                        break;
                    case "fazok":
                        HomeCondition.Warmer(latestCommandSentence);
                        break;
                    case "melegem_van":
                        HomeCondition.Colder(latestCommandSentence);
                        break;
                    case "homerseklet_alap":
                        HomeCondition.TemperatureToBasicSituation(latestCommandSentence);
                        break;
                    case "homerseklet_vissza":
                        HomeCondition.ResetBackTemperature(latestCommandSentence);
                        break;
                    case "klima_be":
                        HomeCondition.TurnAirConditionerOn(latestCommandSentence);
                        break;
                    case "klima_le":
                        HomeCondition.TurnAirConditionerOff(latestCommandSentence);
                        break;
                    case "mennyi_az_ido":
                        HomeCondition.WhatsTheTime(latestCommandSentence);
                        break;
                    case "vicc":
                        HomeCondition.TellAJoke(latestCommandSentence);
                        break;
                    case "villany_fel":
                        HomeCondition.TurnLightsOn(latestCommandSentence);
                        break;
                    case "villany_le":
                        HomeCondition.TurnLightsOff(latestCommandSentence);
                        break;
                    case "nincs_match":
                        break;
                    default:
                        break;
                }                

                return Ok();


            }
            else
            {
                return BadRequest("Command was null or with invalid source");
            }
        }

    }
}




