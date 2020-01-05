using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.locks
{
    class LockFactory
    {
        public static LockProvider getLock(MainWindow mainWindow, models.Lock input)
        {
            switch (input)
            {
                case models.Lock.LIVELOCK:
                    return new livelock.LiveLock(mainWindow);

                case models.Lock.DEADLOCK:
                    return new deadlock.DeadLock(mainWindow);

            }
            return null;

        }
    }
}
