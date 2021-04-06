using System.IO.MemoryMappedFiles;
using System.Threading;

public class SharedMemoryAccess
{
    private string mutexName; 
    private string mmfName; 
    
    private static Mutex mutex;
    private MemoryMappedFile mmf;
    public MemoryMappedViewAccessor accessor;

    public SharedMemoryAccess(string mutexName, string mmfName)
    {
        mutex = new Mutex(false, mutexName);
        mmf = MemoryMappedFile.OpenExisting(mmfName);
        accessor = mmf.CreateViewAccessor();
    }

    public void Destroy()
    {
        mutex.Dispose();
        mmf.Dispose();
        accessor.Dispose();
    }

    public bool Lock()
    {
        return mutex.WaitOne();
    }

    public void Unlock()
    {
        mutex.ReleaseMutex();
    }
}