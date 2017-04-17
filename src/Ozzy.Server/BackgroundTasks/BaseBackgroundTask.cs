using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ozzy.Server.BackgroundTasks
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
                return string.IsNullOrEmpty(Content) ? null : JsonConvert.DeserializeObject<T>(Content);
            }
            set
            {
                Content = JsonConvert.SerializeObject(Content);
            }

        }

    }
}
