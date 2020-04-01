using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using EventToMetaValueDeconstructor;

namespace EventToMetaValueDeconstructorTests
{
    public class StringValueTypeDeterminatorTests
    {
        [Fact]
        public void GetType_StringWith0_UIntReturned()
        {
            //Arrange
            StringValueTypeDeterminator stringValueTypeDeterminator = new StringValueTypeDeterminator();

            //Act
            string result = stringValueTypeDeterminator.Get("0");

            //Assert
            Assert.Equal("UInt", result);
        }
    }
}
