using System;
using EliteVA.Constants.Proxy.Abstractions;
using EliteVA.VoiceAttackProxy.Abstractions;

namespace EliteVA.Constants.Proxy
{
    public class Proxy : IProxy
    {
        private readonly IServiceProvider services;
        private IVoiceAttackProxy proxy;
        
        public Proxy(IServiceProvider services)
        {
            this.services = services;
        }

        /// <inheritdoc />
        public void SetProxy(dynamic proxy)
        {
            this.proxy = new VoiceAttackProxy.VoiceAttackProxy(proxy, services);
        }

        /// <inheritdoc />
        public IVoiceAttackProxy GetProxy()
        {
            return this.proxy;
        }
    }
}