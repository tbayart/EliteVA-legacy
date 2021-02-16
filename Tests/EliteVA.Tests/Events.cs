using System.Collections.Generic;
using System.Linq;

using EliteAPI.Abstractions;
using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;

using EliteVA.Constants.Formatting;
using EliteVA.Event;
using EliteVA.Services;
using EliteVA.Services.Variable;

using FluentAssertions;

using Moq;

using Xunit;

using IEventProcessor = EliteVA.Event.Abstractions.IEventProcessor;

namespace EliteVA.Tests
{
    public class Events
    {
        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[]
                {
                    BuyAmmoEvent.FromJson("{ \"timestamp\":\"2021-01-07T20:42:27Z\", \"event\":\"BuyAmmo\", \"Cost\":504 }"), 
                    "((EliteAPI.BuyAmmo))", 
                    new[] {"EliteAPI.BuyAmmo.Cost"}
                },
                new object[]
                {
                    CarrierStatsEvent.FromJson("{ \"timestamp\":\"2020-12-19T20:10:59Z\", \"event\":\"CarrierStats\", \"CarrierID\":3704238848, \"Callsign\":\"J5G-61H\", \"Name\":\"ASCENSION\", \"DockingAccess\":\"friends\", \"AllowNotorious\":true, \"FuelLevel\":246, \"JumpRangeCurr\":500.000000, \"JumpRangeMax\":500.000000, \"PendingDecommission\":false, \"SpaceUsage\":{ \"TotalCapacity\":25000, \"Crew\":930, \"Cargo\":544, \"CargoSpaceReserved\":0, \"ShipPacks\":0, \"ModulePacks\":0, \"FreeSpace\":23526 }, \"Finance\":{ \"CarrierBalance\":150229394, \"ReserveBalance\":47556164, \"AvailableBalance\":102673230, \"ReservePercent\":32, \"TaxRate\":100 }, \"Crew\":[ { \"CrewRole\":\"BlackMarket\", \"Activated\":false }, { \"CrewRole\":\"Captain\", \"Activated\":true, \"Enabled\":true, \"CrewName\":\"Fred Bloggs\" }, { \"CrewRole\":\"Refuel\", \"Activated\":true, \"Enabled\":true, \"CrewName\":\"Janely Anthony\" }, { \"CrewRole\":\"Repair\", \"Activated\":true, \"Enabled\":true, \"CrewName\":\"Kenley Clayton\" }, { \"CrewRole\":\"Rearm\", \"Activated\":true, \"Enabled\":true, \"CrewName\":\"Claire Combs\" }, { \"CrewRole\":\"Commodities\", \"Activated\":true, \"Enabled\":true, \"CrewName\":\"Fred Bloggs\" }, { \"CrewRole\":\"VoucherRedemption\", \"Activated\":false }, { \"CrewRole\":\"Exploration\", \"Activated\":false }, { \"CrewRole\":\"Shipyard\", \"Activated\":false }, { \"CrewRole\":\"Outfitting\", \"Activated\":false }, { \"CrewRole\":\"CarrierFuel\", \"Activated\":true, \"Enabled\":true, \"CrewName\":\"Fred Bloggs\" } ], \"ShipPacks\":[  ], \"ModulePacks\":[  ] }"),
                    "((EliteAPI.CarrierStats))", 
                    new[] {"EliteAPI.CarrierStats.SpaceUsage.TotalCapacity", "EliteAPI.CarrierStats.Finance.CarrierBalance"}
                },
                new object[]
                {
                    ReceiveTextEvent.FromJson("{ \"timestamp\":\"2020-12-19T20:17:06Z\", \"event\":\"ReceiveText\", \"From\":\"BHUbaaCKczeCH\", \"Message\":\"summar\", \"Channel\":\"squadron\" }"),
                    "((EliteAPI.ReceiveText))",
                    new[] { "EliteAPI.ReceiveText.Message", "EliteAPI.ReceiveText.MessageLocalised" }
                }
            };

        [Theory(DisplayName = "Command name")]
        [MemberData(nameof(Data))]
        public void Name(IEvent e, string expectedCommand, IEnumerable<string> expectedVariables)
        {
            IEventProcessor events = new EventProcessor(Mock.Of<IEliteDangerousApi>(), new Formatting(), Mock.Of<IVariableService>(), Mock.Of<ICommandService>());

            events.GetCommand(e).Should().Be(expectedCommand);
        }

        [Theory(DisplayName = "Command variables")]
        [MemberData(nameof(Data))]
        public void Variables(IEvent e, string expectedCommand, IEnumerable<string> expectedVariables)
        {
            IEventProcessor events = new EventProcessor(Mock.Of<IEliteDangerousApi>(), new Formatting(), Mock.Of<IVariableService>(), Mock.Of<ICommandService>());

            events.GetVariables(e).Select(x => x.Name).Should().Contain(expectedVariables);
        }
    }
}