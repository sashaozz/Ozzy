using Ozzy.Core;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Ozzy.Server
{
    public class FeatureFlagService : IFeatureFlagService
    {
        private IServiceProvider _serviceProvider;
        private ConcurrentDictionary<string, FeatureFlag> _flags = new ConcurrentDictionary<string, FeatureFlag>();
        private IFeatureFlagRepository _ffRepository;

        public FeatureFlagService(IFeatureFlagRepository ffRepository, IServiceProvider serviceProvider)
        {
            Guard.ArgumentNotNull(ffRepository, nameof(ffRepository));
            _ffRepository = ffRepository;
            _serviceProvider = serviceProvider;
        }

        protected virtual FeatureFlag CreateFlag(string code, Type type)
        {
            Guard.ArgumentNotNullOrEmptyString(code, nameof(code));
            Guard.ArgumentNotNull(type, nameof(type));

            if (type == typeof(FeatureFlag))
            {
                return new FeatureFlag(code);
            }
            return _serviceProvider.GetService(type) as FeatureFlag;
        }

        public virtual TVariation GetVariation<TFeature, TVariation>() where TFeature : FeatureFlag<TVariation>
        {
            var flag = GetFeatureFlag<TFeature>();
            if (flag == null)
            {
                //todo: log                
                return default(TVariation);
            }
            return flag.GetVariation();
        }

        public virtual TFeature GetFeatureFlag<TFeature>() where TFeature : FeatureFlag
        {
            var type = typeof(TFeature);
            var code = type.FullName;
            _flags.TryGetValue(code, out var flag);
            if (flag == null)
            {
                flag = CreateFlag(code, type);
                var existedFlag = _ffRepository.Query().FirstOrDefault(f => f.Id == code);                
                if (existedFlag != null)
                {
                    flag.UpdateConfiguration(existedFlag.Configuration, existedFlag.Version);
                }
            }
            return flag as TFeature;
        }

        public bool IsEnabled<TFeature>() where TFeature : FeatureFlag
        {
            return GetFeatureFlag<TFeature>().IsEnabled();
        }

        public FeatureFlag GetFeatureFlag(string code)
        {
            _flags.TryGetValue(code, out var flag);
            if (flag == null)
            {
                return CreateFlag(code, typeof(FeatureFlag));
            }
            return flag;
        }

        public bool IsEnabled(string code, bool defaultValue = false)
        {
            var flag = GetFeatureFlag(code);
            return flag == null ? defaultValue : flag.IsEnabled();
        }

        public string GetVariation(string code, string defaultValue = null)
        {
            var flag = GetFeatureFlag(code);
            return flag == null ? defaultValue : flag.GetVariation();
        }

        public void SetFlagState(string code, FeatureFlagConfiguration state, int version)
        {
            var flag = GetFeatureFlag(code);
            flag.UpdateConfiguration(state, version);
        }
    }
}
