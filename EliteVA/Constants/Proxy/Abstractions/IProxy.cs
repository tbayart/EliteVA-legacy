using IVoiceAttackProxy = EliteVA.VoiceAttackProxy.Abstractions.IVoiceAttackProxy;

namespace EliteVA.Constants.Proxy.Abstractions
{
    public interface IProxy
    {
        /// <summary>
        /// Sets or updates the Proxy object
        /// </summary>
        void SetProxy(dynamic proxy);
        
        /// <summary>
        /// Gets the Proxy object
        /// </summary>
        IVoiceAttackProxy GetProxy();
    }
}