using System;
using Xunit;
using EventToMetaValueDeconstructor;

namespace EventToMetaValueDeconstructorTests
{
    public class StringValueTypeDeterminatorTests
    {
        [Fact]
        public void GetType_EmptyString_ReturnString()
        {
            //Arrange
            StringValueTypeDeterminator stringValueTypeDeterminator = new StringValueTypeDeterminator();

            //Act
            string result = stringValueTypeDeterminator.Get("");

            //Assert
            Assert.Equal("String", result);
        }

        [Fact]
        public void GetType_String0_ReturnUInt()
        {
            //Arrange
            StringValueTypeDeterminator stringValueTypeDeterminator = new StringValueTypeDeterminator();

            //Act
            string result = stringValueTypeDeterminator.Get("0");

            //Assert
            Assert.Equal("UInt", result);
        }

        [Fact]
        public void GetType_String21313_ReturnUInt()
        {
            //Arrange
            StringValueTypeDeterminator stringValueTypeDeterminator = new StringValueTypeDeterminator();

            //Act
            string result = stringValueTypeDeterminator.Get("21313");

            //Assert
            Assert.Equal("UInt", result);
        }

        [Fact]
        public void GetType_StringWithFirstFormatDate_ReturnDate()
        {
            //Arrange
            StringValueTypeDeterminator stringValueTypeDeterminator = new StringValueTypeDeterminator();

            //Act
            string result = stringValueTypeDeterminator.Get("01.01.2020 12:12:12");

            //Assert
            Assert.Equal("Date", result);
        }

        [Fact]
        public void GetType_StringWithSecondFormatDate_ReturnDate()
        {
            //Arrange
            StringValueTypeDeterminator stringValueTypeDeterminator = new StringValueTypeDeterminator();

            //Act
            string result = stringValueTypeDeterminator.Get("2020-03-10 00:00:34.1627");

            //Assert
            Assert.Equal("Date", result);
        }

        [Fact]
        public void GetType_JustString_ReturnString()
        {
            //Arrange
            StringValueTypeDeterminator stringValueTypeDeterminator = new StringValueTypeDeterminator();

            //Act
            string result = stringValueTypeDeterminator.Get("JustString");

            //Assert
            Assert.Equal("String", result);
        }

        [Fact]
        public void GetType_StringWithDigits_ReturnString()
        {
            //Arrange
            StringValueTypeDeterminator stringValueTypeDeterminator = new StringValueTypeDeterminator();

            //Act
            string result = stringValueTypeDeterminator.Get("JustString212");

            //Assert
            Assert.Equal("String", result);
        }
    }
}
