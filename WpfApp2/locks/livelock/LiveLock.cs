using System;
using System.Threading.Tasks;
using WpfApp2.models;

namespace WpfApp2.livelock
{
    class LiveLock : locks.LockProvider
    {
        private readonly Employee[] employees = new Employee[2];
        private static readonly int TASK_SLEEP_DELAY = 1250;

        public LiveLock(MainWindow mainWindow) : base(mainWindow)
        {
            for (int j = 0; j < employees.Length; j++)
            {
                employees[j] = new Employee()
                {
                    employee_name = "Employee" + j.ToString(),
                    employee_age = new Random().Next(30, 40).ToString(),
                    is_waiting = true
                };
            }
        }

        /* Use this method to see how livelock works
         * use @param enableLock to enable/disable livelock
         * .The method will show an example where two employees
         * are waiting to pass a corridor, but every one of them
         * is too polite and wants to let the other pass
         * so the result is livelock: both employees are prompting 
         * each other to pass, but no one is doing, t.e.
         * they are in active state, but no one is doing a usefull work...
         */
        public override async Task showLock(bool enableLock)
        {
            await base.showLock(enableLock);
            Task<string> firstTask = runTask(employees[0], employees[1], enableLock);
            Task<string> secondTask = runTask(employees[1], employees[0], enableLock);
            var result = await Task.WhenAll(new Task<string>[] { firstTask, secondTask });
            mWindow.items.Items.Add(result[0]);
            mWindow.items.Items.Add(result[1]);
        }

        private Task<string> runTask(Employee first, Employee second, bool enableLock)
        {

            return Task.Run(async () =>
            {
                runOnUiThread(first.employee_name + " is staying.");
                await Task.Delay(TASK_SLEEP_DELAY/2);
                if (!enableLock && first.is_waiting && second.is_waiting)
                {
                    first.is_waiting = false;
                }
                while (first.is_waiting)
                {
                    runOnUiThread(first.employee_name + " is waiting for other to pass...");
                    await Task.Delay(TASK_SLEEP_DELAY * 2);
                }
                //First is passing
                runOnUiThread(first.employee_name + " is passing...");
                await Task.Delay(TASK_SLEEP_DELAY / 2);
                runOnUiThread(first.employee_name + " has passed the corridor");
                second.is_waiting = false;
                return "Task finished!";
            });
        }

        //Use this method to update ui from background thread
        private void runOnUiThread(string message)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                mWindow.items.Items.Add(message);
            });
        }

    }
}
