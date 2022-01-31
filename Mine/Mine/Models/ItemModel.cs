using System;
using SQLite;

namespace Mine.Models
{
    /// <summary>
    /// Items for the characters and Monsters to use
    /// </summary>
    public class ItemModel
    {
        // The Id for the Item
        [PrimaryKey]
        public string Id { get; set; } = System.Guid.NewGuid().ToString();
        // The Display Text for the Item
        public string Text { get; set; }
        // The Descirption for the Item
        public string Description { get; set; }
        // The Value of the Item +9 Damage
        public int Value { get; set; } = 0;
    }
}