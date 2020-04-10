using EventToMetaValueDeconstructor;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EventToMetaValueDeconstructorTests
{
    public class JsonEventParserTests
    {
        [Fact]
        public void Parse_EmptyString_ReturnEmptyEvent()
        {
            //Arrange
            JsonEventParser jsonEventParser = new JsonEventParser();

            //Act
            Event Result = jsonEventParser.Parse("SomeEventName", "");

            //Assert
            Assert.Equal("", Result.EventKey);
            Assert.Equal("", Result.CreationDate);
            Assert.Equal(new List<JsonProperty>(), Result.JsonPropertiesMetaValue);
        }

        [Fact]
        public void Parse_JsonWithObjectProperty_ReturnEventWithObjectProperty()
        {
            //Arrange
            JsonEventParser jsonEventParser = new JsonEventParser();

            //Act
            Event Result = jsonEventParser.Parse("SomeEventName", "{Name: \"Jhon\",\nage: \"32\"\n,\nson: {Name: \"Jhon\",\nage: \"12\"\n}\n}");

            //Assert
            Assert.Equal("SomeEventName", Result.EventKey);
            Assert.Equal("", Result.CreationDate);
            Assert.Equal("Property name: Name\n  Property type: String\n  Sample value: \n  Jhon", Result.JsonPropertiesMetaValue[0].ToString());
            Assert.Equal("Property name: age\n  Property type: Number\n  Sample value: \n  32", Result.JsonPropertiesMetaValue[1].ToString());
            Assert.Equal("Property name: son\n  Property type: Object\n  Sample value: \n  {\r\n  \"Name\": \"Jhon\",\r\n  \"age\": \"12\"\r\n}", Result.JsonPropertiesMetaValue[2].ToString());
        }

        [Fact]
        public void Parse_JsonWithListProperty_ReturnEmptyEvent()
        {
            //Arrange
            JsonEventParser jsonEventParser = new JsonEventParser();

            //Act
            Event Result = jsonEventParser.Parse("SomeEventName", "{courses: [\"html\", \"css\"]\n}");

            //Assert
            Assert.Equal("SomeEventName", Result.EventKey);
            Assert.Equal("", Result.CreationDate);            
            Assert.Equal("Property name: courses\n  Property type: List\n  Sample value: \n  [\r\n  \"html\",\r\n  \"css\"\r\n]", Result.JsonPropertiesMetaValue[0].ToString());
        }
    }
}
