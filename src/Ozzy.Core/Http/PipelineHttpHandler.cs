using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Ozzy.Core.Http
{
    public class PipelineHttpHandler : DelegatingHandler
    {
        public PipelineHttpHandler(IEnumerable<DelegatingHandler> handlers, HttpMessageHandler innerHandler)
        {
            Guard.ArgumentNotNull(innerHandler, "innerHandler");
            if (handlers == null)
            {
                InnerHandler = innerHandler;
                return;
            }

            // Wire handlers up in reverse order starting with the inner handler
            HttpMessageHandler pipeline = innerHandler;
            IEnumerable<DelegatingHandler> reversedHandlers = handlers.Reverse();
            foreach (DelegatingHandler handler in reversedHandlers)
            {
                if (handler == null)
                {
                    throw new ArgumentException("Handler in the pipeline is null", nameof(handlers));
                }

                if (handler.InnerHandler != null)
                {
                    throw new ArgumentException("Handler in the pipeline contains non null InnerHandler", nameof(handlers));
                }

                handler.InnerHandler = pipeline;
                pipeline = handler;
            }

            InnerHandler = pipeline;
        }
    }
}
