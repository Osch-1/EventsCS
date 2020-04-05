using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using EventToMetaValueDeconstructor;

namespace EventToMetaValueDeconstructorTests
{
    public class JsonEventParserTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(@"2020 - 03 - 10 00:00:33.1312 | 44 |[Publish] Events.IntegrationEvents.WebPms:WebPmsRoomsInventoryDelivered")]
        public void Deserialize_StringsWithoutKeyJsonAndId_ReturnEmtpyEvent(string InputLine)
        {
            //Arrange
            JsonEventParser JsonEventParser = new JsonEventParser();

            //Act
            Event result = JsonEventParser.Deserialize(InputLine);

            //Assert
            Assert.Equal("NoParam", result.EventKey);
            Assert.Equal("NoParam", result.EventName);
            Assert.Equal(new List<JsonProperty>(), result.EventPropertyMetaValue);
        }

        [Fact]
        public void Deserialize_StringWithAllProperties_ReturnValuableEvent()
        {
            //Arrange
            JsonEventParser JsonEventParser = new JsonEventParser();

            //Act
            Event result = JsonEventParser.Deserialize("2020-03-10 00:00:33.1502|44|[Publish] {6ffacc6a-322b-48ea-aa4a-399c825e026d} key:ET_TravelLine.PriceOptimizer.IntegrationTLTransit.Events.IntegrationEvents.WebPms:WebPmsRoomListDelivered, json:{\"PropertyId\":1111}");

            //Assert
            Assert.Equal("6ffacc6a-322b-48ea-aa4a-399c825e026d", result.EventKey);
            Assert.Equal("ET_TravelLine.PriceOptimizer.IntegrationTLTransit.Events.IntegrationEvents.WebPms:WebPmsRoomListDelivered", result.EventName);
            //Assert.Equal(new List<JsonProperty>() {new JsonProperty("PropertyId", JsonPropertyType.Int, "1111") }, result.EventPropertyMetaValue);
            Assert.Equal("PropertyId", result.EventPropertyMetaValue[0].PropertyName);
            Assert.Equal(JsonPropertyType.Int, result.EventPropertyMetaValue[0].PropertyType);
            Assert.Equal("1111", result.EventPropertyMetaValue[0].DefaultValue);
        }
    }
}