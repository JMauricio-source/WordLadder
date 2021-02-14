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
using WordLadder.Models.Exceptions;
using System.IO;

namespace WordLadder.XUnit_Tests
{

    public class Test_Service_WordListRepository
    {
        
        [Fact]
        public void Can_Load_From_File_with_Path_from_CommandArgs()
        {
            //Arrange
            var mockIlloger = new Mock<ILogger<WordListRepository>>();
            var mockOptions = new Mock<IOptions<WordLadderOptions>>();

            var _path = Path.GetFullPath("./Data/words-english.txt");
            WordLadderOptions wOp = new WordLadderOptions()
            {
                TypeOfSearch = JobPayloadCommand.SearchType.BREATH_FIRST,
                LocalWordDictionaryFilePath = ""
            };

            mockOptions.Setup(e => e.Value).Returns(wOp);

            var repo = new Mock<WordListRepository>(mockIlloger.Object, mockOptions.Object);
            var _args = new string[] { "-d", _path };
            repo.Setup(e => e.GetCommandLineArgs()).Returns(_args);
            //Act
            var _list = repo.Object.All();
            //Assert

            Assert.NotEmpty(_list);
        }
       
        
        [Fact]
        public void Can_Load_From_File_with_Path_from_Configuration()
        {
            //Arrange
            var mockIlloger = new Mock<ILogger<WordListRepository>>();
            var mockOptions = new Mock<IOptions<WordLadderOptions>>();

            var _path = Path.GetFullPath("./Data/words-english.txt");
            WordLadderOptions wOp = new WordLadderOptions() { 
                TypeOfSearch = JobPayloadCommand.SearchType.BREATH_FIRST,
                LocalWordDictionaryFilePath = _path
            };

            mockOptions.Setup(e => e.Value).Returns(wOp);

            var repo = new WordListRepository(mockIlloger.Object, mockOptions.Object);

            //Act
            var _list = repo.All();
            //Assert

            Assert.NotEmpty(_list);
        }

        
        [Fact]
        public void Can_Filter_List_With_Predicate()
        {
            //Arrange
            var mockIlloger = new Mock<ILogger<WordListRepository>>();
            var mockOptions = new Mock<IOptions<WordLadderOptions>>();

            WordLadderOptions wOp = new WordLadderOptions() { TypeOfSearch = JobPayloadCommand.SearchType.BREATH_FIRST };


            mockOptions.Setup(e => e.Value).Returns(wOp);

            var repo = new WordListRepository(mockIlloger.Object, mockOptions.Object);
            repo.WordList.Add("Test");
            repo.WordList.Add("Testtwo");
            repo.WordList.Add("Testabc");
            //Act
            var _list = repo.GetFiltered((_word) => _word.Length == 4);
            //Assert

            Assert.Single(_list);
        }
        

        [Fact]
        public void Can_Empty_the_Repository()
        {
            //Arrange
            var mockIlloger = new Mock<ILogger<WordListRepository>>();
            var mockOptions = new Mock<IOptions<WordLadderOptions>>();

            WordLadderOptions wOp = new WordLadderOptions() { TypeOfSearch = JobPayloadCommand.SearchType.BREATH_FIRST };


            mockOptions.Setup(e => e.Value).Returns(wOp);

            var repo = new WordListRepository(mockIlloger.Object, mockOptions.Object);
            repo.WordList.Add("Test");
            //Act
            repo.Clear();
            //Assert

            Assert.Empty(repo.WordList);
        }
        

        [Fact]
        public void Throws_SourceNotFoundException_whenNoPath()
        {
            //Arrange
            var mockIlloger = new Mock<ILogger<WordListRepository>>();
            var mockOptions = new Mock<IOptions<WordLadderOptions>>();

            WordLadderOptions wOp = new WordLadderOptions() { TypeOfSearch = JobPayloadCommand.SearchType.BREATH_FIRST };


            mockOptions.Setup(e => e.Value).Returns(wOp);

            var repo =  new WordListRepository(mockIlloger.Object, mockOptions.Object);

            //Assert
            Assert.Throws<SourceNotFoundException>(()=>repo.All());
        }
    }
}
