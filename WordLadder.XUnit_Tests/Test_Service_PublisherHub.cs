using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WordLadder.Models;
using WordLadder.Services.Abstract;
using WordLadder.Services.Imp;
using Xunit;

namespace WordLadder.XUnit_Tests
{
    public class Test_Service_PublisherHub
    {

        [Fact]
        public void Call_PublishMessageToAll_Call_publish_In_All_Publishers()
        {
            //Arrange
            var mockIlloger = new Mock<ILogger<IPublisherHub>>();
            var mockpublisher1 = new Mock<IPublisher>();
            var mockpublisher2 = new Mock<IPublisher>();
            var mockpublisher3 = new Mock<IPublisher>();

            var publisherList = new List<IPublisher>() { 
                    mockpublisher1.Object, 
                    mockpublisher2.Object, 
                    mockpublisher3.Object 
            };

            var repo = new PublisherHub(mockIlloger.Object, publisherList);
            //Act
            var msg = "Test Message";
            JobPayloadCommand job = new JobPayloadCommand();
            repo.PublishMessageToAll(msg, job);

            //Assert
            mockpublisher1.Verify(p => p.Publish(msg, job), Times.Once());
            mockpublisher2.Verify(p => p.Publish(msg, job), Times.Once());
            mockpublisher3.Verify(p => p.Publish(msg, job), Times.Once());
        }

        [Fact]
        public void Call_PublishResultToAll_Call_publish_In_All_Publishers()
        {
            //Arrange
            var mockIlloger = new Mock<ILogger<IPublisherHub>>();
            var mockpublisher1 = new Mock<IPublisher>();
            var mockpublisher2 = new Mock<IPublisher>();
            var mockpublisher3 = new Mock<IPublisher>();

            var publisherList = new List<IPublisher>() {
                    mockpublisher1.Object,
                    mockpublisher2.Object,
                    mockpublisher3.Object
            };

            var repo = new PublisherHub(mockIlloger.Object, publisherList);
            //Act
            
            ProcessingResult _result = new ProcessingResult();
            repo.PublishResultToAll(_result);

            //Assert
            mockpublisher1.Verify(p => p.Publish(_result), Times.Once());
            mockpublisher2.Verify(p => p.Publish(_result), Times.Once());
            mockpublisher3.Verify(p => p.Publish(_result), Times.Once());
        }
    }
}
