using System.Threading;

namespace Lab4Tests

{
    internal class Program
    {
        private Mutex interprocessMutex;

        [SetUp]
        public void Setup()
        {
            interprocessMutex = new Mutex(false, "Global\\Lab4Mutex");

        }

        [Test]
        public void WeTookMutex()
        {
            bool mutexCaptured =  interprocessMutex.WaitOne();
            Assert.That(mutexCaptured , Is.True,"The  thread could not capture the loose mutex");
            interprocessMutex.ReleaseMutex();
            
        }
        
        [Test]
        public void MutexIsBusyAndWeCouldntTakeIt()
        {
            interprocessMutex.WaitOne();
            
            bool mutexNotCaptured = false;

            var thread = new Thread(() =>
            {
                mutexNotCaptured = interprocessMutex.WaitOne(100);
            });
            
            thread.Start();
            thread.Join();
            Assert.That(mutexNotCaptured, Is.False, "Another thread failed to capture the busy mutex");
            interprocessMutex.ReleaseMutex();
            


        }

        [TearDown]
        public void Teardown()
        {
            if (interprocessMutex != null && interprocessMutex.WaitOne(0))
            {
                interprocessMutex.ReleaseMutex();
            }

            interprocessMutex?.Dispose();

        }

    }
}
        
        