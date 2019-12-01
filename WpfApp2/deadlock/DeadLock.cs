using System.Net.Http;
using System.Threading.Tasks;

namespace WpfApp2.deadlock
{
    class DeadLock
    {
        private readonly object lock1 = new object();
        private readonly object lock2 = new object();
        private readonly static string URL = "http://dummy.restapiexample.com/api/v1/employees";
        private MainWindow mWindow;

        public DeadLock(MainWindow mWindow)
        {
            this.mWindow = mWindow;

        }

        /* Use this method to see how deadlock works
         * if @enableDeadLock is set to true the result from the background task
         * is being accessed before the task has completed, so the UI-thread will be blocked 
         * ,waits for the result. Meanwhile the getJsonAsync() attemps to return the value on the UI-thread.
         * The problem is that this thread is currently paused by the mTask.Result call.
         * And therefore we have a classic deadlock situation: A thread blocked waiting for a result
         * and a result that cannot be set because it is trying to return it’s value on the blocked thread.
         * If @enableDeadLock is set to false deadlock will be avoided. Code will run async, the getJsonAsync()
         * task will first return a result on the UI-thread, than the result will be proccessed on it.
         */
        public async Task demonstrateDeadlock(bool enableDeadLock)
        {
            mWindow.infoLabel.Text = "Started...";
            var mTask = getJsonAsync();
            if (!enableDeadLock)
            {
                await mTask;
            }
            var response = mTask.Result;
            mWindow.infoLabel.Text = response;

        }

        private async Task<string> getJsonAsync()
        {
            var httpClient = new HttpClient();
            var mResult = await httpClient.GetStringAsync(URL);
            return mResult;
        }


    }
}
