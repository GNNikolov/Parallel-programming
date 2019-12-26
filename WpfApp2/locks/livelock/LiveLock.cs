using System;
using System.Threading.Tasks;
using WpfApp2.models;

namespace WpfApp2.livelock
{
    class LiveLock : locks.LockProvider
    {
        private string resourse = "string has to be refactored";
        private readonly Employee[] employees = new Employee[2];
        private static readonly int TASK_SLEEP_DELAY = 300;

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

        public override async Task showLock(bool enableLock)
        {
            Task<string> firstTask = runTask(employees[0], employees[1], enableLock);
            Task<string> secondTask = runTask(employees[1], employees[0], enableLock);
            var result = await Task.WhenAll(new Task<string>[] { firstTask, secondTask });
            mWindow.infoLabel.Text = result[0] + "\n" + result[1];
        }

        private Task<string> runTask(Employee first, Employee second, bool enableLock) {

            return Task.Run(async () =>
            {
                if (!enableLock && first.is_waiting && second.is_waiting) {
                    first.is_waiting = false;
                }
                while (first.is_waiting)
                {
                    await Task.Delay(TASK_SLEEP_DELAY);
                    runOnUiThread(first);
                }
                second.is_waiting = false;
                return second.employee_name + " has passed the corridor";
            });
        }

        //Use this method to update ui from background thread
        private void runOnUiThread(Employee emloyee) {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                mWindow.infoLabel.Text = emloyee.employee_name + " is waiting...";
            });
        }

    }
}
