using Xunit;
using EventToMetaValueDeconstructor;
using Newtonsoft.Json.Linq;

namespace EventToMetaValueDeconstructorTests
{
    public class JpropertyTypeDeterminatorTests
    {
        [ Theory ]
        [ InlineData( "" ) ]
        [ InlineData( "This is some input string with 123 [] 213" ) ]
        [ InlineData( "123123This is some input string with 123 [] 213" ) ]
        public void GetType_EmptyString_ReturnString( string inputLine )
        {
            //Arrange
            JpropertyTypeDeterminator stringValueTypeDeterminator = new JpropertyTypeDeterminator();

            //Act
            PropertyType Result = stringValueTypeDeterminator.Get( inputLine );

            //Assert
            Assert.Equal( PropertyType.String, Result );
        }
        
        [ Theory ]
        [ InlineData( "-123214" ) ]
        [ InlineData( "123214" ) ]
        [ InlineData( "12.1324124" ) ]
        [ InlineData( "1" ) ]
        [ InlineData( "-1" ) ]        
        public void GetType_SomeNumber_ReturnNumber( string inputLine )
        {
            //Arrange
            JpropertyTypeDeterminator stringValueTypeDeterminator = new JpropertyTypeDeterminator();

            //Act
            PropertyType Result = stringValueTypeDeterminator.Get( inputLine );

            //Assert
            Assert.Equal( PropertyType.Number, Result );
        }
        
        [ Theory ]
        [ InlineData( "01.01.2020 12:12:12" ) ]        
        [ InlineData( "2020-03-10 00:00:34.1627" ) ]
        public void GetType_AnyOfTwoDateTypes_ReturnDate( string inputLine )
        {
            //Arrange
            JpropertyTypeDeterminator stringValueTypeDeterminator = new JpropertyTypeDeterminator();

            //Act
            PropertyType Result = stringValueTypeDeterminator.Get( inputLine );

            //Assert
            Assert.Equal( PropertyType.DateTime, Result );
        }           

        [ Fact ]
        public void GetType_Array_ReturnList()
        {
            //Arrange
            JpropertyTypeDeterminator stringValueTypeDeterminator = new JpropertyTypeDeterminator();

            //Act
            PropertyType Result = stringValueTypeDeterminator.Get( "[{\"RoomTypeId\": 309271, \"Physical\": null, \"DateInventories\": [{\"Date\": \"2021-03-11T00:00:00\", \"OutOfOrder\": 0, \"Sold\": 0}]}]" );

            //Assert
            Assert.Equal( PropertyType.List, Result );
        }

        [ Theory ]
        //empty object
        [ InlineData( "{}" ) ]
        //simple object
        [ InlineData( "{\"Name: \"\"Jhon\",\n\"age: \"\"32\"\n}" ) ]
        //object with object property
        [ InlineData( "{\"Name: \"\"Jhon\",\n\"age: \"\"32\"\n,\n\"son: \"{\"Name: \"\"Jhon\",\n\"age: \"\"12\"\n}\n}" ) ]
        public void GetType_StructureBetweenBraces_ReturnObject( string inputLine )
        {
            //Arrange
            JpropertyTypeDeterminator stringValueTypeDeterminator = new JpropertyTypeDeterminator();

            //Act
            PropertyType Result = stringValueTypeDeterminator.Get( inputLine );

            //Assert
            Assert.Equal( PropertyType.Object, Result );
        }
    }
}
