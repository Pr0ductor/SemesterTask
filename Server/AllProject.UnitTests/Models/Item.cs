using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllProject.UnitTests.Models
{
    /// <summary>
    /// Представляет элемент с базовыми свойствами.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Получает или задает название элемента.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Получает или задает значение, связанное с элементом.
        /// </summary>
        public int Value { get; set; }
    }
}
