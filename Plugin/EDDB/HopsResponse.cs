using System.Collections.Generic;
using Newtonsoft.Json;

namespace Plugin.EDDB
{
    public class HopsResponse
    {
        [JsonProperty("count")] public long Count { get; private set; }

        [JsonProperty("creditsInitial")] public long CreditsInitial { get; private set; }

        [JsonProperty("creditsPreSell")] public long CreditsPreSell { get; private set; }

        [JsonProperty("creditsPostSell")] public long CreditsPostSell { get; private set; }

        [JsonProperty("buyListing")] public ListingInfo BuyListing { get; private set; }

        [JsonProperty("sellListing")] public ListingInfo SellListing { get; private set; }

        [JsonProperty("buySystem")] public BuySystemClassInfo BuySystem { get; private set; }

        [JsonProperty("buyStation")] public StationInfo BuyStation { get; private set; }

        [JsonProperty("commodity")] public CommodityInfo Commodity { get; private set; }

        [JsonProperty("sellSystem")] public BuySystemClassInfo SellSystem { get; private set; }

        [JsonProperty("sellStation")] public StationInfo SellStation { get; private set; }

        [JsonProperty("sellCommodity")] public object SellCommodity { get; private set; }

        [JsonProperty("targetSystem")] public object TargetSystem { get; private set; }

        [JsonProperty("targetSystemDistance")] public object TargetSystemDistance { get; private set; }

        [JsonProperty("distance")] public double Distance { get; private set; }

        [JsonProperty("query")] public object Query { get; private set; }

        public class ListingInfo
        {
            [JsonProperty("id")] public long Id { get; private set; }

            [JsonProperty("station_id")] public long StationId { get; private set; }

            [JsonProperty("commodity_id")] public long CommodityId { get; private set; }

            [JsonProperty("supply")] public long Supply { get; private set; }

            [JsonProperty("supply_bracket")] public long SupplyBracket { get; private set; }

            [JsonProperty("buy_price")] public long BuyPrice { get; private set; }

            [JsonProperty("sell_price")] public long SellPrice { get; private set; }

            [JsonProperty("demand")] public long Demand { get; private set; }

            [JsonProperty("demand_bracket")] public long DemandBracket { get; private set; }

            [JsonProperty("collected_at")] public long CollectedAt { get; private set; }
        }

        public class StationInfo
        {
            [JsonProperty("id")] public long Id { get; private set; }

            [JsonProperty("name")] public string Name { get; private set; }

            [JsonProperty("system_id")] public long SystemId { get; private set; }

            [JsonProperty("max_landing_pad_size")] public string MaxLandingPadSize { get; private set; }

            [JsonProperty("distance_to_star")] public long DistanceToStar { get; private set; }

            [JsonProperty("faction")] public string Faction { get; private set; }

            [JsonProperty("government")] public string Government { get; private set; }

            [JsonProperty("allegiance")] public string Allegiance { get; private set; }

            [JsonProperty("states")] public List<string> States { get; private set; }

            [JsonProperty("type_id")] public long TypeId { get; private set; }

            [JsonProperty("type")] public string Type { get; private set; }

            [JsonProperty("has_blackmarket")] public bool HasBlackmarket { get; private set; }

            [JsonProperty("has_market")] public bool HasMarket { get; private set; }

            [JsonProperty("has_refuel")] public bool HasRefuel { get; private set; }

            [JsonProperty("has_repair")] public bool HasRepair { get; private set; }

            [JsonProperty("has_rearm")] public bool HasRearm { get; private set; }

            [JsonProperty("has_outfitting")] public bool HasOutfitting { get; private set; }

            [JsonProperty("has_shipyard")] public bool HasShipyard { get; private set; }

            [JsonProperty("has_docking")] public bool HasDocking { get; private set; }

            [JsonProperty("has_commodities")] public bool HasCommodities { get; private set; }

            [JsonProperty("has_material_trader")] public bool HasMaterialTrader { get; private set; }

            [JsonProperty("has_technology_broker")]
            public bool HasTechnologyBroker { get; private set; }

            [JsonProperty("has_carrier_vendor")] public bool HasCarrierVendor { get; private set; }

            [JsonProperty("has_carrier_administration")]
            public bool HasCarrierAdministration { get; private set; }

            [JsonProperty("has_interstellar_factors")]
            public bool HasInterstellarFactors { get; private set; }

            [JsonProperty("has_universal_cartographics")]
            public bool HasUniversalCartographics { get; private set; }

            [JsonProperty("has_social_space")] public bool HasSocialSpace { get; private set; }

            [JsonProperty("import_commodities")] public List<string> ImportCommodities { get; private set; }

            [JsonProperty("export_commodities")] public List<string> ExportCommodities { get; private set; }

            [JsonProperty("prohibited_commodities")]
            public List<string> ProhibitedCommodities { get; private set; }

            [JsonProperty("economies")] public List<string> Economies { get; private set; }

            [JsonProperty("updated_at")] public long UpdatedAt { get; private set; }

            [JsonProperty("shipyard_updated_at")] public long ShipyardUpdatedAt { get; private set; }

            [JsonProperty("outfitting_updated_at")]
            public long OutfittingUpdatedAt { get; private set; }

            [JsonProperty("market_updated_at")] public long MarketUpdatedAt { get; private set; }

            [JsonProperty("is_planetary")] public bool IsPlanetary { get; private set; }

            [JsonProperty("selling_ships")] public List<string> SellingShips { get; private set; }

            [JsonProperty("selling_modules")] public List<long> SellingModules { get; private set; }
        }

        public class BuySystemClassInfo
        {
            [JsonProperty("id")] public long Id { get; private set; }

            [JsonProperty("edsm_id")] public long EdsmId { get; private set; }

            [JsonProperty("name")] public string Name { get; private set; }

            [JsonProperty("x")] public double X { get; private set; }

            [JsonProperty("y")] public double Y { get; private set; }

            [JsonProperty("z")] public double Z { get; private set; }

            [JsonProperty("faction")] public string Faction { get; private set; }

            [JsonProperty("population")] public long Population { get; private set; }

            [JsonProperty("government")] public string Government { get; private set; }

            [JsonProperty("allegiance_id")] public long AllegianceId { get; private set; }

            [JsonProperty("government_id")] public long GovernmentId { get; private set; }

            [JsonProperty("allegiance")] public string Allegiance { get; private set; }

            [JsonProperty("security")] public string Security { get; private set; }

            [JsonProperty("primary_economy")] public string PrimaryEconomy { get; private set; }

            [JsonProperty("power")] public string Power { get; private set; }

            [JsonProperty("power_state")] public string PowerState { get; private set; }

            [JsonProperty("needs_permit")] public bool NeedsPermit { get; private set; }

            [JsonProperty("updated_at")] public long UpdatedAt { get; private set; }

            [JsonProperty("simbad_ref")] public string SimbadRef { get; private set; }

            [JsonProperty("is_populated")] public bool IsPopulated { get; private set; }

            [JsonProperty("reserve_type")] public string ReserveType { get; private set; }
        }

        public class CommodityInfo
        {
            [JsonProperty("id")] public long Id { get; private set; }

            [JsonProperty("name")] public string Name { get; private set; }

            [JsonProperty("category_id")] public long CategoryId { get; private set; }

            [JsonProperty("average_price")] public long AveragePrice { get; private set; }

            [JsonProperty("is_rare")] public long IsRare { get; private set; }

            [JsonProperty("max_buy_price")] public long MaxBuyPrice { get; private set; }

            [JsonProperty("max_sell_price")] public long MaxSellPrice { get; private set; }

            [JsonProperty("min_buy_price")] public long MinBuyPrice { get; private set; }

            [JsonProperty("min_sell_price")] public long MinSellPrice { get; private set; }

            [JsonProperty("buy_price_lower_average")]
            public long BuyPriceLowerAverage { get; private set; }

            [JsonProperty("sell_price_upper_average")]
            public long SellPriceUpperAverage { get; private set; }

            [JsonProperty("is_non_marketable")] public long IsNonMarketable { get; private set; }

            [JsonProperty("ed_id")] public long EdId { get; private set; }
        }
    }
}