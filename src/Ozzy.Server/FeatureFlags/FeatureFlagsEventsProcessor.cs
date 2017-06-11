using Ozzy.Core;
using Ozzy.DomainModel;
using System;

namespace Ozzy.Server
{
    public class FeatureFlagsEventsProcessor : DomainEventsProcessor,
        IHandleEvent<DataRecordDeletedEvent<FeatureFlag>>,
        IHandleEvent<DataRecordUpdatedEvent<FeatureFlag>>,
        IHandleEvent<DataRecordCreatedEvent<FeatureFlag>>
    {
        private IFeatureFlagService _ffService;

        public FeatureFlagsEventsProcessor(IFeatureFlagService ffService, TypedRegistration<FeatureFlag, ICheckpointManager> checkpointManager)
            : base(checkpointManager.GetService())
        {
            Guard.ArgumentNotNull(ffService, nameof(ffService));
            _ffService = ffService;            
        }       

        public void Handle(DataRecordDeletedEvent<FeatureFlag> obj)
        {
            throw new NotImplementedException();
        }

        public void Handle(DataRecordUpdatedEvent<FeatureFlag> obj)
        {
            //if (obj.RecordType != typeof(FeatureFlag)) return true;
            var newFlag = obj.RecordValue as FeatureFlag;
            _ffService.SetFlagState(newFlag.Id, newFlag.Configuration, newFlag.Version);
        }

        public void Handle(DataRecordCreatedEvent<FeatureFlag> obj)
        {
            //if (obj.RecordType != typeof(FeatureFlag)) return true;
            var newFlag = obj.RecordValue as FeatureFlag;
            _ffService.SetFlagState(newFlag.Id, newFlag.Configuration, newFlag.Version);
        }
    }
}
