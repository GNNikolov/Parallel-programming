using System.Threading.Tasks;

namespace WpfApp2.livelock
{
    class LiveLock : locks.LockProvider
    {
        public LiveLock(MainWindow mainWindow) : base(mainWindow){}
        private Task<string> mTask;

        /* Use this method to see how livelock works
         * if @enableLock is set to true livelock will occure, 
         * othwerwise it will run normally
         */
        public override async Task showLock(bool enableLock)
        {
            if (enableLock)
            {
                mTask = getJsonAsync();
                mTask.Wait();
            }
            else {
                mTask = getJsonAsync();
                await mTask;
            }
            var response = mTask.Result;
            var employees = decodeData(response);

            mWindow.infoLabel.Text = response;

        }

    }
}
