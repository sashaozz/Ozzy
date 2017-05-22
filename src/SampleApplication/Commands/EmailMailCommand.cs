using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace SampleApplication.Commands
{
    public class EmailMailCommand : IRequest
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Message { get; set; }
    }
}
