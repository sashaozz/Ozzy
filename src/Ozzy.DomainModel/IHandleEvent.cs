namespace Ozzy.DomainModel
{
    public interface IHandleEvent<T>
    {
        void Handle(T message);
    }
}
