using System;

namespace Ozzy.Server
{
    public class BackgroundTaskConfig
    {
        private Type _taskType;
        private Type _taskConfigType;
        public string TaskTypeString { get; private set; }
        public string TaskConfigTypeString { get; private set; }
        public byte[] SerializedConfig { get; private set; }
        public BackgroundTaskConfig(string taskTypeString, string taskConfigTypeString, byte[] serializedConfig)
        {
            Guard.ArgumentNotNullOrEmptyString(taskTypeString, nameof(taskTypeString));
            Guard.ArgumentNotNullOrEmptyString(taskConfigTypeString, nameof(taskConfigTypeString));
            Guard.ArgumentNotNull(serializedConfig, nameof(serializedConfig));
            TaskTypeString = taskTypeString;
            TaskConfigTypeString = taskConfigTypeString;
            SerializedConfig = serializedConfig;
            _taskType = Type.GetType(TaskTypeString);
            _taskConfigType = Type.GetType(TaskConfigTypeString);
        }
        public BackgroundTaskConfig(Type taskType, Type taskConfigType, byte[] serializedConfig)
        {
            Guard.ArgumentNotNull(taskType, nameof(taskType));
            Guard.ArgumentNotNull(taskConfigType, nameof(taskConfigType));
            Guard.ArgumentNotNull(serializedConfig, nameof(serializedConfig));
            _taskType = taskType;
            _taskConfigType = taskConfigType;
            SerializedConfig = serializedConfig;
            TaskTypeString = _taskType.AssemblyQualifiedName;
            TaskConfigTypeString = taskConfigType.AssemblyQualifiedName;
        }

        public Type GetTaskType() { return _taskType; }
        public Type GetTaskConfigType() { return _taskConfigType; }


    }
}
