using System;
using System.Linq;
using System.Threading.Tasks;
using SQLite;

using Mine.Models;
using System.Collections.Generic;

namespace Mine.Services
{
    public class DatabaseService: IDataStore<ItemModel>
    {
        static readonly Lazy<SQLiteAsyncConnection> LazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => LazyInitializer.Value;
        static bool initialized = false;

        public DatabaseService()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(ItemModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(ItemModel)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        /// <summary>
        /// Create a new record for the data passed in
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(ItemModel item)
        {
            if (item == null)
            {
                return false;
            }
            var result = await Database.InsertAsync(item);
            if (result == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Update the record passed in if it exists
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ItemModel item)
        {
            if (item == null)
            {
                return false;
            }
            var result = await Database.UpdateAsync(item);
            if (result == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Delete the record of the ID passed in
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<bool> DeleteAsync(string id)
        {
            var data = await ReadAsync(id);
            if (data == null)
            {
                return false;
            }
            var result = await Database.DeleteAsync(data);
            if (result == 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Return the record for the ID passed in
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ItemModel> ReadAsync(string id)
        {
            if (id == null)
            {
                return null;
            }

            // Call the Database to read the id
            // Using the  Ling syntax   Find the first record that has the matched id 
            var result = Database.Table<ItemModel>().FirstOrDefaultAsync(m => m.Id.Equals(id));

            return result;
        }

        /// <summary>
        /// Return all records in the database
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ItemModel>> IndexAsync(bool forceRefresh = false)
        {
            var result = await Database.Table<ItemModel>().ToListAsync();
            return result;
        }
    }
}
