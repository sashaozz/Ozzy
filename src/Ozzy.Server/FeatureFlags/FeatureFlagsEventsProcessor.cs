using Ozzy.Core;
using Ozzy.DomainModel;
using Ozzy.Server.DomainDsl;
using System;

namespace Ozzy.Server.FeatureFlags
{
    public class FeatureFlagsEventsProcessor : BaseEventsProcessor
    {
        private IFeatureFlagService _ffService;

        public FeatureFlagsEventsProcessor(IFeatureFlagService ffService, TypedRegistration<FeatureFlag, ICheckpointManager> checkpointManager)
            : base(checkpointManager.GetService())
        {
            Guard.ArgumentNotNull(ffService, nameof(ffService));
            _ffService = ffService;

            AddHandler<DataRecordCreatedEvent<FeatureFlagRecord>>(Handle);
            AddHandler<DataRecordUpdatedEvent<FeatureFlagRecord>>(Handle);
            AddHandler<DataRecordDeletedEvent<FeatureFlagRecord>>(Handle);
        }       

        private void Handle(DataRecordDeletedEvent<FeatureFlagRecord> obj)
        {
            throw new NotImplementedException();
        }

        private void Handle(DataRecordUpdatedEvent<FeatureFlagRecord> obj)
        {
            if (obj.RecordType != typeof(FeatureFlagRecord)) return;
            var newFlag = obj.RecordValue as FeatureFlagRecord;
            _ffService.SetFlagState(newFlag.Id, newFlag.Configuration, newFlag.Version);
        }

        private void Handle(DataRecordCreatedEvent<FeatureFlagRecord> obj)
        {
            if (obj.RecordType != typeof(FeatureFlagRecord)) return;
            var newFlag = obj.RecordValue as FeatureFlagRecord;
            _ffService.SetFlagState(newFlag.Id, newFlag.Configuration, newFlag.Version);
        }
    }
}
