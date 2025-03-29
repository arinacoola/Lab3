using Microsoft.VisualStudio.TestPlatform.TestHost;
using Lab3.RefactoringGuru.DesignPatterns.Prototype.Conceptual;
using Program = Lab3.RefactoringGuru.DesignPatterns.Prototype.Conceptual.Program;


namespace Lab3.RefactoringGuru.DesignPatterns.Prototype.ConceptualТest
{
    public class PersonTests
    {
        private Person p1;

        [SetUp]
        public void Setup()
        {
            p1 = new Person
            {
                Age = 107,
                BirthDate = new DateTime(1918, 6, 1),
                Name = "Tyler Durden",
                IdInfo = new IdInfo(000)
            };
        }

        [Test]
        public void ShallowCopyTest()
        {
            Person p2 = p1.ShallowCopy();
            Assert.That(p2, Is.Not.SameAs(p1));

            Assert.That(p2.Age, Is.EqualTo(p1.Age));
            Assert.That(p2.BirthDate, Is.EqualTo(p1.BirthDate));
            Assert.That(p2.Name, Is.EqualTo(p1.Name));

            Assert.That(p2.IdInfo, Is.SameAs(p1.IdInfo));

        }

        [Test]
        public void ShallowCopyInChangesInOriginalTest()
        {
            Person p2 = p1.ShallowCopy();

            Assert.That(p2.Age, Is.EqualTo(107));
            Assert.That(p2.BirthDate, Is.EqualTo(new DateTime(1918, 6, 1)));
            Assert.That(p2.Name, Is.EqualTo("Tyler Durden"));

            p1.IdInfo.IdNumber = 101;

            Assert.That(p2.IdInfo.IdNumber, Is.EqualTo(101));
        }

        [Test]
        public void DeepCopyTest()
        {
            Person p3 = p1.DeepCopy();
            Assert.That(p3, Is.Not.SameAs(p1));

            Assert.That(p3.Age, Is.EqualTo(p1.Age));
            Assert.That(p3.BirthDate, Is.EqualTo(p1.BirthDate));
            Assert.That(p3.Name, Is.EqualTo(p1.Name));

            Assert.That(p3.IdInfo, Is.Not.SameAs(p1.IdInfo));

        }

        [Test]
        public void DeepCopyChangesInOriginalTest()
        {
            Person p3 = p1.DeepCopy();

            p1.Age = 1;
            p1.BirthDate = new DateTime(2024, 10, 10);
            p1.Name = "Aboba";
            p1.IdInfo.IdNumber = 300;

            Assert.That(p3.Age, Is.EqualTo(107));
            Assert.That(p3.BirthDate, Is.EqualTo(new DateTime(1918, 6, 1)));
            Assert.That(p3.Name, Is.EqualTo("Tyler Durden"));
            Assert.That(p3.IdInfo.IdNumber, Is.EqualTo(000));
        }

    }

    public class IdInfoTest
    {
        private IdInfo id;

        [SetUp]
        public void Setup()
        {
            id = new(112210101);

        }

        [Test]
        public void CorectIdTest()
        {
            Assert.That(id.IdNumber, Is.EqualTo(112210101));
        }
        //доробить мб
    }
    

        
    public class ProgramTests
    {
        [SetUp]
        public void Setup()
        {
                
        }

        [Test]
        public void CorrectMain()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
                
            Program.Main(new string[0]);
                
            string output = stringWriter.ToString();

                
            Assert.That(output, Does.Contain("Original values of p1, p2, p3:"));
            Assert.That(output, Does.Contain("   p1 instance values:"));
            Assert.That(output, Does.Match(@"Name: Jack Daniels, Age: 42, BirthDate: \d{2}[\./]\d{2}[\./]\d{2}"));

            Assert.That(output, Does.Contain("ID#: 666"));
            Assert.That(output, Does.Contain("   p2 instance values:"));
            Assert.That(output, Does.Contain("   p3 instance values:"));

            Assert.That(output, Does.Contain("Values of p1, p2 and p3 after changes to p1:"));
            Assert.That(output, Does.Contain("   p1 instance values:"));
            Assert.That(output, Does.Match(@"Name: Jack Daniels, Age: 42, BirthDate: \d{2}[\./]\d{2}[\./]\d{2}"));
            Assert.That(output, Does.Contain("ID#: 7878"));
            Assert.That(output, Does.Contain("   p2 instance values (reference values have changed):"));
            Assert.That(output, Does.Match(@"Name: Jack Daniels, Age: 42, BirthDate: \d{2}[\./]\d{2}[\./]\d{2}"));
            Assert.That(output, Does.Contain("ID#: 7878"));
            Assert.That(output, Does.Contain("   p3 instance values (everything was kept the same):"));
            Assert.That(output, Does.Match(@"Name: Jack Daniels, Age: 42, BirthDate: \d{2}[\./]\d{2}[\./]\d{2}"));
            Assert.That(output, Does.Contain("ID#: 666"));
        }
        [Test]
        public static void DisplayValuesTest()
        {
            
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var person = new Person
        {
          Age = 42,
          BirthDate = new DateTime(1977, 1, 1),
          Name = "Jack Daniels",
          IdInfo = new IdInfo(666)
        };

        Program.DisplayValues(person); 

        string output = stringWriter.ToString();
        Console.SetOut(Console.Out);
        
        Assert.That(output, Does.Match(@"Name: Jack Daniels, Age: 42, BirthDate: \d{1,2}[/\.]\d{1,2}[/\.]\d{2}"));


        Assert.That(output, Does.Contain("ID#: 666"));
        }

    }

}





    



