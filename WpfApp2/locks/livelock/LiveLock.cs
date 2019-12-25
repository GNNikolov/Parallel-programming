using System;
using System.Threading;
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
                    employee_name = "Employee" + j.ToString(),
                    employee_age = new Random().Next(30, 40).ToString(),
                    is_active = true
                };
            }
        }

        public override async Task showLock(bool enableLock)
        {
            Task<string> firstTask = Task.Run(async () =>
            {
                while (employees[0].is_active)
                {
                    await Task.Delay(500);
                    //Post message on UI-thread
                    System.Windows.Application.Current.Dispatcher.Invoke(delegate
                    {
                        mWindow.infoLabel.Text = employees[0].employee_name + "\t is working";
                    });
                }
                employees[1].is_active = false;
                return employees[1].employee_name + " has completed the task";
            });

            Task<string> secondTask = Task.Run(async () =>
            {
                while (employees[1].is_active)
                {
                    await Task.Delay(500);
                    //Post message on UI-thread
                    System.Windows.Application.Current.Dispatcher.Invoke(delegate
                    {
                        mWindow.infoLabel.Text = employees[1].employee_name + "\t is working";
                    });
                }
                employees[0].is_active = false;
                return employees[0].employee_name + " has completed the task";
            });
            var result = await Task.WhenAll(new Task<string>[] { firstTask, secondTask });
            mWindow.infoLabel.Text = result[0];
        }

    }
}
