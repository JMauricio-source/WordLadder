using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using WordLadder.Services.Imp;
using WordLadder.Models;
using Microsoft.Extensions.Options;

namespace WordLadder.XUnit_Tests
{
    public class Test_Service_ComandLinePayloadLoader
    {
        [Fact]
        public void All_Four_mandatory_parameters_must_given()
        {
            //Arrange
            //public CommandLinePayloadLoader(IOptions<WordLadderOptions> options)
            WordLadderOptions wopt = new WordLadderOptions() { };
            wopt.AllowedWordSize = 4;
            var op = new Mock<IOptions<WordLadderOptions>>();

            op.Setup(e => e.Value).Returns(wopt);

            var sut = new Mock<CommandLinePayloadLoader>(op.Object);
            sut.Setup(e => e.GetCommandLineArgs()).Returns(new string[0]);
            //Act
            string errors = "";
            var isValid = sut.Object.IsValid(out errors);
            //Assert

            Assert.False(isValid);
        }

        [Fact]
        public void Start_Word_Must_Have_AllowedWordSize()
        {
            //Arrange
            //public CommandLinePayloadLoader(IOptions<WordLadderOptions> options)
            WordLadderOptions wopt = new WordLadderOptions() 
            { AllowedWordSize = 4 };
            
            var op = new Mock<IOptions<WordLadderOptions>>();

            op.Setup(e => e.Value).Returns(wopt);

            var sut = new Mock<CommandLinePayloadLoader>(op.Object);
            var payloadArgs = new string[] 
            {
            "-s", 
            "bar",       
            "-f", 
            "chaw", 
            "-d",             
            @"C:\Work\Projectos2021\Puzzle\words-english\words-english.txt", 
            "-o",
            @"C:\Work\Projectos2021\Puzzle\words-english\result2.txt"

            };
            sut.Setup(e => e.GetCommandLineArgs()).Returns(payloadArgs);
            //Act
            string errors = "";
            var isValid = sut.Object.IsValid(out errors);
            //Assert

            Assert.False(isValid);
        }

        [Fact]
        public void End_Word_Must_Have_AllowedWordSize()
        {
            //Arrange
            //public CommandLinePayloadLoader(IOptions<WordLadderOptions> options)
            WordLadderOptions wopt = new WordLadderOptions()
            { AllowedWordSize = 4 };

            var op = new Mock<IOptions<WordLadderOptions>>();

            op.Setup(e => e.Value).Returns(wopt);

            var sut = new Mock<CommandLinePayloadLoader>(op.Object);
            var payloadArgs = new string[]
            {
            "-s",
            "bart",
            "-f",
            "cha",
            "-d",
            @"C:\Work\Projectos2021\Puzzle\words-english\words-english.txt",
            "-o",
            @"C:\Work\Projectos2021\Puzzle\words-english\result2.txt"

            };
            sut.Setup(e => e.GetCommandLineArgs()).Returns(payloadArgs);
            //Act
            string errors = "";
            var isValid = sut.Object.IsValid(out errors);
            //Assert

            Assert.False(isValid);
        }

        [Fact]
        public void Dictionary_Param_must_be_a_valid_path()
        {
            //Arrange
            //public CommandLinePayloadLoader(IOptions<WordLadderOptions> options)
            WordLadderOptions wopt = new WordLadderOptions()
            { AllowedWordSize = 4 };

            var op = new Mock<IOptions<WordLadderOptions>>();

            op.Setup(e => e.Value).Returns(wopt);

            var sut = new Mock<CommandLinePayloadLoader>(op.Object);
            var payloadArgs = new string[]
            {
            "-s",
            "bart",
            "-f",
            "chaw",
            "-d",
            @"C:\words-english.txt",
            "-o",
            @"C:\Work\Projectos2021\Puzzle\words-english\result2.txt"

            };
            sut.Setup(e => e.GetCommandLineArgs()).Returns(payloadArgs);
            //Act
            string errors = "";
            var isValid = sut.Object.IsValid(out errors);
            //Assert

            Assert.False(isValid);
        }

    }
}
