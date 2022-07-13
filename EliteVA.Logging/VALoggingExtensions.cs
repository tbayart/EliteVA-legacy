using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Somfic.Logging.VoiceAttack;
using System.IO;
using System.Reflection;

namespace EliteVA.Logging
{
    public static class VALoggingExtensions
    {
        public static ILoggingBuilder AddVoiceAttack(ILoggingBuilder loggingBuilder, dynamic vaProxy)
        {
            VoiceAttackLoggerExtensions.AddVoiceAttack(loggingBuilder, vaProxy);
            return loggingBuilder;
        }

        public static ILoggingBuilder AddLogger(this ILoggingBuilder loggingBuilder)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configfile = Path.Combine(path, "nlog.config");
            return loggingBuilder.AddNLog(configfile);
        }
    }
}
