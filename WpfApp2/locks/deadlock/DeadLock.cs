using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WpfApp2.deadlock
{
    class DeadLock : locks.LockProvider
    {
        private readonly static string URL = "http://dummy.restapiexample.com/api/v1/employees";

        public DeadLock(MainWindow mWindow) : base(mWindow) { }

       
        override public async Task showLock(bool enableLock)
        {
            await base.showLock(enableLock);
            mWindow.items.Items.Add("Started...");
            Task<string> mTask = getJsonAsync();
            if (!enableLock)
            {
                await mTask;
            }
            var response = mTask.Result;
            var employees = decodeData(response);
            foreach (models.Employee data in employees) {
                mWindow.items.Items.Add(data.employee_name);
            }
        }
        private async Task<string> getJsonAsync()
        {
            var httpClient = new HttpClient();
            var mResult = await httpClient.GetStringAsync(URL);
          
            return mResult;
        }

        private List<models.Employee> decodeData(string json)
        {
            return JsonConvert.DeserializeObject<List<models.Employee>>(json);
        }
    }
}
