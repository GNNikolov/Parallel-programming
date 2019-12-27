using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WpfApp2.locks
{
    class LockProvider
    {
        protected readonly static string URL = "http://dummy.restapiexample.com/api/v1/employees";
        protected readonly MainWindow mWindow;

        protected LockProvider(MainWindow mWindow)
        {
            this.mWindow = mWindow;
        }

        public async virtual Task showLock(bool enableLock)
        {
            mWindow.items.Items.Clear();
        }

        protected async Task<string> getJsonAsync()
        {
            var httpClient = new HttpClient();
            var mResult = await httpClient.GetStringAsync(URL);
            return mResult;
        }

        protected List<models.Employee> decodeData(string json)
        {
            return JsonConvert.DeserializeObject<List<models.Employee>>(json);
        }

    }
}
