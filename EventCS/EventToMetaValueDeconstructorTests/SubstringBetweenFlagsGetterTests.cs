using Xunit;
using EventToMetaValueDeconstructor;

namespace EventToMetaValueDeconstructorTests
{
    public class SubstringBetweenFlagsGetterTests
    {
        [ Theory ]
        [ InlineData( "", "", "" ) ]
        [ InlineData( "", "", "SomeEndingFlag" ) ]
        [ InlineData( "", "SomeString", "" ) ]
        [ InlineData( "SomeString", "", "" ) ]
        [ InlineData( "", "someKey", "someFlag" ) ]
        [ InlineData( "SomeStringWithParams", "SomeString", "" ) ]
        [ InlineData( "SomeStringWithParams", "", "Params" ) ]
        public void Get_SomeParamIsEmptyOrNoSuchProperty_ReturnNoParam( string inputLine, string startFlag, string endFlag )
        {
            //Arrange
            SubstringBetweenFlagsGetter substringBetweenFlagsGetter = new SubstringBetweenFlagsGetter();

            //Act
            string Result = substringBetweenFlagsGetter.Get( inputLine, startFlag, endFlag );

            //Assert
            Assert.Equal( "", Result );
        }

        [ Fact ]
        public void Get_SearchBetweenKeyInStringWithKeyWithEndingFlag_ReturnKeyValue()
        {
            //Arrange
            SubstringBetweenFlagsGetter substringBetweenFlagsGetter = new SubstringBetweenFlagsGetter();

            //Act
            string Result = substringBetweenFlagsGetter.Get( "Text contains key:_IamGreatestKey, good luck", "key:", "," );

            //Assert
            Assert.Equal( "_IamGreatestKey", Result );
        }
    }
}