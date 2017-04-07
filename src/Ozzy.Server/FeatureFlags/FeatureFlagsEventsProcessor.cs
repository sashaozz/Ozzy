using Ozzy.Core;
using Ozzy.DomainModel;
using Ozzy.Server.DomainDsl;
using System;

namespace Ozzy.Server.FeatureFlags
{
    public class FeatureFlagsEventsProcessor : DomainEventsProcessor,
        IHandleEvent<DataRecordDeletedEvent<FeatureFlagRecord>>,
        IHandleEvent<DataRecordUpdatedEvent<FeatureFlagRecord>>,
        IHandleEvent<DataRecordCreatedEvent<FeatureFlagRecord>>
    {
        private IFeatureFlagService _ffService;

        public FeatureFlagsEventsProcessor(IFeatureFlagService ffService, TypedRegistration<FeatureFlag, ICheckpointManager> checkpointManager)
            : base(checkpointManager.GetService())
        {
            Guard.ArgumentNotNull(ffService, nameof(ffService));
            _ffService = ffService;            
        }       

        public bool Handle(DataRecordDeletedEvent<FeatureFlagRecord> obj)
        {
            throw new NotImplementedException();
        }

        public bool Handle(DataRecordUpdatedEvent<FeatureFlagRecord> obj)
        {
            if (obj.RecordType != typeof(FeatureFlagRecord)) return true;
            var newFlag = obj.RecordValue as FeatureFlagRecord;
            _ffService.SetFlagState(newFlag.Id, newFlag.Configuration, newFlag.Version);
            return false;
        }

        public bool Handle(DataRecordCreatedEvent<FeatureFlagRecord> obj)
        {
            if (obj.RecordType != typeof(FeatureFlagRecord)) return true;
            var newFlag = obj.RecordValue as FeatureFlagRecord;
            _ffService.SetFlagState(newFlag.Id, newFlag.Configuration, newFlag.Version);
            return false;
        }
    }
}
