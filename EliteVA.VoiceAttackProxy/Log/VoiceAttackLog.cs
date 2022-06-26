using System;
using System.Threading.Tasks;

namespace EliteVA.VoiceAttackProxy.Log
{
    public class VoiceAttackLog
    {
        private readonly dynamic _proxy;

        internal VoiceAttackLog(dynamic proxy, IServiceProvider services = null)
        {
            _proxy = proxy;
        }

        public Task Write(string content, VoiceAttackColor color)
        {
            _proxy.WriteToLog(content, color.ToString());
            return Task.CompletedTask;
        }

        public Task Clear()
        {
            _proxy.ClearLog();
            return Task.CompletedTask;
        }
    }
}
