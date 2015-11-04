using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CookbookWin10
{
    // https://msdn.microsoft.com/en-us/library/windows/apps/mt185401.aspx
    class StorageManager
    {
        private const string fileName = "favorites.txt";
        private StorageFolder storageFolder = ApplicationData.Current.LocalFolder;        


        public async Task<bool> createFile()
        {
            try
            {
                IStorageItem item = await storageFolder.TryGetItemAsync(fileName);
                if (item == null)
                {
                    IStorageFile myFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                }
            }
            catch (Exception ex)
            {
                printError(ex);
                return false;
            }
            return true;
            
        }

        public async Task<bool> fileExists()
        {
            try
            {
                IStorageItem item = await storageFolder.TryGetItemAsync(fileName);
                if (item == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                printError(ex);
                return false;
            }
            return true;

        }

        private async Task<string> readFromFile()
        {
            try
            {
                IStorageItem item = await storageFolder.TryGetItemAsync(fileName);
                if(item.IsOfType(StorageItemTypes.File))
                {
                    IStorageFile file = (IStorageFile)item;
                    return await FileIO.ReadTextAsync(file);
                }                
            }
            catch (Exception ex)
            {
                printError(ex);                
            }
            return "";
        }

        private async Task<bool> writeToFile(string input)
        {
            try
            {
                IStorageItem item = await storageFolder.TryGetItemAsync(fileName);
                if (item.IsOfType(StorageItemTypes.File))
                {
                    IStorageFile file = (IStorageFile)item;
                    await FileIO.WriteTextAsync(file, input);
                }
            }
            catch (Exception ex)
            {
                printError(ex);
                return false;
            }
            return true;
        }

        public async void addNewFavorite(int id)
        {
            string contents = await readFromFile();

            if (contents.Length <= 0)
            {
                await writeToFile(id.ToString());
            }
            else
            {
                string[] tokens = contents.Split(',');
                int[] arr = new int[tokens.Length];


                for (int x = 0; x < tokens.Length; x++)
                {
                    int local;
                    Int32.TryParse(tokens[x], out local);
                    arr[x] = local;
                }

                if (arr.ToList().IndexOf(id) == -1)
                {
                    await writeToFile(contents + "," + id);
                }
            }
        }

        public async void removeFavorite(int id)
        {
            string contents = await readFromFile();

            if(contents.Length > 0)
            {
                string[] tokens = contents.Split(',');
                int[] arr = new int[tokens.Length]; // dangerous?
                for (int x = 0; x < tokens.Length; x++)
                {
                    int local;
                    Int32.TryParse(tokens[x], out local);
                    if (local != id)
                    {
                        arr[x] = local;
                    }
                }

                StringBuilder stringBuilder = new StringBuilder();
                if (arr[0] != 0)
                    stringBuilder.Append(arr[0]);
                for (int x = 1; x < arr.Length; x++)
                {
                    if(arr[x] != 0)
                        stringBuilder.Append("," + arr[x]);
                }
                await writeToFile(stringBuilder.ToString());
            }            
        }

        public async Task<bool> isFavorite(int id)
        {
            string contents = await readFromFile();

            string[] tokens = contents.Split(',');            
            for (int x = 0; x < tokens.Length; x++)
            {
                int local;
                Int32.TryParse(tokens[x], out local);
                if(local == id)
                {
                    return true;
                }
            }
            return false;
        }

        private void printError(Exception ex)
        {

        }
    }
}
