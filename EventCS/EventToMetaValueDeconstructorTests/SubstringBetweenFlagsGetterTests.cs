using Xunit;
using EventToMetaValueDeconstructor;

namespace EventToMetaValueDeconstructorTests
{
    public class SubstringBetweenFlagsGetterTests
    {
        [Fact]
        public void Get_SearchBetweenEmptyStringInEmptyStringWithEmptyEndingFlag_ReturnNoParam()
        {
            //Arrange
            SubstringBetweenFlagsGetter substringBetweenFlagsGetter = new SubstringBetweenFlagsGetter();

            //Act
            string result = substringBetweenFlagsGetter.Get("", "", "");

            //Assert
            Assert.Equal("NoParam", result);
        }

        [Fact]
        public void Get_BetweenEmptyStringInEmptyStringWithSomeEndingFlag_ReturnNoParam()
        {
            //Arrange
            SubstringBetweenFlagsGetter substringBetweenFlagsGetter = new SubstringBetweenFlagsGetter();

            //Act
            string result = substringBetweenFlagsGetter.Get("", "", "SomeEndingFlag");

            //Assert
            Assert.Equal("NoParam", result);
        }

        [Fact]
        public void Get_BetweenSomeStringInEmptyStringWithEmptyEndingFlag_ReturnNoParam()
        {
            //Arrange
            SubstringBetweenFlagsGetter substringBetweenFlagsGetter = new SubstringBetweenFlagsGetter();

            //Act
            string result = substringBetweenFlagsGetter.Get("", "SomeString", "");

            //Assert
            Assert.Equal("NoParam", result);
        }

        [Fact]
        public void Get_BetweenEmptyStringInSomeStringWithEmptyEndingFlag_ReturnNoParam()
        {
            //Arrange
            SubstringBetweenFlagsGetter substringBetweenFlagsGetter = new SubstringBetweenFlagsGetter();

            //Act
            string result = substringBetweenFlagsGetter.Get("SomeString", "", "");

            //Assert
            Assert.Equal("NoParam", result);
        }

        [Fact]

        public void Get_SearchBetweenSomeKeyInEmptyStringWithSomeFlagAsEndingFlag_ReturnNoParam()
        {
            //Arrange
            SubstringBetweenFlagsGetter substringBetweenFlagsGetter = new SubstringBetweenFlagsGetter();

            //Act
            string result = substringBetweenFlagsGetter.Get("", "someKey", "someFlag");

            //Assert
            Assert.Equal("NoParam", result);
        }

        [Fact]
        public void Get_SearchBetweenSomeStringInSomeStringWithParamsWithEmptyEndingFlag_ReturnNoParam()
        {
            //Arrange
            SubstringBetweenFlagsGetter substringBetweenFlagsGetter = new SubstringBetweenFlagsGetter();

            //Act
            string result = substringBetweenFlagsGetter.Get("SomeStringWithParams", "SomeString", "");

            //Assert
            Assert.Equal("NoParam", result);
        }

        [Fact]
        public void Get_SearchBetweenEmptyStringInSomeStringWithParamsWithParamsEndingFlag_ReturnNoParam()
        {
            //Arrange
            SubstringBetweenFlagsGetter substringBetweenFlagsGetter = new SubstringBetweenFlagsGetter();

            //Act
            string result = substringBetweenFlagsGetter.Get("SomeStringWithParams", "", "Params");

            //Assert
            Assert.Equal("NoParam", result);
        }

        [Fact]
        public void Get_SearchBetweenKeyInStringWithKeyWithEndingFlag_ReturnKeyValue()
        {
            //Arrange
            SubstringBetweenFlagsGetter substringBetweenFlagsGetter = new SubstringBetweenFlagsGetter();

            //Act
            string result = substringBetweenFlagsGetter.Get("Text contains key:_IamGreatestKey, good luck", "key:", ",");

            //Assert
            Assert.Equal("_IamGreatestKey", result);
        }
    }
}