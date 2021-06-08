using System;
using System.Collections.Generic;
using System.Linq;

using EliteAPI.Abstractions;

using EliteVA.Constants.Formatting.Abstractions;
using EliteVA.Services;
using EliteVA.Services.Variable;
using EliteVA.Status.Abstractions;

namespace EliteVA.Status
{
    public class StatusProcessor : IStatusProcessor
    {
        private readonly IEliteDangerousApi api;
        private readonly IFormatting formats;
        private readonly IVariableService variables;
        private readonly ICommandService commands;

        public StatusProcessor(IEliteDangerousApi api, IFormatting formats, IVariableService variables, ICommandService commands)
        {
            this.api = api;
            this.formats = formats;
            this.variables = variables;
            this.commands = commands;
        }

        /// <inheritdoc />
        public void Bind()
        {
            api.Ship.Flags.OnChange += (sender, e) => { SetVariablesAndInvoke("Flags", e); };
            api.Ship.Available.OnChange += (sender, e) => { SetVariablesAndInvoke("Available", e); };
            api.Ship.Docked.OnChange += (sender, e) => { SetVariablesAndInvoke("Docked", e); };
            api.Ship.Landed.OnChange += (sender, e) => { SetVariablesAndInvoke("Landed", e); };
            api.Ship.Gear.OnChange += (sender, e) => { SetVariablesAndInvoke("Gear", e); };
            api.Ship.Shields.OnChange += (sender, e) => { SetVariablesAndInvoke("Shields", e); };
            api.Ship.Supercruise.OnChange += (sender, e) => { SetVariablesAndInvoke("Supercruise", e); };
            api.Ship.FlightAssist.OnChange += (sender, e) => { SetVariablesAndInvoke("FlightAssist", e); };
            api.Ship.Hardpoints.OnChange += (sender, e) => { SetVariablesAndInvoke("Hardpoints", e); };
            api.Ship.Winging.OnChange += (sender, e) => { SetVariablesAndInvoke("Winging", e); };
            api.Ship.Lights.OnChange += (sender, e) => { SetVariablesAndInvoke("Lights", e); };
            api.Ship.CargoScoop.OnChange += (sender, e) => { SetVariablesAndInvoke("CargoScoop", e); };
            api.Ship.SilentRunning.OnChange += (sender, e) => { SetVariablesAndInvoke("SilentRunning", e); };
            api.Ship.Scooping.OnChange += (sender, e) => { SetVariablesAndInvoke("Scooping", e); };
            api.Ship.SrvHandbreak.OnChange += (sender, e) => { SetVariablesAndInvoke("SrvHandbreak", e); };
            api.Ship.MassLocked.OnChange += (sender, e) => { SetVariablesAndInvoke("MassLocked", e); };
            api.Ship.FsdCharging.OnChange += (sender, e) => { SetVariablesAndInvoke("FsdCharging", e); };
            api.Ship.FsdCooldown.OnChange += (sender, e) => { SetVariablesAndInvoke("FsdCooldown", e); };
            api.Ship.LowFuel.OnChange += (sender, e) => { SetVariablesAndInvoke("LowFuel", e); };
            api.Ship.Overheating.OnChange += (sender, e) => { SetVariablesAndInvoke("Overheating", e); };
            api.Ship.HasLatLong.OnChange += (sender, e) => { SetVariablesAndInvoke("HasLatLong", e); };
            api.Ship.InDanger.OnChange += (sender, e) => { SetVariablesAndInvoke("InDanger", e); };
            api.Ship.InInterdiction.OnChange += (sender, e) => { SetVariablesAndInvoke("InInterdiction", e); };
            api.Ship.InMothership.OnChange += (sender, e) => { SetVariablesAndInvoke("InMothership", e); };
            api.Ship.InFighter.OnChange += (sender, e) => { SetVariablesAndInvoke("InFighter", e); };
            api.Ship.InSrv.OnChange += (sender, e) => { SetVariablesAndInvoke("InSrv", e); };
            api.Ship.AnalysisMode.OnChange += (sender, e) => { SetVariablesAndInvoke("AnalysisMode", e); };
            api.Ship.NightVision.OnChange += (sender, e) => { SetVariablesAndInvoke("NightVision", e); };
            api.Ship.AltitudeFromAverageRadius.OnChange += (sender, e) => { SetVariablesAndInvoke("AltitudeFromAverageRadius", e); };
            api.Ship.FsdJump.OnChange += (sender, e) => { SetVariablesAndInvoke("FsdJump", e); };
            api.Ship.SrvHighBeam.OnChange += (sender, e) => { SetVariablesAndInvoke("SrvHighBeam", e); };
            api.Ship.Pips.OnChange += (sender, e) => { SetVariablesAndInvoke("Pips", e); };
            api.Ship.FireGroup.OnChange += (sender, e) => { SetVariablesAndInvoke("FireGroup", e); };
            api.Ship.GuiFocus.OnChange += (sender, e) => { SetVariablesAndInvoke("GuiFocus", e); };
            api.Ship.Fuel.OnChange += (sender, e) => { SetVariablesAndInvoke("Fuel", e); };
            api.Ship.Cargo.OnChange += (sender, e) => { SetVariablesAndInvoke("Cargo", e); };
            api.Ship.LegalState.OnChange += (sender, e) => { SetVariablesAndInvoke("LegalState", e.ToString()); };
            api.Ship.Latitude.OnChange += (sender, e) => { SetVariablesAndInvoke("Latitude", e); };
            api.Ship.Altitude.OnChange += (sender, e) => { SetVariablesAndInvoke("Altitude", e); };
            api.Ship.Longitude.OnChange += (sender, e) => { SetVariablesAndInvoke("Longitude", e); };
            api.Ship.Heading.OnChange += (sender, e) => { SetVariablesAndInvoke("Heading", e); };
            api.Ship.Body.OnChange += (sender, e) => { SetVariablesAndInvoke("Body", e); };
            api.Ship.BodyRadius.OnChange += (sender, e) => { SetVariablesAndInvoke("BodyRadius", e); };

            api.Commander.OnFoot.OnChange += (sender, e) => SetVariablesAndInvoke("OnFoot", e);
            api.Commander.InTaxi.OnChange += (sender, e) => SetVariablesAndInvoke("InTaxi", e);
            api.Commander.InMultiCrew.OnChange += (sender, e) => SetVariablesAndInvoke("InMultiCrew", e);
            api.Commander.OnFootInStation.OnChange += (sender, e) => SetVariablesAndInvoke("OnFootInStation", e);
            api.Commander.OnFootOnPlanet.OnChange += (sender, e) => SetVariablesAndInvoke("OnFootOnPlanet", e);
            api.Commander.AimDownSight.OnChange += (sender, e) => SetVariablesAndInvoke("AimDownSight", e);
            api.Commander.LowOxygen.OnChange += (sender, e) => SetVariablesAndInvoke("LowOxygen", e);
            api.Commander.LowHealth.OnChange += (sender, e) => SetVariablesAndInvoke("LowHealth", e);
            api.Commander.Cold.OnChange += (sender, e) => SetVariablesAndInvoke("Cold", e);
            api.Commander.Hot.OnChange += (sender, e) => SetVariablesAndInvoke("Hot", e);
            api.Commander.VeryCold.OnChange += (sender, e) => SetVariablesAndInvoke("VeryCold", e);
            api.Commander.VeryHot.OnChange += (sender, e) => SetVariablesAndInvoke("VeryHot", e);
            api.Commander.Oxygen.OnChange += (sender, e) => SetVariablesAndInvoke("Oxygen", e);
            api.Commander.Health.OnChange += (sender, e) => SetVariablesAndInvoke("Health", e);
            api.Commander.Temperature.OnChange += (sender, e) => SetVariablesAndInvoke("Temperature", e);
            api.Commander.SelectedWeapon.OnChange += (sender, e) => SetVariablesAndInvoke("SelectedWeapon", e);
            api.Commander.SelectedWeaponLocalised.OnChange += (sender, e) => SetVariablesAndInvoke("SelectedWeaponLocalised", e);
            api.Commander.Gravity.OnChange += (sender, e) => SetVariablesAndInvoke("Gravity", e);
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
                    string variableName = formats.Status.ToVariable(name);
                    return new[] {new Variable(variableName, value)};
            }
        }

        /// <inheritdoc />
        public string GetCommand(string name)
        {
            return formats.Status.ToCommand(name);
        }

        void SetVariablesAndInvoke<T>(string name, T value)
        {
            var statusVariables = GetVariables(name, value);
            var command = GetCommand(name);
            variables.SetVariables("Status", statusVariables);

            commands.InvokeCommand(command);
        }
    }
}