using System.Collections.Generic;

namespace Ozzy.DomainModel
{
    /// <summary>
    /// Читатель доменных событий из персистентного ("медленного") хранилища.
    /// </summary>
    public interface IPeristedEventsReader
    {
        /// <summary>
        /// Прочитать до maxEvents событий из персистентного хранилища начиная с checkpoint. Возвращаются отсортированные по sequence события.
        /// </summary>
        /// <param name="checkpoint">Будут прочитаны события больше данной контрлоьной точки</param>
        /// <param name="maxEvents">Макисмальное количество событий для получения из персистентного хранилища</param>
        /// <returns></returns>
        List<DomainEventRecord> GetEvents(long checkpoint, int maxEvents);

        long GetMaxSequence();
    }
}