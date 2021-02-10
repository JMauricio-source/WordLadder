using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using WordLadder.Services.Imp;
using WordLadder.Services.Abstract;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WordLadder.Models;

namespace WordLadder.XUnit_Tests
{

    public class Test_Service_WordListRepository
    {

        [Fact]
        public void Can_Load_From_File()
        {
            //Arrange
            var mockIlloger = new Mock<ILogger<WordListRepository>>();
            var mockOptions = new Mock<IOptions<WordLadderOptions>>();
            //mockOptions.g
            mockOptions.Setup(e => e.Value.LocalWordDictionaryFilePath).Returns("");
            var repo = new WordListRepository(mockIlloger.Object, mockOptions.Object);

            //Act

            //Assert

            Assert.True(false);
        }

        [Fact]
        public void Can_Filter_List_With_Predicate()
        {
            //Arrange
            var mockValidator = new Mock<IWordListRepository>();

            //Act

            //Assert

            Assert.True(false);
        }

        [Fact]
        public void Can_Empty_the_Repository()
        {
            //Arrange
            var mockValidator = new Mock<IWordListRepository>();

            //Act

            //Assert

            Assert.True(false);
        }
    }
}
