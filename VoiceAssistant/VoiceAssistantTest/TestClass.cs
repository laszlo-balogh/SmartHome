using System.Net.Http;
using System.Runtime.CompilerServices;
using Castle.Components.DictionaryAdapter.Xml;
using Moq;
using NUnit.Framework;
using VoiceAssistant.Controllers;
using VoiceAssistant.Data;
using VoiceAssistant.Helpers;
using VoiceAssistant.Models;

namespace VoiceAssistantTest
{

    [TestFixture]
    public class TestClass
    {
        private HomeCondition homeCondition;
        private Mock<IHomeCondition> homeConditionMock;

        [SetUp]
        public void Init()
        {
            this.homeCondition = new HomeCondition();
            this.homeConditionMock = new Mock<IHomeCondition>(MockBehavior.Loose);
            this.homeConditionMock.Setup(h => h.OpenBrowser());
            this.homeConditionMock.Setup(h => h.Warmer("fazok"));
            this.homeConditionMock.Setup(h => h.Colder("melegem_van"));
            this.homeConditionMock.Setup(h => h.TemperatureToBasicSituation("homerseklet_alap"));
            this.homeConditionMock.Setup(h => h.ResetBackTemperature("homerseklet_vissza"));
            this.homeConditionMock.Setup(h => h.TurnAirConditionerOn("klima_be"));
            this.homeConditionMock.Setup(h => h.TurnAirConditionerOff("klima_le"));
            this.homeConditionMock.Setup(h => h.WhatsTheTime("mennyi_az_ido"));
            this.homeConditionMock.Setup(h => h.TellAJoke("vicc"));
            this.homeConditionMock.Setup(h => h.TurnLightsOn("villany_fel"));
            this.homeConditionMock.Setup(h => h.TurnLightsOff("villany_le"));            
        }

        [Test]
        public void OpenBrowserShouldCalledOnce()
        {                        
            this.homeConditionMock.Object.OpenBrowser();
            this.homeConditionMock.Verify(x=> x.OpenBrowser(), Times.Once);
           
        }

        [Test]
        public void WarmerShouldCalledOnce()
        {
            this.homeConditionMock.Object.Warmer("fazok");
            this.homeConditionMock.Verify(x => x.Warmer("fazok"), Times.Once);

        }

        [Test]
        public void ColderShouldCalledOnce()
        {
            this.homeConditionMock.Object.Colder("melegem_van");
            this.homeConditionMock.Verify(x => x.Colder("melegem_van"), Times.Once);

        }

        [Test]
        public void TemperatureToBasicSituationShouldCalledOnce()
        {
            this.homeConditionMock.Object.TemperatureToBasicSituation("homerseklet_alap");
            this.homeConditionMock.Verify(x => x.TemperatureToBasicSituation("homerseklet_alap"), Times.Once);

        }

        [Test]
        public void ResetBackTemperatureShouldCalledOnce()
        {
            this.homeConditionMock.Object.ResetBackTemperature("homerseklet_vissza");
            this.homeConditionMock.Verify(x => x.ResetBackTemperature("homerseklet_vissza"), Times.Once);
        }

        [Test]
        public void TurnAirConditionerOnShouldCalledOnce()
        {
            this.homeConditionMock.Object.TurnAirConditionerOn("klima_be");
            this.homeConditionMock.Verify(x => x.TurnAirConditionerOn("klima_be"), Times.Once);

        }

        [Test]
        public void TurnAirConditionerOffShouldCalledOnce()
        {
            this.homeConditionMock.Object.TurnAirConditionerOff("klima_le");
            this.homeConditionMock.Verify(x => x.TurnAirConditionerOff("klima_le"), Times.Once);

        }

        [Test]
        public void WhatsTheTimeShouldCalledOnce()
        {
            this.homeConditionMock.Object.WhatsTheTime("mennyi_az_ido");
            this.homeConditionMock.Verify(x => x.WhatsTheTime("mennyi_az_ido"), Times.Once);

        }

        [Test]
        public void TellAJokeShouldCalledOnce()
        {
            this.homeConditionMock.Object.TellAJoke("vicc");
            this.homeConditionMock.Verify(x => x.TellAJoke("vicc"), Times.Once);
        }

        [Test]
        public void TurnLightsOnShouldCalledOnce()
        {
            this.homeConditionMock.Object.TurnLightsOn("villany_fel");
            this.homeConditionMock.Verify(x => x.TurnLightsOn("villany_fel"), Times.Once);
        }

        [Test]
        public void TurnLightsOffShouldCalledOnce()
        {
            this.homeConditionMock.Object.TurnLightsOff("villany_le");
            this.homeConditionMock.Verify(x => x.TurnLightsOff("villany_le"), Times.Once);
        }

        [Test]
        public void WarmerWithValidInputShouldIncreaseTemperature()
        {
            int initialTemperature = this.homeCondition.Temperature;

            this.homeCondition.Warmer("fazok");
         
            Assert.That(this.homeCondition.Temperature, Is.GreaterThan(initialTemperature));
        }

        [Test]
        public void ColderWithValidInputShouldDecreaseTemperature()
        {            
            int initialTemperature = this.homeCondition.Temperature;

            this.homeCondition.Colder("melegem_van");

            Assert.That(this.homeCondition.Temperature, Is.LessThan(initialTemperature));
        }

        [Test]
        public void LightsOnLightsAreOnShouldNotTurnOffThem()
        {
            this.homeCondition.LightsOn = true;

            this.homeCondition.TurnLightsOn("Villany_fel");

            Assert.That(this.homeCondition.LightsOn, Is.EqualTo(true));
        }

        [Test]
        public void LightsOnLightsAreOffShouldTurnOnThem()
        {
            this.homeCondition.LightsOn = false;

            this.homeCondition.TurnLightsOn("Villany_fel");

            Assert.That(this.homeCondition.LightsOn, Is.EqualTo(true));
        }

        [Test]
        public void LightsOffLightsAreOffShouldNotTornOnThem()
        {
            this.homeCondition.LightsOn = false;

            this.homeCondition.TurnLightsOff("Villany_le");

            Assert.That(this.homeCondition.LightsOn, Is.EqualTo(false));
        }

        [Test]
        public void LightsOffLightsAreOnShouldTurnOffThem()
        {
            this.homeCondition.LightsOn = true;

            this.homeCondition.TurnLightsOff("Villany_le");

            Assert.That(this.homeCondition.LightsOn, Is.EqualTo(false));
        }

        [Test]
        public void AirConditionerOnAirConditionerAreOnShouldNotTurnOffIt()
        {
            this.homeCondition.AirConditionerOn = true;

            this.homeCondition.TurnAirConditionerOn("klima_be");

            Assert.That(this.homeCondition.AirConditionerOn, Is.EqualTo(true));
        }

        [Test]
        public void AirConditionerOnAirConditionerAreOffShouldTurnOnIt()
        {
            this.homeCondition.AirConditionerOn = false;

            this.homeCondition.TurnAirConditionerOn("klima_be");

            Assert.That(this.homeCondition.AirConditionerOn, Is.EqualTo(true));
        }

        [Test]
        public void AirConditionerOffAirConditionerAreOffShouldNotTurnOnIt()
        {
            this.homeCondition.AirConditionerOn = false;

            this.homeCondition.TurnAirConditionerOff("klima_le");

            Assert.That(this.homeCondition.AirConditionerOn, Is.EqualTo(false));
        }

        [Test]
        public void AirConditionerOffAirConditionerAreOnShouldTurnOffIt()
        {
            this.homeCondition.AirConditionerOn = true;

            this.homeCondition.TurnAirConditionerOff("klima_le");

            Assert.That(this.homeCondition.AirConditionerOn, Is.EqualTo(false));
        }

        [Test]
        public void TempToBasicSitIncreaseTempToBasSitTempShouldSixteen()
        {         
            int initialTemperature = this.homeCondition.Temperature;

            this.homeCondition.Warmer("fazok");            

            Assert.That(this.homeCondition.Temperature, Is.GreaterThan(initialTemperature));

            this.homeCondition.TemperatureToBasicSituation("homerseklet_alap");
            

            Assert.That(this.homeCondition.Temperature, Is.EqualTo(16));
        }

        [Test]
        public void ResBackTempIncreaseBasSitResetBackTempShouldEqeualToLastTemp()
        {
            int initialTemperature = this.homeCondition.Temperature;

            this.homeCondition.Warmer("fazok");

            int changedTemperature = this.homeCondition.Temperature;

            Assert.That(this.homeCondition.Temperature, Is.GreaterThan(initialTemperature));

            this.homeCondition.TemperatureToBasicSituation("homerseklet_alap");
            this.homeCondition.ResetBackTemperature("homerseklet_vissza");

            Assert.That(this.homeCondition.Temperature, Is.EqualTo(changedTemperature));
        }

    }
}
