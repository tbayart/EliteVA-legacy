using EliteVA.Constants.Proxy.Abstractions;

using Somfic.VoiceAttack.Proxy;
using Somfic.VoiceAttack.Proxy.Abstractions;

namespace EliteVA.Constants.Proxy
{
    public class Proxy : IProxy
    {
        private IVoiceAttackProxy proxy;

        /// <inheritdoc />
        public void SetProxy(dynamic proxy)
        {
            this.proxy = new VoiceAttackProxy(proxy);
        }

        /// <inheritdoc />
        public IVoiceAttackProxy GetProxy()
        {
            return this.proxy;
        }
    }
}