namespace Ozzy.Server
{
    public interface ISagaFactory
    {
        TSaga GetSaga<TSaga>();
    }
}
