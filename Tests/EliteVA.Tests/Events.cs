using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using EliteAPI.Abstractions;
using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;
using EliteVA.Event;
using EliteVA.Services;
using EliteVA.Services.Variable;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Formatting = EliteVA.Constants.Formatting.Formatting;
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
                },
                new object[]
                {
                    MaterialsEvent.FromJson("{ \"timestamp\":\"2021-05-16T18:24:34Z\", \"event\":\"Materials\", \"Raw\":[ { \"Name\":\"carbon\", \"Count\":78 }, { \"Name\":\"chromium\", \"Count\":29 }, { \"Name\":\"iron\", \"Count\":16 }, { \"Name\":\"phosphorus\", \"Count\":45 }, { \"Name\":\"sulphur\", \"Count\":58 }, { \"Name\":\"manganese\", \"Count\":27 }, { \"Name\":\"cadmium\", \"Count\":2 }, { \"Name\":\"niobium\", \"Count\":3 }, { \"Name\":\"yttrium\", \"Count\":3 }, { \"Name\":\"zinc\", \"Count\":8 }, { \"Name\":\"arsenic\", \"Count\":1 }, { \"Name\":\"lead\", \"Count\":12 }, { \"Name\":\"rhenium\", \"Count\":6 }, { \"Name\":\"vanadium\", \"Count\":4 } ], \"Manufactured\":[ { \"Name\":\"mechanicalequipment\", \"Name_Localised\":\"Mechanical Equipment\", \"Count\":29 }, { \"Name\":\"conductivecomponents\", \"Name_Localised\":\"Conductive Components\", \"Count\":18 }, { \"Name\":\"salvagedalloys\", \"Name_Localised\":\"Salvaged Alloys\", \"Count\":3 }, { \"Name\":\"refinedfocuscrystals\", \"Name_Localised\":\"Refined Focus Crystals\", \"Count\":30 }, { \"Name\":\"phasealloys\", \"Name_Localised\":\"Phase Alloys\", \"Count\":3 }, { \"Name\":\"shieldingsensors\", \"Name_Localised\":\"Shielding Sensors\", \"Count\":7 }, { \"Name\":\"compoundshielding\", \"Name_Localised\":\"Compound Shielding\", \"Count\":6 }, { \"Name\":\"focuscrystals\", \"Name_Localised\":\"Focus Crystals\", \"Count\":34 }, { \"Name\":\"heatconductionwiring\", \"Name_Localised\":\"Heat Conduction Wiring\", \"Count\":1 }, { \"Name\":\"highdensitycomposites\", \"Name_Localised\":\"High Density Composites\", \"Count\":4 }, { \"Name\":\"galvanisingalloys\", \"Name_Localised\":\"Galvanising Alloys\", \"Count\":12 }, { \"Name\":\"shieldemitters\", \"Name_Localised\":\"Shield Emitters\", \"Count\":14 }, { \"Name\":\"heatresistantceramics\", \"Name_Localised\":\"Heat Resistant Ceramics\", \"Count\":2 }, { \"Name\":\"wornshieldemitters\", \"Name_Localised\":\"Worn Shield Emitters\", \"Count\":24 }, { \"Name\":\"hybridcapacitors\", \"Name_Localised\":\"Hybrid Capacitors\", \"Count\":28 }, { \"Name\":\"mechanicalcomponents\", \"Name_Localised\":\"Mechanical Components\", \"Count\":22 }, { \"Name\":\"heatvanes\", \"Name_Localised\":\"Heat Vanes\", \"Count\":9 }, { \"Name\":\"conductiveceramics\", \"Name_Localised\":\"Conductive Ceramics\", \"Count\":15 }, { \"Name\":\"conductivepolymers\", \"Name_Localised\":\"Conductive Polymers\", \"Count\":22 }, { \"Name\":\"fedcorecomposites\", \"Name_Localised\":\"Core Dynamics Composites\", \"Count\":3 }, { \"Name\":\"fedproprietarycomposites\", \"Name_Localised\":\"Proprietary Composites\", \"Count\":12 }, { \"Name\":\"crystalshards\", \"Name_Localised\":\"Crystal Shards\", \"Count\":2 }, { \"Name\":\"uncutfocuscrystals\", \"Name_Localised\":\"Flawed Focus Crystals\", \"Count\":59 }, { \"Name\":\"gridresistors\", \"Name_Localised\":\"Grid Resistors\", \"Count\":64 }, { \"Name\":\"biotechconductors\", \"Name_Localised\":\"Biotech Conductors\", \"Count\":23 }, { \"Name\":\"thermicalloys\", \"Name_Localised\":\"Thermic Alloys\", \"Count\":12 }, { \"Name\":\"precipitatedalloys\", \"Name_Localised\":\"Precipitated Alloys\", \"Count\":23 }, { \"Name\":\"exquisitefocuscrystals\", \"Name_Localised\":\"Exquisite Focus Crystals\", \"Count\":53 }, { \"Name\":\"mechanicalscrap\", \"Name_Localised\":\"Mechanical Scrap\", \"Count\":37 }, { \"Name\":\"electrochemicalarrays\", \"Name_Localised\":\"Electrochemical Arrays\", \"Count\":13 }, { \"Name\":\"filamentcomposites\", \"Name_Localised\":\"Filament Composites\", \"Count\":3 }, { \"Name\":\"chemicaldistillery\", \"Name_Localised\":\"Chemical Distillery\", \"Count\":2 }, { \"Name\":\"temperedalloys\", \"Name_Localised\":\"Tempered Alloys\", \"Count\":3 } ], \"Encoded\":[ { \"Name\":\"shieldpatternanalysis\", \"Name_Localised\":\"Aberrant Shield Pattern Analysis\", \"Count\":29 }, { \"Name\":\"shieldcyclerecordings\", \"Name_Localised\":\"Distorted Shield Cycle Recordings\", \"Count\":67 }, { \"Name\":\"scandatabanks\", \"Name_Localised\":\"Classified Scan Databanks\", \"Count\":27 }, { \"Name\":\"shielddensityreports\", \"Name_Localised\":\"Untypical Shield Scans \", \"Count\":106 }, { \"Name\":\"decodedemissiondata\", \"Name_Localised\":\"Decoded Emission Data\", \"Count\":29 }, { \"Name\":\"scanarchives\", \"Name_Localised\":\"Unidentified Scan Archives\", \"Count\":42 }, { \"Name\":\"encodedscandata\", \"Name_Localised\":\"Divergent Scan Data\", \"Count\":12 }, { \"Name\":\"archivedemissiondata\", \"Name_Localised\":\"Irregular Emission Data\", \"Count\":23 }, { \"Name\":\"shieldfrequencydata\", \"Name_Localised\":\"Peculiar Shield Frequency Data\", \"Count\":12 }, { \"Name\":\"scrambledemissiondata\", \"Name_Localised\":\"Exceptional Scrambled Emission Data\", \"Count\":13 }, { \"Name\":\"embeddedfirmware\", \"Name_Localised\":\"Modified Embedded Firmware\", \"Count\":11 }, { \"Name\":\"shieldsoakanalysis\", \"Name_Localised\":\"Inconsistent Shield Soak Analysis\", \"Count\":53 }, { \"Name\":\"emissiondata\", \"Name_Localised\":\"Unexpected Emission Data\", \"Count\":39 }, { \"Name\":\"legacyfirmware\", \"Name_Localised\":\"Specialised Legacy Firmware\", \"Count\":8 }, { \"Name\":\"symmetrickeys\", \"Name_Localised\":\"Open Symmetric Keys\", \"Count\":3 }, { \"Name\":\"encryptionarchives\", \"Name_Localised\":\"Atypical Encryption Archives\", \"Count\":8 }, { \"Name\":\"bulkscandata\", \"Name_Localised\":\"Anomalous Bulk Scan Data\", \"Count\":43 }, { \"Name\":\"disruptedwakeechoes\", \"Name_Localised\":\"Atypical Disrupted Wake Echoes\", \"Count\":46 }, { \"Name\":\"encryptedfiles\", \"Name_Localised\":\"Unusual Encrypted Files\", \"Count\":3 }, { \"Name\":\"securityfirmware\", \"Name_Localised\":\"Security Firmware Patch\", \"Count\":4 }, { \"Name\":\"classifiedscandata\", \"Name_Localised\":\"Classified Scan Fragment\", \"Count\":12 }, { \"Name\":\"fsdtelemetry\", \"Name_Localised\":\"Anomalous FSD Telemetry\", \"Count\":16 }, { \"Name\":\"adaptiveencryptors\", \"Name_Localised\":\"Adaptive Encryptors Capture\", \"Count\":6 }, { \"Name\":\"hyperspacetrajectories\", \"Name_Localised\":\"Eccentric Hyperspace Trajectories\", \"Count\":3 }, { \"Name\":\"compactemissionsdata\", \"Name_Localised\":\"Abnormal Compact Emissions Data\", \"Count\":6 }, { \"Name\":\"consumerfirmware\", \"Name_Localised\":\"Modified Consumer Firmware\", \"Count\":1 }, { \"Name\":\"industrialfirmware\", \"Name_Localised\":\"Cracked Industrial Firmware\", \"Count\":16 } ] }"),
                    "((EliteAPI.Materials))",
                    new[] { "EliteAPI.Materials.Raw[0].Name" }
                }
            };

        [Theory(DisplayName = "Command name")]
        [MemberData(nameof(Data))]
        public void Name(IEvent e, string expectedCommand, IEnumerable<string> expectedVariables)
        {
            IEventProcessor events = new EventProcessor( Mock.Of<ILogger<EventProcessor>>(), Mock.Of<IEliteDangerousApi>(), new Formatting(), Mock.Of<IVariableService>(), Mock.Of<ICommandService>());

            events.GetCommand(e).Should().Be(expectedCommand);
        }

        [Theory(DisplayName = "Command variables")]
        [MemberData(nameof(Data))]
        public void Variables(IEvent e, string expectedCommand, IEnumerable<string> expectedVariables)
        {
            //IEventProcessor events = new EventProcessor(Mock.Of<ILogger<EventProcessor>>(),Mock.Of<IEliteDangerousApi>(), new Formatting(), Mock.Of<IVariableService>(), Mock.Of<ICommandService>());

            JObject jObject = JsonConvert.DeserializeObject<JObject>(e.ToJson());

            jObject.Type.Should().Be(JsonTokenType.Comment);

            //events.GetVariables(e).Select(x => x.Name).Should().Contain(expectedVariables);
        }
    }
}