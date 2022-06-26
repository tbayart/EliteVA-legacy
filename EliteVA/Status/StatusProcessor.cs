using EliteAPI.Abstractions;
using EliteVA.Constants.Formatting.Abstractions;
using EliteVA.Services;
using EliteVA.Status.Abstractions;
using EliteVA.VoiceAttackProxy.Variables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EliteVA.Status
{
    public class StatusProcessor : IStatusProcessor
    {
        #region fields
        private readonly IEliteDangerousApi _api;
        private readonly IFormatting _formats;
        private readonly IVariableService _variables;
        private readonly ICommandService _commands;
        #endregion fields

        #region ctor
        public StatusProcessor(IEliteDangerousApi api, IFormatting formats, IVariableService variables, ICommandService commands)
        {
            _api = api;
            _formats = formats;
            _variables = variables;
            _commands = commands;
        }
        #endregion ctor

        #region IStatusProcessor
        /// <inheritdoc />
        public void Bind()
        {
            _api.Ship.Flags.OnChange += (sender, e) => { SetVariablesAndInvoke("Flags", e); };
            _api.Ship.Available.OnChange += (sender, e) => { SetVariablesAndInvoke("Available", e); };
            _api.Ship.Docked.OnChange += (sender, e) => { SetVariablesAndInvoke("Docked", e); };
            _api.Ship.Landed.OnChange += (sender, e) => { SetVariablesAndInvoke("Landed", e); };
            _api.Ship.Gear.OnChange += (sender, e) => { SetVariablesAndInvoke("Gear", e); };
            _api.Ship.Shields.OnChange += (sender, e) => { SetVariablesAndInvoke("Shields", e); };
            _api.Ship.Supercruise.OnChange += (sender, e) => { SetVariablesAndInvoke("Supercruise", e); };
            _api.Ship.FlightAssist.OnChange += (sender, e) => { SetVariablesAndInvoke("FlightAssist", e); };
            _api.Ship.Hardpoints.OnChange += (sender, e) => { SetVariablesAndInvoke("Hardpoints", e); };
            _api.Ship.Winging.OnChange += (sender, e) => { SetVariablesAndInvoke("Winging", e); };
            _api.Ship.Lights.OnChange += (sender, e) => { SetVariablesAndInvoke("Lights", e); };
            _api.Ship.CargoScoop.OnChange += (sender, e) => { SetVariablesAndInvoke("CargoScoop", e); };
            _api.Ship.SilentRunning.OnChange += (sender, e) => { SetVariablesAndInvoke("SilentRunning", e); };
            _api.Ship.Scooping.OnChange += (sender, e) => { SetVariablesAndInvoke("Scooping", e); };
            _api.Ship.SrvHandbreak.OnChange += (sender, e) => { SetVariablesAndInvoke("SrvHandbreak", e); };
            _api.Ship.MassLocked.OnChange += (sender, e) => { SetVariablesAndInvoke("MassLocked", e); };
            _api.Ship.FsdCharging.OnChange += (sender, e) => { SetVariablesAndInvoke("FsdCharging", e); };
            _api.Ship.FsdCooldown.OnChange += (sender, e) => { SetVariablesAndInvoke("FsdCooldown", e); };
            _api.Ship.LowFuel.OnChange += (sender, e) => { SetVariablesAndInvoke("LowFuel", e); };
            _api.Ship.Overheating.OnChange += (sender, e) => { SetVariablesAndInvoke("Overheating", e); };
            _api.Ship.HasLatLong.OnChange += (sender, e) => { SetVariablesAndInvoke("HasLatLong", e); };
            _api.Ship.InDanger.OnChange += (sender, e) => { SetVariablesAndInvoke("InDanger", e); };
            _api.Ship.InInterdiction.OnChange += (sender, e) => { SetVariablesAndInvoke("InInterdiction", e); };
            _api.Ship.InMothership.OnChange += (sender, e) => { SetVariablesAndInvoke("InMothership", e); };
            _api.Ship.InFighter.OnChange += (sender, e) => { SetVariablesAndInvoke("InFighter", e); };
            _api.Ship.InSrv.OnChange += (sender, e) => { SetVariablesAndInvoke("InSrv", e); };
            _api.Ship.AnalysisMode.OnChange += (sender, e) => { SetVariablesAndInvoke("AnalysisMode", e); };
            _api.Ship.NightVision.OnChange += (sender, e) => { SetVariablesAndInvoke("NightVision", e); };
            _api.Ship.AltitudeFromAverageRadius.OnChange += (sender, e) => { SetVariablesAndInvoke("AltitudeFromAverageRadius", e); };
            _api.Ship.FsdJump.OnChange += (sender, e) => { SetVariablesAndInvoke("FsdJump", e); };
            _api.Ship.SrvHighBeam.OnChange += (sender, e) => { SetVariablesAndInvoke("SrvHighBeam", e); };
            _api.Ship.Pips.OnChange += (sender, e) => { SetVariablesAndInvoke("Pips", e); };
            _api.Ship.FireGroup.OnChange += (sender, e) => { SetVariablesAndInvoke("FireGroup", e); };
            _api.Ship.GuiFocus.OnChange += (sender, e) => { SetVariablesAndInvoke("GuiFocus", e); };
            _api.Ship.Fuel.OnChange += (sender, e) => { SetVariablesAndInvoke("Fuel", e); };
            _api.Ship.Cargo.OnChange += (sender, e) => { SetVariablesAndInvoke("Cargo", e); };
            _api.Ship.LegalState.OnChange += (sender, e) => { SetVariablesAndInvoke("LegalState", e.ToString()); };
            _api.Ship.Latitude.OnChange += (sender, e) => { SetVariablesAndInvoke("Latitude", e); };
            _api.Ship.Altitude.OnChange += (sender, e) => { SetVariablesAndInvoke("Altitude", e); };
            _api.Ship.Longitude.OnChange += (sender, e) => { SetVariablesAndInvoke("Longitude", e); };
            _api.Ship.Heading.OnChange += (sender, e) => { SetVariablesAndInvoke("Heading", e); };
            _api.Ship.Body.OnChange += (sender, e) => { SetVariablesAndInvoke("Body", e); };
            _api.Ship.BodyRadius.OnChange += (sender, e) => { SetVariablesAndInvoke("BodyRadius", e); };

            _api.Commander.OnFoot.OnChange += (sender, e) => SetVariablesAndInvoke("OnFoot", e);
            _api.Commander.InTaxi.OnChange += (sender, e) => SetVariablesAndInvoke("InTaxi", e);
            _api.Commander.InMultiCrew.OnChange += (sender, e) => SetVariablesAndInvoke("InMultiCrew", e);
            _api.Commander.OnFootInStation.OnChange += (sender, e) => SetVariablesAndInvoke("OnFootInStation", e);
            _api.Commander.OnFootOnPlanet.OnChange += (sender, e) => SetVariablesAndInvoke("OnFootOnPlanet", e);
            _api.Commander.AimDownSight.OnChange += (sender, e) => SetVariablesAndInvoke("AimDownSight", e);
            _api.Commander.LowOxygen.OnChange += (sender, e) => SetVariablesAndInvoke("LowOxygen", e);
            _api.Commander.LowHealth.OnChange += (sender, e) => SetVariablesAndInvoke("LowHealth", e);
            _api.Commander.Cold.OnChange += (sender, e) => SetVariablesAndInvoke("Cold", e);
            _api.Commander.Hot.OnChange += (sender, e) => SetVariablesAndInvoke("Hot", e);
            _api.Commander.VeryCold.OnChange += (sender, e) => SetVariablesAndInvoke("VeryCold", e);
            _api.Commander.VeryHot.OnChange += (sender, e) => SetVariablesAndInvoke("VeryHot", e);
            _api.Commander.Oxygen.OnChange += (sender, e) => SetVariablesAndInvoke("Oxygen", e);
            _api.Commander.Health.OnChange += (sender, e) => SetVariablesAndInvoke("Health", e);
            _api.Commander.Temperature.OnChange += (sender, e) => SetVariablesAndInvoke("Temperature", e);
            _api.Commander.SelectedWeapon.OnChange += (sender, e) => SetVariablesAndInvoke("SelectedWeapon", e);
            _api.Commander.SelectedWeaponLocalised.OnChange += (sender, e) => SetVariablesAndInvoke("SelectedWeaponLocalised", e);
            _api.Commander.Gravity.OnChange += (sender, e) => SetVariablesAndInvoke("Gravity", e);
        }

        /// <inheritdoc />
        public IEnumerable<Variable> GetVariables(string name, object value)
        {
            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Object:
                    var properties = value.GetType().GetProperties();
                    return properties.SelectMany(p => GetVariables($"{name}.{p.Name}", p.GetValue(value)));

                default:
                    string variableName = _formats.Status.ToVariable(name);
                    return new[] { new Variable(string.Empty, variableName, value) };
            }
        }

        /// <inheritdoc />
        public string GetCommand(string name)
        {
            return _formats.Status.ToCommand(name);
        }
        #endregion IStatusProcessor

        #region methods
        void SetVariablesAndInvoke<T>(string name, T value)
        {
            var statusVariables = GetVariables(name, value);
            _variables.SetVariables("Status", statusVariables);
            var command = GetCommand(name);
            _commands.InvokeCommand(command);
        }
        #endregion methods
    }
}
