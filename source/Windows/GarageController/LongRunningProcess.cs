using System;
using System.Threading;

namespace GarageController
{
    public abstract class LongRunningProcess
    {
        private readonly ILog _log;
        private volatile bool _running;
        private bool _isInitialized;
        private Thread _workerThread;

        protected LongRunningProcess(ILog log)
        {
            _log = log;
            _isInitialized = false;
        }

        protected abstract void Initialize();
        protected abstract void Run();

        public void Start()
        {
            if (!_isInitialized)
            {
                try
                {
                    Initialize();
                    _isInitialized = true;
                }
                catch (Exception ex)
                {
                    _log.Error("Error initializing process", ex);
                }
                
            }
            _running = true;
            _workerThread = new Thread(Run);
            _workerThread.Start();
        }

        public void Stop()
        {
            _running = false;
            Thread.Sleep(2000);
            try
            {
                _workerThread.Abort();
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch (Exception) { } //Do nothing if exception during cleanup
        }

        public bool IsRunning
        {
            get { return _running; }
        }

        protected ILog Log
        {
            get { return _log; }
        }
    }
}
