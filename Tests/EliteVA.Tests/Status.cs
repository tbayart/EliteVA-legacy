using EliteAPI.Abstractions;
using EliteVA.Constants.Formatting;
using EliteVA.Services;
using EliteVA.Status;
using EliteVA.Status.Abstractions;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EliteVA.Tests
{
    class Pips
    {
        public Pips(int engines, int system, int weapons)
        {
            Engines = engines;
            System = system;
            Weapons = weapons;
        }

        public int Engines { get; }
        public int System  { get; }
        public int Weapons { get; }
    }
    
    public class Status
    {
        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[]
                {
                    "Gear", true, 
                    "((EliteAPI.Status.Gear))", 
                    new[] {"EliteAPI.Gear"}
                },
                new object[]
                {
                    "Pips", new Pips(1,2,3), 
                    "((EliteAPI.Status.Pips))", 
                    new[] {"EliteAPI.Pips.Engines", "EliteAPI.Pips.System", "EliteAPI.Pips.Weapons"}
                }
            };

        [Theory(DisplayName = "StatusProcessor")]
        [MemberData(nameof(Data))]
        public void Variable(string statusName, object statusValue, string expectedCommand, IEnumerable<string> expectedVariables)
        {
            IStatusProcessor status = new StatusProcessor(Mock.Of<IEliteDangerousApi>(), new Formatting(), Mock.Of<IVariableService>(), Mock.Of<ICommandService>());
            var variables = status.GetVariables(statusName, statusValue);
            variables.Should().OnlyContain(o => o.SourceEvent == string.Empty);
            variables.Select(x => x.Name).Should().Contain(expectedVariables);
            status.GetCommand(statusName).Should().Be(expectedCommand);
        }
    }
}