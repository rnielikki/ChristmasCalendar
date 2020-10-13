using System;

namespace AdventCalendar.Models
{
    /// <summary>
    /// Provides public DateTime wrapper, both for server and client.
    /// </summary>
    public interface IDateTime
    {
        /// <summary>
        /// <see cref="DateTime"/> for the wrap.
        /// </summary>
        DateTime Now { get; }
        /// <summary>
        /// <see cref="DateTime.Year"/> for the wrap.
        /// </summary>
        int Year { get; }
    }
    /// <summary>
    /// Default implementation for public DateTime wrapper, both for server and client.
    /// </summary>
    public class DefaultDateTime : IDateTime
    {
        /// <summary>
        /// Provides Real <see cref="DateTime.Now"/>.
        /// </summary>
        public DateTime Now { get => DateTime.Now; }
        /// <summary>
        /// Provides Real Year from <see cref="DateTime.Now"/>.
        /// </summary>
        public int Year { get => Now.Year; }
    }
}
