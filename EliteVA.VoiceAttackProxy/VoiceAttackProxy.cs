using EliteVA.VoiceAttackProxy.Abstractions;
using EliteVA.VoiceAttackProxy.Audio;
using EliteVA.VoiceAttackProxy.Commands;
using EliteVA.VoiceAttackProxy.Log;
using EliteVA.VoiceAttackProxy.Options;
using EliteVA.VoiceAttackProxy.Paths;
using EliteVA.VoiceAttackProxy.Variables;
using EliteVA.VoiceAttackProxy.Versions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EliteVA.VoiceAttackProxy
{
    public class VoiceAttackProxy : IVoiceAttackProxy
    {
        private readonly dynamic _vaProxy;

        public VoiceAttackProxy(dynamic vaProxy, IServiceProvider services)
        {
            _vaProxy = vaProxy;

            Variables = new VoiceAttackVariables(vaProxy, services);
            Versions = new VoiceAttackVersions(vaProxy, services);
            Log = new VoiceAttackLog(vaProxy, services);
            Paths = new VoiceAttackPaths(vaProxy, services);
            Options = new VoiceAttackOptions(vaProxy, services);
            Speech = new VoiceAttackSpeech(vaProxy, services);
            Command = new VoiceAttackCommand(vaProxy,services);
            Commands = new VoiceAttackCommands(vaProxy, services);
        }

        /// <inheritdoc />
        public string Context => _vaProxy.Context;

        /// <inheritdoc />
        public bool HasStopped => _vaProxy.Stopped;

        /// <inheritdoc />
        public IReadOnlyCollection<string> Profiles => _vaProxy.ProfileNames();

        /// <inheritdoc />
        public IntPtr Handle => _vaProxy.MainWindowHandle;

        /// <inheritdoc />
        public VoiceAttackVariables Variables { get; }

        /// <inheritdoc />
        public VoiceAttackVersions Versions { get; }

        /// <inheritdoc />
        public VoiceAttackLog Log { get; }

        /// <inheritdoc />
        public VoiceAttackPaths Paths { get; }

        /// <inheritdoc />
        public VoiceAttackOptions Options { get; }

        /// <inheritdoc />
        public VoiceAttackSpeech Speech { get; }

        /// <inheritdoc />
        public VoiceAttackCommands Commands { get; }

        /// <inheritdoc />
        public VoiceAttackCommand Command { get; }

        /// <inheritdoc />
        public Task<IReadOnlyCollection<string>> GeneratePhrases(string query, bool trimSpaces = false, bool lowercase = false)
        {
            return Task.FromResult(_vaProxy.ExtractPhrases(query, trimSpaces, lowercase));
        }

        /// <inheritdoc />
        public Task Opacity(int percentage)
        {
            _vaProxy.SetOpacity(percentage);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task ResetStopFlag()
        {
            _vaProxy.ResetStopFlag();
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task Close()
        {
            _vaProxy.Close();
            return Task.CompletedTask;
        }
    }
}