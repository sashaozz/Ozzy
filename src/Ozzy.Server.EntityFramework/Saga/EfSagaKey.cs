namespace Ozzy.Server.EntityFramework
{
    public class EfSagaKey
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public virtual EfSagaRecord Saga { get; set; }
    }
}