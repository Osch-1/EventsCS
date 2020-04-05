using Xunit;
using EventToMetaValueDeconstructor;
using Newtonsoft.Json.Linq;

namespace EventToMetaValueDeconstructorTests
{
    public class JpropertyTypeDeterminatorTests
    {
        [Fact]
        public void GetType_EmptyString_ReturnString()
        {
            //Arrange
            JpropertyTypeDeterminator stringValueTypeDeterminator = new JpropertyTypeDeterminator();

            //Act
            JsonPropertyType result = stringValueTypeDeterminator.Get(new JProperty("SomeProperty", ""));

            //Assert
            Assert.Equal(JsonPropertyType.String, result);
        }

        [Fact]
        public void GetType_String0_ReturnInt()
        {
            //Arrange
            JpropertyTypeDeterminator stringValueTypeDeterminator = new JpropertyTypeDeterminator();

            //Act
            JsonPropertyType result = stringValueTypeDeterminator.Get(new JProperty("SomeProperty", "0"));

            //Assert
            Assert.Equal(JsonPropertyType.Int, result);
        }

        [Fact]
        public void GetType_String21313_ReturnInt()
        {
            //Arrange
            JpropertyTypeDeterminator stringValueTypeDeterminator = new JpropertyTypeDeterminator();

            //Act
            JsonPropertyType result = stringValueTypeDeterminator.Get(new JProperty("SomeProperty", "21313"));

            //Assert
            Assert.Equal(JsonPropertyType.Int, result);
        }

        [Fact]
        public void GetType_StringWithFirstFormatDate_ReturnDate()
        {
            //Arrange
            JpropertyTypeDeterminator stringValueTypeDeterminator = new JpropertyTypeDeterminator();

            //Act
            JsonPropertyType result = stringValueTypeDeterminator.Get(new JProperty("SomeProperty", "01.01.2020 12:12:12"));

            //Assert
            Assert.Equal(JsonPropertyType.DateTime, result);
        }

        [Fact]
        public void GetType_StringWithSecondFormatDate_ReturnDate()
        {
            //Arrange
            JpropertyTypeDeterminator stringValueTypeDeterminator = new JpropertyTypeDeterminator();

            //Act
            JsonPropertyType result = stringValueTypeDeterminator.Get(new JProperty("SomeProperty", "2020-03-10 00:00:34.1627"));

            //Assert
            Assert.Equal(JsonPropertyType.DateTime, result);
        }

        [Fact]
        public void GetType_JustString_ReturnString()
        {
            //Arrange
            JpropertyTypeDeterminator stringValueTypeDeterminator = new JpropertyTypeDeterminator();

            //Act
            JsonPropertyType result = stringValueTypeDeterminator.Get(new JProperty("SomeProperty", "JustString"));

            //Assert
            Assert.Equal(JsonPropertyType.String, result);
        }

        [Fact]
        public void GetType_StringWithDigits_ReturnString()
        {
            //Arrange
            JpropertyTypeDeterminator stringValueTypeDeterminator = new JpropertyTypeDeterminator();

            //Act
            JsonPropertyType result = stringValueTypeDeterminator.Get(new JProperty("SomeProperty", "JustString212"));

            //Assert
            Assert.Equal(JsonPropertyType.String, result);
        }

        [Fact]
        public void GetType_Array_ReturnList()
        {
            //Arrange
            JpropertyTypeDeterminator stringValueTypeDeterminator = new JpropertyTypeDeterminator();

            //Act
            JsonPropertyType result = stringValueTypeDeterminator.Get(new JProperty("SomeProperty", new JArray()));

            //Assert
            Assert.Equal(JsonPropertyType.List, result);
        }
    }
}
