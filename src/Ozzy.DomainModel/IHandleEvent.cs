namespace Ozzy.DomainModel
{
    public interface IHandleEvent<T>
    {
        bool Handle(T message);
    }
}
