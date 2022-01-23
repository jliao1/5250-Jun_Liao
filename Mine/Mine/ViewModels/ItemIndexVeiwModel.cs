using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Mine.Models;
using Mine.Views;

namespace Mine.ViewModels
{
    /// <summary>
    /// All of our data operations will start in the <type>IndexViewModel 
    /// (consider this the entry point to the datastore)
    /// </summary>
    public class ItemIndexVeiwModel : BaseViewModel
    {
        public ObservableCollection<ItemModel> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemIndexVeiwModel()
        {
            Title = "Items";
            Items = new ObservableCollection<ItemModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<ItemCreatePage, ItemModel>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as ItemModel;
                Items.Add(newItem);
                await DataStore.CreateAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.IndexAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Call to the DataStore.ReadAsync passing the id, get an
        /// item record from the datastore into a variable called result
        /// and then return result. This is to make debugging, maintenance easier.
        /// 
        /// The reason we need this method is defensive programing. We 
        /// want to hookup Delete, before delete an item, we want to 
        /// check if it exists first, so we need defensive programing.
        /// There is a read method in the MockDataStore, so just need to 
        /// call to it. This “ReadAsync” method that will read the item 
        /// from the datastore, If the item exist, then Delete can continue.

        /// </summary>
        /// <param name="id">Id of the Record</param>
        /// <returns>The record from ReadAsync</returns>
        public async Task<ItemModel> ReadAsync(string id)
        {
            var result = await DataStore.ReadAsync(id);
            return result; 
        }

    }
}