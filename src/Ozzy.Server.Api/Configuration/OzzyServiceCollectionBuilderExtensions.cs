using System.Runtime.Serialization.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Ozzy.Server.Configuration
{
    public static class OzzyServiceCollectionBuilderExtensions
    {
        public static IOzzyBuilder AddApi(this IOzzyBuilder builder)
        {
            builder.Services.AddMvcCore()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
            return builder;
        }
    }
}
