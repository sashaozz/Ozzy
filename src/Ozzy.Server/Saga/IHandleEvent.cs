namespace Ozzy.Server.Saga
{
    public interface IHandleEvent<T>
    {
        void Handle(T message);
    }
}
