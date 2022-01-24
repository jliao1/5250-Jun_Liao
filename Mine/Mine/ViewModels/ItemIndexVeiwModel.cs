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
        // If we rename this DataSet, we also need to rename
        // its where we use it to binding data for display
        public ObservableCollection<ItemModel> DataSet { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemIndexVeiwModel()
        {
            Title = "Items";
            DataSet = new ObservableCollection<ItemModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<ItemCreatePage, ItemModel>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as ItemModel;
                DataSet.Add(newItem);
                await DataStore.CreateAsync(newItem);
            });

            MessagingCenter.Subscribe<ItemDeletePage, ItemModel>(this, "DeleteItem", async (obj, item) =>
            {
                var data = item as ItemModel;
                await DeleteAsyc(data);
            });

            MessagingCenter.Subscribe<ItemUpdatePage, ItemModel>(this, "UpdateItem", async (obj, item) =>
            {
                var data = item as ItemModel;
                await UpdateAsyc(data);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                DataSet.Clear();
                var items = await DataStore.IndexAsync(true);
                foreach (var item in items)
                {
                    DataSet.Add(item);
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

        /// <summary>
        /// Delete the item record from the system
        /// </summary>
        /// <param name="data">The item record to delete</param>
        /// <returns>Ture if successfully deleted</returns>
        public async Task<bool> DeleteAsyc(ItemModel data)
        {
            // Check if the record exists,
            // if it does not, then False is returned
            var record = await ReadAsync(data.Id);
            if (record == null)
            {
                return false;
            }

            // Remove from the local data set cache
            DataSet.Remove(data);

            // Call to remove it from the Data Store
            var result = await DataStore.DeleteAsync(data.Id);

            return result;
        }

        public async Task<bool> UpdateAsyc(ItemModel data)
        {
            // Check if the record exists,
            // if it does not, then False is returned
            var record = await ReadAsync(data.Id);
            if (record == null)
            {
                return false;
            }

            // Call to update it from the Data Store
            var result = await DataStore.UpdateAsync(data);

            // Need the list box on ItemIndex to refresh
            // The way to do it is to force the ItemIndexViewModel
            // to refresh the dataset
            var canExecute = LoadItemsCommand.CanExecute(null);
            LoadItemsCommand.Execute(null);

            return result;
        }
    }
}