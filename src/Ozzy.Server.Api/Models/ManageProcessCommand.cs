using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server.Api.Models
{
   public class ManageProcessCommand
    {
        public string NodeId { get; set; }
        public Guid ProcessId { get; set; }
    }
}
