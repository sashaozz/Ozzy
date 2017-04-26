using Ozzy.DomainModel;
using System.Threading.Tasks;

namespace Ozzy.Server
{
    public abstract class BaseBackgroundTask
    {
        public string Id { get; set; }
        public abstract Task Execute();
        public string Content { get; set; }
    }

    public abstract class BaseBackgroundTask<T> : BaseBackgroundTask where T : class
    {
        public T ContentTyped
        {
            get
            {
                return string.IsNullOrEmpty(Content) ? null : EventSerializer.Deserialize<T>(Content);
            }
            set
            {
                Content = EventSerializer.Serialize(Content);
            }

        }

    }
}
