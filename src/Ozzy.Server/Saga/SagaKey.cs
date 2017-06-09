using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server.Saga
{
    public class SagaKey
    {
        public SagaKey(string value)
        {
            Value = value;
        }

        public SagaKey(int id, string value)
        {
            Value = value;
            Id = id;
        }

        public SagaKey() { }
        public string Value { get; set; }
        public int Id { get; private set; }
    }
}
