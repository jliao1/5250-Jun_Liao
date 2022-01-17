using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mine.Models;

namespace Mine.Services
{
    public class MockDataStore : IDataStore<ItemModel>
    {
        readonly List<ItemModel> items;

        public MockDataStore()
        {
            items = new List<ItemModel>()
            {
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Chicken", Description="Saute diced chicken with hot peppers" },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Tofu", Description="Stwed beancurd with minced pork in pepper sauce" },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Rib", Description="Pork fillets with sweet&sour sauce" },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Cabbage", Description="Cabbage&pepper in sweet&sour sauce" },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Soup", Description="Pork with Sichuan cabbage soup" },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Rice", Description="Fried rice with egg" }
            };
        }

        public async Task<bool> AddItemAsync(ItemModel item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(ItemModel item)
        {
            var oldItem = items.Where((ItemModel arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((ItemModel arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<ItemModel> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<ItemModel>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}