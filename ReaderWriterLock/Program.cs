
// Reader and Writer lock allows to access the cache simultaniasly 
public class GlobalConfigurationCache
{
    // new instance of the lock
    private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

    private Dictionary<int, string> _cache = new Dictionary<int, string>();

    // Writer function
    public void Add(int key, string value)
    {
        bool lockAcquired = false;
        try
        {
            _lock.EnterWriteLock();
            lockAcquired = true;
            _cache[key] = value;

        }
        finally
        {
            if (lockAcquired) _lock.ExitWriteLock();

        }

    }
    // Reader function
    public string Get(int key)
    {
        bool lockAcquired = false;
        try
        {
            _lock.EnterReadLock();
            lockAcquired = true;
            return _cache.TryGetValue(key, out var value) ? value : null;
        }
        finally
        {
            if(lockAcquired) _lock.ExitReadLock();
        }
        
    }
}
