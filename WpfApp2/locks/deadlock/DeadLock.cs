using System.Threading.Tasks;

namespace WpfApp2.deadlock
{
    class DeadLock : locks.LockProvider
    {

        public DeadLock(MainWindow mWindow) : base(mWindow) { }

        /* Use this method to see how deadlock works
         * if @enableDeadLock is set to true the result from the background task
         * is being accessed before the task has completed, so the UI-thread will be blocked 
         * ,waits for the result. Meanwhile the getJsonAsync() attemps to return the value on the UI-thread.
         * The problem is that this thread is currently paused by the mTask.Result call.
         * And therefore we have a classic deadlock situation: A thread blocked waiting for a result(the critical resourse)
         * and a result that cannot be set because it is trying to return it’s value on the blocked thread.
         * If @enableLock is set to false deadlock will be avoided. Code will run async, the getJsonAsync()
         * task will first return a result on the UI-thread, than the result will be proccessed on it.
         */
        override public async Task showLock(bool enableLock)
        {
            mWindow.infoLabel.Text = "Started...";
            var mTask = getJsonAsync();
            if (!enableLock)
            {
                await mTask;
            }
            var response = mTask.Result;
            var employees = decodeData(response);

            mWindow.infoLabel.Text = response;
        }
    }
}
