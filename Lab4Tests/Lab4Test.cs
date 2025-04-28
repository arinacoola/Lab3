using System.Threading;

namespace Lab4Tests

{
    public class ProgramTests
    {
        private Mutex interprocessMutex;
        private Mutex newinterferMutex;


        [SetUp]
        public void Setup()
        {
            interprocessMutex = new Mutex(false, "Global\\Lab4Mutex");
            newinterferMutex = new Mutex(false, "Global\\Lab4Mutex");
        }

        [TearDown]
        public void TearDown()
        {
            interprocessMutex.Dispose();
            newinterferMutex.Dispose();
        }

        
        [Test]
        public void CheckMutexProperlyGrippedAndReleasedTest()
        {
            Assert.That(interprocessMutex.WaitOne(100), Is.True, "Mutex should be acquired");
            interprocessMutex.ReleaseMutex();
            Assert.That(interprocessMutex.WaitOne(100), Is.True, "Mutex should be acquired again after release");
            interprocessMutex.ReleaseMutex();
        }

        [Test]
        public void MutexLockIsWorkingTest()
        {
            Assert.That(interprocessMutex.WaitOne(100), Is.True, "First instance should be able to acquire the mutex");

            bool secondInstanceAcquired = false;
            var thread = new Thread(() => { secondInstanceAcquired = newinterferMutex.WaitOne(100); });
            thread.Start();
            thread.Join();

            Assert.That(secondInstanceAcquired, Is.False, "Second instance should not be able to acquire the mutex");

            interprocessMutex.ReleaseMutex();

            Assert.That(newinterferMutex.WaitOne(100), Is.True,
                "Second instance should be able to acquire the mutex after first instance  releases it");
            newinterferMutex.ReleaseMutex();
        }

        [Test]

        public void TimeoutTest()
        {
            Assert.That(interprocessMutex.WaitOne(100), Is.True, "First instance should be able to acquire the mutex");

            bool secondInstanceAcquired = false;
            var thread = new Thread(() => { secondInstanceAcquired = newinterferMutex.WaitOne(1000); });

            var startTime = DateTime.Now;

            thread.Start();
            thread.Join();

            var endTime = DateTime.Now - startTime;

            Assert.That(secondInstanceAcquired, Is.False, "Second instance should not acquire the mutex");

            Assert.That(endTime.TotalMilliseconds, Is.EqualTo(1000), "Timeout should be respected");
            interprocessMutex.ReleaseMutex();

            Assert.That(newinterferMutex.WaitOne(100), Is.True,
                "Second instance should acquire the mutex after first instance releases it");
            newinterferMutex.ReleaseMutex();
        }
        
        [Test]
        public void DisplayMessageTest()
        {
            bool keyPressed = false;

            var thread = new Thread(() =>
            {
                var input = new StreamReader(" ");
                var output  = new StringWriter();
                
                Console.SetIn(input);
                Console.SetOut(output);
                
                Console.WriteLine("Press any key to exit");
                Console.ReadKey(); 
                keyPressed = true;
                
                Assert.That(output.ToString().Contains("Press any key to exit"), "Expected message not found");

            });
            
            thread.Start();
            thread.Join();
            
            Assert.That(keyPressed, Is.True, "Keypress not detected");

        }

        [Test]
        public void ProgramShouldWaitForKeyPressBeforeExiting()
        {
            bool keyPressed = false;

            var thread = new Thread(() =>
            {
                var input = new StreamReader(" ");
                var output = new StringWriter();

                Console.SetIn(input);
                Console.SetOut(output);

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                keyPressed = true;

                var concoleOutput = output.ToString();
                Assert.That(output.ToString().Contains("Press any key to exit"), "Expected message not found");
            });
            
            thread.Start();
            thread.Join();
            
            Assert.That(keyPressed, Is.True, "Program should wait for a key press before exiting");

        }

        [Test]
        public void ProgramShouldExitAfterKeyPressTest()
        {
            bool  exited = false;

            var thread = new Thread(() => 
            { 
                var input = new StreamReader("\n");
                var output = new StringWriter();

                Console.SetIn(input);
                Console.SetOut(output);

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                exited = true;
            });

            thread.Start();
            thread.Join();
            
            Assert.That(exited, Is.True, "Program should exit after key press");
        }

      [Test]
        public void DisplayMessageWhenAnotherInstanceIsRunning()
        {
            var interprocessMutex = new Mutex(false, "Global\\Lab4Mutex");
            Assert.That(interprocessMutex.WaitOne(100), Is.True, "First instance should acquire mutex");
            
            bool messageDisplayed = false;

            var thread = new Thread(() =>
            {
                var input = new StreamReader(" ");
                var output = new StringWriter();

                Console.SetIn(input);
                Console.SetOut(output);

                var newinterferMutex = new Mutex(false, "Global\\Lab4Mutex");

                if (!newinterferMutex.WaitOne(100))
                {
                    Console.WriteLine("Another instance is running");
                    Console.WriteLine("Press any key to exit");
                    Console.ReadKey();
                }

                var concoleOutput = output.ToString();
                Assert.That(concoleOutput.Contains("Press any key to exit"));
            });
            
            thread.Start();
            thread.Join();
            
            interprocessMutex.ReleaseMutex();
            
        } 

    }
}
