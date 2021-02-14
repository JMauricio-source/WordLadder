using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WordLadder.Models;
using System.IO;
using WordLadder.Services.Abstract;
using WordLadder.Services.Imp;
using System.Linq;
using System.Linq.Expressions;

namespace WordLadder.XUnit_Tests
{
    public class Test_Service_WordLadderProcessor
    {
        [Fact]
        public void WordLadderProcessor_Finds_Word_Sequence_When_Exists()
        {
            //Arrange
            var mockIlloger = new Mock<ILogger<IWordLadderProcessor>>();
            var mockOptions = new Mock<IOptions<WordLadderOptions>>();
            var mockRepository= new Mock<IWordListRepository>();

            var _path = Path.GetFullPath("./Data/words-english.txt");
            var _dict = File.ReadAllLines(_path).Where(e=> e.Length==4).Select(e=>e).ToList();
            mockRepository.Setup(e => e.GetFiltered(It.IsAny<Func<string, bool>>())).Returns(_dict);

            WordLadderOptions wOp = new WordLadderOptions()
            {
                TypeOfSearch = JobPayloadCommand.SearchType.BREATH_FIRST,
                ResultsDefaultPath = _path
            };

            mockOptions.Setup(e => e.Value).Returns(wOp);
            JobPayloadCommand payload = new JobPayloadCommand()
            {
                StartWord = "bard",
                EndWord = "chaw",
                TypeOfSearch = JobPayloadCommand.SearchType.BREATH_FIRST
            };

            WordLadderProcessor _result = new WordLadderProcessor(mockIlloger.Object, mockRepository.Object, mockOptions.Object);

            ////Act
            var _processResult = _result.ProcessAsync(payload).Result;

            //Assert
            Assert.Equal(24, _processResult.Result.Count);
        }
    }
}
