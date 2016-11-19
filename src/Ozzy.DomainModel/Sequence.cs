namespace Ozzy.DomainModel
{
    /// <summary>
    /// Именнованная последовательность, описывающая позицию обработчика в очереди доменных событий. 
    /// </summary>
    public class Sequence
    {
        /// <summary>
        /// Название обработчика последовательности
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Значение последовательности на котором в данны момент находится обработчик
        /// </summary>
        public long Number { get; set; }

    }
}
