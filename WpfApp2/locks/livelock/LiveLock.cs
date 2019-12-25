using System;
using System.Threading.Tasks;
using WpfApp2.models;

namespace WpfApp2.livelock
{
    class LiveLock : locks.LockProvider
    {
        private string resourse = "string has to be refactored";
        private readonly Employee[] employees = new Employee[2];

        public LiveLock(MainWindow mainWindow) : base(mainWindow)
        {
            for (int j = 0; j < employees.Length; j++)
            {
                employees[j] = new Employee()
                {
                    employee_name = string.Format("Employee%d", j),
                    employee_age = new Random().Next(30, 40).ToString(),
                    is_active = true
                };
            }
        }

        public override async Task showLock(bool enableLock)
        {
            var firstTask = doWork(employees[0], employees[1]);
            var secondTask = doWork(employees[1], employees[0]);
            var result = await Task.WhenAll(new Task<string>[] { firstTask, secondTask });
            mWindow.infoLabel.Text = result[0];    
        }

        private async Task<string> doWork(Employee first, Employee second)
        {
            string result = null;
            await Task.Run(() =>
            {
                result = processWork(first, second);
            });
            return result;
        }

        private string processWork(Employee first, Employee second)
        {
            while (first.is_active)
            {
                /* Livelock loop will
                 * happen here. Second is waiting
                 * for the first to finish it`s execution.
                 */
            }
            second.is_active = false;
            return resourse.ToUpper();
        }
    }
}
