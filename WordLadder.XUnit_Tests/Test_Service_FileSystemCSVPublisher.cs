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

namespace WordLadder.XUnit_Tests
{
    public class Test_Service_FileSystemCSVPublisher
    {

        [Fact]
        public void Can_write_To_File_with_Path_from_CommandArgs()
        {
            /* */
            //Arrange
            var mockIlloger = new Mock<ILogger<IPublisher>>();
            var mockOptions = new Mock<IOptions<WordLadderOptions>>();


            var _newFilePath = Path.Combine("./Data", Guid.NewGuid() + ".csv");
            var _path = Path.GetFullPath(_newFilePath);
            WordLadderOptions wOp = new WordLadderOptions()
            {
                TypeOfSearch = JobPayloadCommand.SearchType.BREATH_FIRST,
                ResultsDefaultPath = null
            };

            mockOptions.Setup(e => e.Value).Returns(wOp);

            var _publisher = new FileSystemCSVPublisher(mockIlloger.Object, mockOptions.Object);
            JobPayloadCommand payload = new JobPayloadCommand();
            payload.ResultPublicationPath = _path;
            //Act
            _publisher.Publish("This is a test", payload);
            //Assert

            var _exist = File.Exists(_path);

            File.Delete(_path);
            Assert.True(_exist);

            
        }

        [Fact]
        public void Can_write_To_File_with_Path_from_Configuration()
        {

            //Arrange
            var mockIlloger = new Mock<ILogger<IPublisher>>();
            var mockOptions = new Mock<IOptions<WordLadderOptions>>();


            var _newFilePath = Path.Combine("./Data", Guid.NewGuid() + ".csv");
            var _path = Path.GetFullPath(_newFilePath);
            WordLadderOptions wOp = new WordLadderOptions()
            {
                TypeOfSearch = JobPayloadCommand.SearchType.BREATH_FIRST,
                ResultsDefaultPath = _path
            };

            mockOptions.Setup(e => e.Value).Returns(wOp);

            var _publisher = new FileSystemCSVPublisher(mockIlloger.Object, mockOptions.Object);
            JobPayloadCommand payload = new JobPayloadCommand();
            //Act
            _publisher.Publish("This is a test", payload);
            //Assert
            var _exist = File.Exists(_path);

            File.Delete(_path);
            Assert.True(_exist);
        }

        [Fact]
        public void Can_write_To_csv_file_from_ProcessingResult()
        {
            //Arrange
            var mockIlloger = new Mock<ILogger<IPublisher>>();
            var mockOptions = new Mock<IOptions<WordLadderOptions>>();


            var _newFilePath = Path.Combine("./Data", Guid.NewGuid() + ".csv");
            var _path = Path.GetFullPath(_newFilePath);
            WordLadderOptions wOp = new WordLadderOptions()
            {
                TypeOfSearch = JobPayloadCommand.SearchType.BREATH_FIRST,
                ResultsDefaultPath = _path
            };

            mockOptions.Setup(e => e.Value).Returns(wOp);

            var _publisher = new FileSystemCSVPublisher(mockIlloger.Object, mockOptions.Object);
            JobPayloadCommand payload = new JobPayloadCommand();
            ProcessingResult _result = new ProcessingResult();
            _result.Payload = payload;
            _result.WasSuccefull = true;

            _result.Result.Add("Test");
            _result.Result.Add("Pest");
            _result.Result.Add("Post");
            //Act

            _publisher.Publish(_result);
            //Assert
            var fromFile = File.ReadAllText(_path);

            File.Delete(_path);
            Assert.Equal(string.Join(";", _result.Result), fromFile);

        }

        [Fact]
        public void Can_write_To_csv_file_from_string()
        {
            //Arrange
            var mockIlloger = new Mock<ILogger<IPublisher>>();
            var mockOptions = new Mock<IOptions<WordLadderOptions>>();


            var _newFilePath = Path.Combine("./Data", Guid.NewGuid() + ".csv");
            var _path = Path.GetFullPath(_newFilePath);
            WordLadderOptions wOp = new WordLadderOptions()
            {
                TypeOfSearch = JobPayloadCommand.SearchType.BREATH_FIRST,
                ResultsDefaultPath = _path
            };

            mockOptions.Setup(e => e.Value).Returns(wOp);

            var _publisher = new FileSystemTextPublisher(mockIlloger.Object, mockOptions.Object);
            JobPayloadCommand payload = new JobPayloadCommand();
            //Act
            var _txt = "This is a test.";
            _publisher.Publish(_txt, payload);
            //Assert
            var fromFile = File.ReadAllText(_path);

            File.Delete(_path);
            Assert.Equal(fromFile, _txt);

            
        }
    }
}
