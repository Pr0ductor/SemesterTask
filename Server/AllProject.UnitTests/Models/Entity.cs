using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllProject.UnitTests.Models
{
    /// <summary>
    /// Представляет сущность с базовыми свойствами.
    /// </summary>
    public class Entity
    { 
        /// <summary>
        /// Получает или задает уникальный идентификатор сущности.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Получает или задает имя сущности.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Получает или задает значение, связанное с сущностью.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Получает или задает информацию о логине сущности.
        /// </summary>
        public string Login { get; set; }
    }
}
