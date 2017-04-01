namespace Ozzy.DomainModel
{
    public interface ISagaFactory
    {
        TSaga GetSaga<TSaga>();
    }
}
