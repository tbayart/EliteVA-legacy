using System.Collections.Generic;
using Newtonsoft.Json;

namespace Autotrade.EDDB
{
    public class HopsRequest
    {
        public HopsRequest(int startSystem, int startStation, int maxHopDistance, int hopCount, int cargoCapacity, int availableCredits, int pad,
            int maxDistance, bool planetary)
        {
            HopsSettings.SystemId = startSystem;
            HopsSettings.StationId = startStation;
            HopsSettings.HopDistance = maxHopDistance;
            HopsSettings.HopCount = hopCount;
            HopsSettings.CargoCapacity = cargoCapacity;
            HopsSettings.Credits = availableCredits;
            StationFilter.LandingPad = pad;
            StationFilter.Distance = maxDistance;
            StationFilter.IncludePlanetary = planetary;
        }

        [JsonProperty("hopsSettings")]
        public HopsSettingsInfo HopsSettings { get; internal set; } = new HopsSettingsInfo();

        [JsonProperty("systemFilter")]
        public SystemFilterInfo SystemFilter { get; internal set; } = new SystemFilterInfo();

        [JsonProperty("stationFilter")]
        public StationFilterInfo StationFilter { get; internal set; } = new StationFilterInfo();

        [JsonProperty("_csrf")] public string Csrf { get; internal set; }

        public class HopsSettingsInfo
        {
            [JsonProperty("avoidLoop")] public bool AvoidLoop { get; internal set; }

            [JsonProperty("fuzzyMode")] public bool FuzzyMode { get; internal set; }

            [JsonProperty("implicitCommodities")]
            public List<object> ImplicitCommodities { get; internal set; } = new List<object>();

            [JsonProperty("ignoredCommodities")] public List<object> IgnoredCommodities { get; internal set; } = new List<object>();

            [JsonProperty("hopDistance")] public long HopDistance { get; internal set; }

            [JsonProperty("hopCount")] public long HopCount { get; internal set; }

            [JsonProperty("cargoCapacity")] public long CargoCapacity { get; internal set; }

            [JsonProperty("credits")] public long Credits { get; internal set; }

            [JsonProperty("priceAge")] public long PriceAge { get; internal set; }

            [JsonProperty("systemId")] public long SystemId { get; internal set; }

            [JsonProperty("stationId")] public long StationId { get; internal set; }

            [JsonProperty("targetSystemId")] public object TargetSystemId { get; internal set; }

            [JsonProperty("minSupply")] public long MinSupply { get; internal set; }

            [JsonProperty("minDemand")] public long MinDemand { get; internal set; }

            [JsonProperty("isMoreCall")] public bool IsMoreCall { get; internal set; }
        }

        public class StationFilterInfo
        {
            [JsonProperty("landingPad")] public long LandingPad { get; internal set; }

            [JsonProperty("governments")] public List<object> Governments { get; internal set; } = new List<object>();

            [JsonProperty("allegiances")] public List<object> Allegiances { get; internal set; } = new List<object>();

            [JsonProperty("states")] public List<object> States { get; internal set; } = new List<object>();

            [JsonProperty("economies")] public List<object> Economies { get; internal set; } = new List<object>();

            [JsonProperty("distance")] public object Distance { get; internal set; }

            [JsonProperty("loopDistance")] public long LoopDistance { get; internal set; }

            [JsonProperty("singleRouteDistance")] public long SingleRouteDistance { get; internal set; }

            [JsonProperty("includePlanetary")] public bool IncludePlanetary { get; internal set; }

            [JsonProperty("includeFleetCarriers")] public bool IncludeFleetCarriers { get; internal set; }
        }

        public class SystemFilterInfo
        {
            [JsonProperty("skipPermit")] public bool SkipPermit { get; internal set; }

            [JsonProperty("powers")] public List<object> Powers { get; internal set; } = new List<object>();
        }
    }
}