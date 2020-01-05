using System.Threading.Tasks;

namespace WpfApp2.locks
{
    class LockProvider
    {
        protected readonly MainWindow mWindow;

        protected LockProvider(MainWindow mWindow)
        {
            this.mWindow = mWindow;
        }

        public async virtual Task showLock(bool enableLock)
        {
            mWindow.items.Items.Clear();
        }

    }
}
