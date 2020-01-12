using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
            Task<string> mTask = null;
            try
            {
                mTask = getJsonAsync();
            }
            catch (Exception e) {
                mTask = loadCacheAsync();
            }
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
            if (mResult == null) {
                throw new Exception("No data fetched!");
            }
            return mResult;
        }

        private async Task<string> loadCacheAsync() {
            var path = @"D:\\Workspace\\МЕИ\\Паралелно програмиране\\kursov_proekt\\WpfApp2\\cache.txt";
            var reader = File.OpenText(path);
            await Task.Delay(300);
            var result = await reader.ReadToEndAsync();
            return result;
        }

        private List<models.Employee> decodeData(string json)
        {
            var jobject = JsonConvert.DeserializeObject<JObject>(json);
            if (jobject.ContainsKey("data")) {
                var data = jobject.GetValue("data").ToString();
                return JsonConvert.DeserializeObject<List<models.Employee>>(data);
            }
            return JsonConvert.DeserializeObject<List<models.Employee>>(json);
        }

      
    }
}
