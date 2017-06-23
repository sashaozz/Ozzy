using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozzy.Server.EntityFramework
{
    public class EfSagaLogEntry
    {
        public int Id { get; set; }
        public Guid SagaId { get; set; }
        public string StateType { get;  set; }
        public DateTime ClosedAt { get; set; }
        public byte[] SagaState { get; set; }
    }
}
