using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Score = CSharpLearning.Program.Tuple<string, int>;
using System.Threading;
using System.Reflection;
using System.Net.Http;
using System.Data;
using System.Globalization;

namespace CSharpLearning
{
    public interface IFoo
    {
        int Marker { get; set; }
    }

    public static class FooExtentions
    {
        public static void NextMarker(this IFoo thing) => thing.Marker += 1;
    }

    class Program
    {
        public class MyType : IFoo
        {
            public int Marker { get; set; }
        }

        public class CelestialBody :
            IComparable<CelestialBody>
        {
            public double Mass { get; set; }
            public string Name { get; set; }

            public int CompareTo(CelestialBody other)
            {
                throw new NotImplementedException();
            }
        }

        public class Planet : CelestialBody
        {
        }

        public class Moon : CelestialBody
        {
        }

        public class Asteroid : CelestialBody
        {
        }

        public class Racer
        {
            public string country { get; set; }
            public string name { get; set; }
            public int age { get; set; }
            public bool sex { get; set; }
        }

        public static class Example
        {
            public static T Add<T>(T left, T right, Func<T, T, T> AddFunc) => AddFunc(left, right);
        }

        public class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public Person() { }
            public Person(string f, string l) { FirstName = f; LastName = l; }
        }

        public class Point
        {
            public double X { get; }
            public double Y { get; }
            public Point(double x, double y)

            {

                this.X = x;

                this.Y = y;

            }
        }


        public class Employee_ClassA : IComparable
        {
            public Employee_ClassA() { Name = "hello from Class A"; }
            public Employee_ClassA(string Name) { this.Name = Name; }
            public string Name { get; set; }
            public int CompareTo(object obj)
            {
                if (!(obj is Employee_ClassA))
                    throw new ArgumentException("Argument is not Employee");
                Employee_ClassA e = (Employee_ClassA)obj;
                return Name.CompareTo(e.Name);
            }
        }
        public class Employee_ClassB : IComparable<Employee_ClassB>
        {
            public Employee_ClassB() { Name = "hello from Class B"; }
            public Employee_ClassB(string Name) { this.Name = Name; }
            public string Name { get; set; }
            public int CompareTo(Employee_ClassB obj)
            {
                return Name.CompareTo(obj.Name);
            }
        }
        public class EmployeeComparer : IComparer<Employee_ClassA>, IComparer<Employee_ClassB>
        {
            public int Compare(Employee_ClassA x, Employee_ClassA y)
            {
                return x.CompareTo(y);
            }
            public int Compare(Employee_ClassB x, Employee_ClassB y)
            {
                return x.CompareTo(y);
            }
        }

        public static class Utils
        {
            public static T Max<T>(T left, T right)
            {
                return Comparer<T>.Default.Compare(left, right) < 0 ?
                    right : left;
            }
            public static T Min<T>(T left, T right)
            {
                return Comparer<T>.Default.Compare(left, right) < 0 ?
                    left : right;
            }
            public static double Max(double left, double right)
            {
                return Math.Max(left, right);
            }
            public static double Min(double left, double right)
            {
                return Math.Min(left, right);
            }
            public static int Max(int left, int right)
            {
                return Math.Max(left, right);
            }
            public static int Min(int left, int right)
            {
                return Math.Min(left, right);
            }
        }


        public class MyBase { }
        public interface IMessageWriter
        {
            void WriteMessage();
        }
        public class MyDerived : MyBase, IMessageWriter
        {
            void IMessageWriter.WriteMessage() { Console.WriteLine("Inside MyDerived.WriteMessage"); }
        }
        public class MyOther : IMessageWriter
        {
            void IMessageWriter.WriteMessage() { Console.WriteLine("Inside MyOther.WriteMessage"); }
        }

        [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
        public class Author : Attribute
        {
            private string name;
            private string team;

            public Author()
            {
                name = default(string);
                team = default(string);
            }
            public string Name { get { return name; } set { name = value; } }
            public string Team { get { return team; } set { team = value; } }
        }

        [Author(Name = "Tom", Team = "WDO")]
        public class TestAttribute
        {
            [Author(Name = "Jack", Team = "WDO")]
            public TestAttribute() { }
        }


        class EagerSingleton
        {
            private static EagerSingleton instance = new EagerSingleton();

            private EagerSingleton() { }

            public static EagerSingleton GetInstance()
            {
                return instance;
            }
        }

        class LazySingleton
        {
            private LazySingleton() { }

            private static LazySingleton instance = null;

            public static LazySingleton GetInstance()
            {
                if (instance == null)
                    instance = new LazySingleton();
                return instance;
            }
        }

        public static void CoVariantArray(CelestialBody[] baseItems)
        {
            foreach (var thing in baseItems)
                Console.WriteLine("CoVariantArray: {0} has a mass of {1} Kg", thing.Name, thing.Mass);
        }

        public static void UnsafeCoVariantArray(CelestialBody[] baseItems)
        {
            baseItems[0] = new Asteroid { Name = "Asteroid", Mass = 3234.09 };
        }

        public static void CovariantGeneric(IEnumerable<CelestialBody> baseItems)
        {
            Console.WriteLine("the type of baseItems is {0}", baseItems.GetType().ToString());
            foreach (var thing in baseItems)
                Console.WriteLine("CovariantGeneric: {0} has a mass of {1} Kg", thing.Name, thing.Mass);
        }

        public static void ContravariantGeneric(Action<Planet> a)
        {
            Console.WriteLine("the type of a is {0}", a.GetType().ToString());
        }

        public static IEnumerable<TOutput> Zip<T1, T2, TOutput>(IEnumerable<T1> left, IEnumerable<T2> right, Func<T1, T2, TOutput> generator)
        {
            IEnumerator<T1> leftSequence = left.GetEnumerator();
            IEnumerator<T2> rightSequence = right.GetEnumerator();
            while (leftSequence.MoveNext() && rightSequence.MoveNext())
            {
                yield return generator(leftSequence.Current, rightSequence.Current);
            }
            leftSequence.Dispose();
            rightSequence.Dispose();
        }

        public static IEnumerable<char> GenerateAlphabet()
        {
            var letter = 'a';
            while (letter <= 'z')
            {
                yield return letter;
                letter++;
            }
        }

        public static IEnumerable<char> GenerateAlphabetSubset(char first, char last)
        {
            if (first < 'a') throw new ArgumentException("first must >= charactor a");
            if (first > 'z') throw new ArgumentException("first must <= charactor z");
            if (first > last) throw new ArgumentException("first <= last");
            if (last > 'z') throw new ArgumentException("last must <= charactor z");

            var letter = first;
            while (letter <= last)
            {
                yield return letter;
                letter++;
            }
        }

        public static IEnumerable<T> GenerateSequence<T>(int numOfElems, Func<T> generator)
        {
            for (int i = 0; i < numOfElems; i++)
            {
                yield return generator();
            }
        }

        public static String MakeWebRequest()
        {
            // Make Web Request, simulate timeout exception...
            throw new TimeoutException();
        }

        public static bool AreEqual<T>(T left, T right)
            where T : class, IComparable<T>
        {
            return left.CompareTo(right) == 0;
        }
        public static bool AreEqual2<T>(T left, T right)
            where T : struct
        {
            return false;
        }

        public static void WriteMessage(MyBase b) { Console.WriteLine("Inside WriteMessage(MyBase)"); }
        public static void WriteMessage<T>(T obj)
        {
            Console.Write("Inside WriteMessage<T>(T):  ");
            Console.WriteLine(obj.ToString());
        }
        public static void WriteMessage(IMessageWriter obj)
        {
            Console.Write(
                "Inside WriteMessage(IMessageWriter):  ");
            obj.WriteMessage();
        }

        public struct Tuple<T1, T2>
        {
            public T1 First { get; set; }
            public T2 Second { get; set; }
            public Tuple(T1 f, T2 s)
            {
                First = f;
                Second = s;
            }
        }

        public static Score FindScore()
        {
            string name = "Tom";
            int score = 10000;
            return new Score(name, score);
        }

        public static class Hero
        {
            private const double TOLERANCE = 1.0E-8;
            public static double FindRoot(double number)
            {
                double guess = 1;
                double error = Math.Abs(guess * guess - number);

                while (error > TOLERANCE)
                {
                    guess = (number / guess + guess) / 2.0;
                    error = Math.Abs(guess * guess - number);
                }
                return guess;
            }
        }

        public const int TIMES = 1000000;
        public const int NUM_THREADS = 2;
        public static void f1()
        {
            Stopwatch sw = new Stopwatch();
            double d12;
            sw.Start();
            for (int i = 0; i < TIMES; i++) d12 = Hero.FindRoot(i);
            sw.Stop();
            Console.WriteLine("total f1() run time is {0}ms", sw.ElapsedMilliseconds);
        }
        public static void f2()
        {
            Stopwatch sw = new Stopwatch();
            using (AutoResetEvent e = new AutoResetEvent(false))
            {
                int numThreads = NUM_THREADS;
                int workerThreads = numThreads;

                sw.Start();
                for (int k = 0; k < numThreads; k++)
                {
                    ThreadPool.QueueUserWorkItem(
                        (x) =>
                        {
                            for (int i = 0; i < TIMES; i++)
                            {
                                // Call the calculation.
                                if (i % numThreads == k)
                                {
                                    double d = Hero.FindRoot(i);
                                }

                            }

                            // Decrement the count.
                            if (Interlocked.Decrement(ref workerThreads) == 0)
                            {
                                // Set the event.
                                e.Set();
                            }
                        });
                }

                e.WaitOne();
                sw.Stop();
                Console.WriteLine("total f2() run time with {0} thread is {1}ms", numThreads, sw.ElapsedMilliseconds);
            }
        }
        public static void f3()
        {
            Stopwatch sw = new Stopwatch();
            using (AutoResetEvent e = new AutoResetEvent(false))
            {
                int numThreads = NUM_THREADS;
                int workerThreads = numThreads;

                sw.Start();
                for (int k = 0; k < numThreads; k++)
                {
                    Thread t = new Thread(
                        () =>
                        {
                            for (int i = 0; i < TIMES; i++)
                            {
                                // Call the calculation.
                                if (i % numThreads == k)
                                {
                                    double d12 = Hero.FindRoot(i);
                                }

                            }

                            // Decrement the count.
                            if (Interlocked.Decrement(ref workerThreads) == 0)
                            {
                                // Set the event.
                                e.Set();
                            }
                        });
                    t.Start();
                }

                e.WaitOne();

                sw.Stop();
                Console.WriteLine("total f3() run time with {0} thread is {1}ms", numThreads, sw.ElapsedMilliseconds);
            }
        }

        public class EventTest
        {
            public event EventHandler evHandler;
            public delegate void EventHandler(EventTest obj, EventArgs e);
            private uint timeout = 1;
            public EventTest(uint t) { timeout = t; }
            public void Run()
            {
                uint i = 0;
                while (true)
                {
                    Thread.Sleep(1000);
                    if (evHandler != null)
                    {
                        evHandler(this, null);
                        i++;
                    }
                    if (i == timeout) break;
                }
            }
        }
        public static void Timeout(EventTest obj, EventArgs e)
        {
            Console.WriteLine("1 second elapsed...");
        }

        public static class Helper
        {
            public static void PrintName(Person2 p)
            {
                Console.WriteLine("Name: {0}", p.Name);
            }
            public static void PrintFriend(Person2 p)
            {
                Console.WriteLine("Friend of {0}: {1}", p.Name, p.Friend.ContainsKey(p.Name) ? p.Friend[p.Name] : "Not Exist");
            }
        }
        public class Person2
        {
            public Dictionary<string, string> Friend;
            public enum SEX { MALE, FEMALE };

            public string Name { get; set; }
            public SEX Sex { get; set; }

            public Person2(string Name, SEX Sex)
            {
                this.Name = Name;
                this.Sex = Sex;
                Friend = new Dictionary<string, string>();
            }
            public void ShowName()
            {
                Helper.PrintName(this);
            }
            public void ShowFriend()
            {
                Helper.PrintFriend(this);
            }
            public string this[string key]
            {
                get
                {
                    return Friend[key];
                }
                set
                {
                    if (!Friend.ContainsKey(key))
                        Friend.Add(key, value);
                }
            }
        }

        public delegate void MethodInvoker();

        public class Student
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public int Height { get; set; }
            public int Weight { get; set; }
            public string Sex { get; set; }
            public double Score { get; set; }
        }


        /// <summary>
        ///  在这里声明SelfApplicable是一个delegate，他有2个参数，第一个参数是他自己，第二个参数是
        ///  类型T，返回类型是TResult
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        delegate TResult SelfApplicable<T, TResult>(SelfApplicable<T, TResult> self, T arg);

        /// <summary>
        /// 这个现在还是没弄懂，高深...
        /// 参考下面链接
        /// </summary>
        /// <href>https://blogs.msdn.microsoft.com/madst/2007/05/11/recursive-lambda-expressions/</href>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        static Func<T, TResult> Fix<T, TResult>(Func<Func<T, TResult>, Func<T, TResult>> f)
        {
            return x => f(Fix(f))(x);
        }

        static AutoResetEvent ev = new AutoResetEvent(false);
        static int count = 0;
        static void ProcessingFunc()
        {
            while (true)
            {
                if (ev.WaitOne(20))
                {
                    Console.WriteLine("1 Signal Processed!");
                    if (Interlocked.CompareExchange(ref count, 0, 10) == 10)
                        break;
                }
            }
        }


        static object taskMethodLock = new object();

        static void TaskMethod(object title)
        {
            lock (taskMethodLock)
            {
                Console.WriteLine(title);
                Console.WriteLine(">> Task:{0}", Task.CurrentId == null ? "no task" : Task.CurrentId.ToString());
                Console.WriteLine(">> Thread:{0}", Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine(">> IsPooledThread:{0}", Thread.CurrentThread.IsThreadPoolThread);
                Console.WriteLine(">> IsBackgroundThread:{0}", Thread.CurrentThread.IsBackground);
                Console.WriteLine();
            }
        }

        static void TaskUsingThreadPool()
        {
            var tf = new TaskFactory();
            var t1 = tf.StartNew(TaskMethod, "using a task factory");
            var t2 = Task.Factory.StartNew(TaskMethod, "factory via a task");
            var t3 = new Task(TaskMethod, "using task constructor and start");
            t3.Start();
            var t4 = Task.Run(() => TaskMethod("using the run method"));
        }

        static void TaskUsingSynchronous()
        {
            TaskMethod("in main thread");
            var t1 = new Task(TaskMethod, "run sync");
            t1.RunSynchronously();
        }

        static void TaskLongRunning()
        {
            var t1 = new Task(TaskMethod, "long running", TaskCreationOptions.LongRunning);
            t1.Start();
        }

        static Tuple<int, int> TaskMethodWithResult(object division)
        {
            Tuple<int, int> input = (Tuple<int, int>)division;
            int result = input.First / input.Second;
            int remainder = input.First % input.Second;
            return new Tuple<int, int>(result, remainder);
        }

        static void TaskWithResult()
        {
            var t1 = new Task<Tuple<int, int>>(TaskMethodWithResult, new Tuple<int, int>(8, 3));
            t1.Start();
            Console.WriteLine(t1.Result);
            t1.Wait();
            Console.WriteLine("result from task: {0} {1}", t1.Result.First, t1.Result.Second);
        }

        static void FirstToDo()
        {
            Console.WriteLine("First Job to do.");
        }

        static void SecondToDo(Task t)
        {
            Console.WriteLine("task {0} finished, new task is {1}", t.Id, Task.CurrentId);
            Console.WriteLine("Second Job to do.");
        }

        static void TaskContinueWith()
        {
            var t1 = new Task(FirstToDo);
            var t2 = t1.ContinueWith(SecondToDo);
            t1.Start();
        }


        static void DoIndependentWork() { }

        // Three things to note in the signature:  
        //  - The method has an async modifier.   
        //  - The return type is Task or Task<T>. (See "Return Types" section.)  
        //    Here, it is Task<int> because the return statement returns an integer.  
        //  - The method name ends in "Async."  
        static async Task<int> AccessTheWebAsync()
        {
            // You need to add a reference to System.Net.Http to declare client.  
            HttpClient client = new HttpClient();

            // GetStringAsync returns a Task<string>. That means that when you await the  
            // task you'll get a string (urlContents).  
            Task<string> getStringTask = client.GetStringAsync("http://msdn.microsoft.com");

            // You can do work here that doesn't rely on the string from GetStringAsync.  
            DoIndependentWork();

            // The await operator suspends AccessTheWebAsync.  
            //  - AccessTheWebAsync can't continue until getStringTask is complete.  
            //  - Meanwhile, control returns to the caller of AccessTheWebAsync.  
            //  - Control resumes here when getStringTask is complete.   
            //  - The await operator then retrieves the string result from getStringTask.  
            string urlContents = await getStringTask;

            // The return statement specifies an integer result.  
            // Any methods that are awaiting AccessTheWebAsync retrieve the length value.  
            return urlContents.Length;
        }


        static void Main(string[] args)
        {
            goto test_item;

            #region Learning-1 Co-Variant(协变) and Contra-Variant(抗变/逆变）
            /*
             * LEARNING-1
             * Following code shows how to use Co-Variant(协变) and Contra-Variant(抗变/逆变）
             */
            Planet[] l1 = new Planet[3];
            l1[0] = new Planet { Name = "Name1", Mass = 9.34 };
            l1[1] = new Planet { Name = "Name2", Mass = 9.44 };
            l1[2] = new Planet { Name = "Name3", Mass = 9.64 };
            CoVariantArray(l1);

            // Passing an IEnumerable<Planet> instance to IEnumerable<CelestialBody> is allowed
            // since IEnumerable<T> is defined as Co-Variant.
            CovariantGeneric(l1);

            // Passing an Action<CelestialBody> instance to Action<Planet> is allowed
            // since Action<T> is defined as Contra-Variant.
            Action<CelestialBody> ac = (p) => { };
            ContravariantGeneric(ac);
            #endregion

            #region Learning-2 Use delegate and lamda
            /*
             * LEARNING-2
             * Following code shows how to use delegate and lamda
             */
            int l2_a = 4;
            int l2_b = 2;
            int l2_sum = Example.Add(l2_a, l2_b, (x, y) => x + y);
            Console.WriteLine("sum = {0}", l2_sum);

            double[] xValues = { 1, 2, 3, 4, 5 };
            double[] yValues = { 2, 3, 4, 5, 6 };
            List<Point> values = new List<Point>(Zip(xValues, yValues, (x, y) => new Point(x, y)));
            #endregion

            #region Learning-3 How to use extenstion 
            /*
             * LEARNING-3
             * The following code shows how to use extenstion.
             * 
             * Please Note: Extension methods should be used to 
             * enhance a type with functionality that naturally
             * extends a type.
             */
            IFoo l3_a = new MyType { Marker = 4 };
            l3_a.NextMarker();
            Console.WriteLine("aa.Marker = {0}", l3_a.Marker);
            #endregion

            #region Learning-4 Use yield to generate a sequence
            /*
             * LEARNING-4
             * The following code shows how to use yield to generate a sequence
             */
            var l4_s1 = GenerateAlphabet();
            var l4_s2 = GenerateAlphabetSubset('b', 'w');
            #endregion

            #region Learning-5 Use Query Syntex instead of loops
            /*
             * LEARNING-5
             * The following code shows how to use Query Syntex instead of using Loops
             */
            var l5_a = from i in Enumerable.Range(0, 10)
                       from j in Enumerable.Range(0, 10)
                       where i + j < 10
                       orderby i * i + j * j descending
                       select Tuple.Create(i, j);

            var l5_b = from i in Enumerable.Range(0, 100)
                       where i % 2 == 1
                       select i;
            #endregion

            #region Learning-6 When to use yield and when do not (Composable API for sequence)
            /*
             * LEARNING-6
             * Please Note, using "yield return" the generated Enumerable<T> will only happened when
             * the result has been used in the code. Otherwise it will not run! And each time when the
             * result has been referenced, the generation will happen again. So the following code will
             * print 1,2,3,4,5,6,7,8,9,10, instead of 1,2,3,4,5,1,2,3,4,5,
             * 
             * That is to say, if the generated result is only used once, it is more efficient to use
             * 'yield return' to generate the result; while if the generated result will be used more
             * than once, probably we should use the following code instead.
             *             var c = GenerateSequence<int>(5, () => start += 1).ToList();
             */
            var l6_start = 0;
            var l6_sequence = GenerateSequence<int>(5, () => l6_start += 1);
            foreach (var k in l6_sequence) Console.Write("{0},", k);
            foreach (var k in l6_sequence) Console.Write("{0},", k);
            #endregion

            #region Learning-7 Use GroupBy in LINQ
            /*
             * LEARNING-7
             * How to use GroupBy in LINQ
             */
            List<Racer> l7 = new List<Racer>();
            l7.Add(new Racer { country = "China", name = "LiMing", age = 23, sex = true });
            l7.Add(new Racer { country = "USA", name = "John", age = 29, sex = true });
            l7.Add(new Racer { country = "Japan", name = "Kanda", age = 21, sex = false });
            l7.Add(new Racer { country = "China", name = "WangYi", age = 22, sex = true });
            l7.Add(new Racer { country = "China", name = "ZhaoYan", age = 20, sex = false });

            var l7_result = from l7_e in l7
                            group l7_e by l7_e.country into g
                            orderby g.Count() descending, g.Key
                            select new { country = g.Key, count = g.Count() };

            var l7_result2 = from l7_e in l7
                             group l7_e by l7_e.sex into g
                             orderby g.Count() descending, g.Key
                             select new { sex = g.Key ? "Male" : "Female", count = g.Count() };

            var l7_result3 = from l7_e in l7
                             group l7_e by l7_e.age into g
                             orderby g.Key ascending
                             select new { age = g.Key, count = g.Count() };

            #endregion

            #region Learning-8 Decouple Iterations from Actions, Predicates and Functions
            /*
             * LEARNING-8
             * Decouple the iteration from Actions, Predicates and Functions
             */
            List<int> myInts = new List<int> { 0, 2, 3, 4, 5, 6, 13, 21, 100, 101 };
            myInts.RemoveAll(i => i % 5 == 0);
            Console.WriteLine();
            myInts.ForEach(i => Console.Write("{0} ", i));
            var myVar = myInts.Select(i => (i * i).ToString());
            #endregion

            #region Learning-9 Use Single() to enforce only 1 element is returned
            /*
             * LEARNING-9
             * Single() returns exactly one element. If no elements exist, or if
             * multiple elements exist, then Single() throw an exception. This is 
             * a rather strong statement about your expectation!
             */
            var somePeople = new List<Person>
            {
                new Person { FirstName = "Bill", LastName = "Gates"},
                new Person { FirstName = "Bill", LastName = "Wagner"},
                new Person { FirstName = "Bill", LastName = "Johnson"}
            };

            var answer = from p in somePeople
                         where p.FirstName == "John"
                         select p;
            // The following code throw an exception
            //var answer2 = (from p in somePeople
            //               where p.FirstName == "John"
            //               select p).Single();

            #endregion

            #region Learning-10 Avoid modifying bound variables
            /*
             * LEARNING-10
             * Due to deferred execution, the output of the following code is
             * 20 21 22 23 24 25 26 27 28 29 Done
             * 50 51 52 53 54 55 56 57 58 59 Done
             */

            Console.WriteLine();
            var index = 0;
            Func<IEnumerable<int>> sequence =
               () => GenerateSequence(10, () => index++);

            index = 20;
            foreach (int n in sequence())
                Console.Write("{0} ", n);
            Console.WriteLine("Done");

            index = 50;
            foreach (var n in sequence())
                Console.Write("{0} ", n);
            Console.WriteLine("Done");
            #endregion

            #region Learning-11 When to use Try-Catch-Finally
            /*
             * LEARNING-11
             * 
             * try-catch is not aim at avoid BUG, instead, it is used to handle the case
             * when programmer can predict the type of error yet can not predict when the
             * error will happen. (能够预知到异常发生的类型,但是无法预知到什么时候会出现异常,
             * 所以需要加try,在catch里代码解决能预测的异常类型)
             * Typical usage of Try-Catch is for IO related operation, such as File Read/Write
             * error, Database connection or Network connection timeout etc.
             */
            try
            {
                // Some code
            }
            catch (SocketException e1)
            {
                // Possibly retry a couple of times, pop up error message when
                // failed after several retry.
            }
            catch (FileNotFoundException e2)
            {
                // Possibly pop up message to allow user make sure the file 
                // is exist.
            }
            catch (Exception)
            {
                // re-throw since we do not know how to handle this
                throw;
            }
            finally
            {
                // Release Resource
            }
            #endregion

            #region Learning-12 Use "Using" to ensure resource to be disposed
            /*
             * LEARNING-12
             * 
             * When the lifetime of an IDisposable object is limited to a single 
             * method, you should declare and instantiate it in a using statement.
             * The using statement calls the Dispose method on the object in the
             * correct way, and (when you use it as shown earlier) it also causes
             * the object itself to go out of scope as soon as Dispose is called.
             * Within the using block, the object is read-only and cannot be 
             * modified or reassigned.
             */
            using (TextWriter w = File.CreateText(@"c:\temp\log.txt"))
            {
                w.WriteLine("This is line one");
            }

            // When using statement itself may cause an exception, you may put
            // using statement in a try {} block as well. Please note, since 'w'
            // is enclosed in using block, there is no need to have the finally{}
            // block.
            try
            {
                using (TextWriter w = File.CreateText(@"c:\log.txt"))
                {
                    w.WriteLine("This is line one");
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
            }
            #endregion

            #region Learning-13 Prefer exception filters to Catch and Re-throw
            /*
             * LEARNING-13
             * 
             * You should develop the habit of using exception filters rather than 
             * conditional logic in catch clauses. An exception filter is an expression
             * on your catch clause, following the 'when' keyword, that limits when the
             * catch clause handles an exception of a given type.
             */
            var retryCount = 0;
            var dataString = default(String);

            while (dataString == null)
            {
                try
                {
                    dataString = MakeWebRequest();
                }
                catch (TimeoutException e) when (retryCount++ < 3)
                {
                    Console.WriteLine("Operation timed out {0} time. Trying again", retryCount);

                    // pause before trying again.
                    Task.Delay(1000 * retryCount);
                }
                //catch (TimeoutException e)
                //{
                //    if (retryCount++ < 3)
                //    {
                //        Console.WriteLine("Operation timed out {0} time. Trying again", retryCount);

                //        // pause before trying again.
                //        Task.Delay(1000 * retryCount);
                //    }
                //    else
                //        throw;
                //}
                catch (Exception e)
                {
                    Console.WriteLine("Operation timed out for > 3 times, exit!");
                    break;
                }
            }

            #endregion

            #region Learning-14 Prefer Generaric version to .Net 1.1 version
            /*
             * LEARNING-14
             * 
             * You should use the generic implementation introduced after .Net 2
             * whenever possible; while at the same time to be compatible with .Net
             * 1.1 implementation.
             * 
             * In following example, the Employee_ClassB is prefered to Employee_ClassA
             */
            Employee_ClassA em1 = new Employee_ClassA();
            Object em2 = new Employee_ClassA();
            em1.CompareTo(em2);

            Employee_ClassB em3 = new Employee_ClassB();
            Employee_ClassB em4 = new Employee_ClassB();
            em3.CompareTo(em4);
            #endregion

            #region Learning-15 Use Constraints that are minimal and sufficient
            /*
             * LEARNING-15
             * 
             * Constraints enable the compiler to expect capabilities in a type parameter beyond 
             * those in the public interface defined in System.Object. AreEqual<T> and AreEqual2<T>
             * is an example.
             */
            bool b = AreEqual2<int>(5, 4);
            bool c = AreEqual<string>("hello", "hello");
            #endregion

            #region Learning-16 Generic is a better match than base class
            /*
             * LEARNING-16
             * 
             * WriteMessage<T>(T obj) is a better match than WriteMessage(MyBase b) 
             * for an object that is derived from MyBase. That’s because the compiler 
             * can make an exact match by substituting MyDerived for T in that message, 
             * and WriteMessage(MyBase) requires an implicit conversion.
             */
            Console.WriteLine();
            MyDerived d = new MyDerived();
            MyOther dd = new MyOther();
            WriteMessage(d);
            WriteMessage((IMessageWriter)d);
            WriteMessage((MyBase)d);
            WriteMessage(dd);
            #endregion

            #region Learning-17 Balance between using generic or non-generic
            /*
             * LEARNING-17
             * 
             * Below example use a non-generic definition for class Utils,
             * the reason is 
             *  - it does not require the caller to have a type specified
             *  - it allows some specific method (which is more efficient) 
             *    can be used
             *  
             * *********************************************************************
             * General GuideLine:
             * *********************************************************************
             * In two cases you must make a generic class: 
             *   - The first occurs when your class stores a value of one of 
             *     the Type parameters as part of its internal state. 
             *     (Collections are an obvious example.) 
             *   - The second occurs when your class implements a generic interface. 
             *   
             * Except for those two cases, you can usually create a nongeneric 
             * class and use generic methods.
             * *********************************************************************
             *  
             */
            Console.WriteLine();

            int i1 = 4;
            int i2 = 5;
            int min = Utils.Min(i1, i2);

            double d1 = 4;
            double d2 = 5;
            double max = Utils.Max(d1, d2);

            string foo = "foo";
            string bar = "bar";
            string sMax = Utils.Max(foo, bar);

            double? d3 = 12;
            double? d4 = null;
            double? Max2 = Utils.Max(d3, d4).Value;
            #endregion

            #region Learning-18 Prefer Generic Tuples to Output or Ref Parameters
            /*
             * LEARNING-18
             * 
             * The following code simulate a function to return 2 values. This can
             * be easily extended to N value result.
             * 
             * Guide Line:
             * 1. Methods that take 'out' and 'ref' parameters do not support composition 
             *    very well. Methods returning a single value (however complex that 
             *    value might be) compose better.
             * 
             */
            Score sc = FindScore();
            #endregion

            #region Learning-19 Use Thread Pool instead of creating threads
            /*
             * LEARNING-19
             * 
             * It is recommended to use .Net thread pool instead of creating
             * threads yourself. 'QueueUserWorkItem' uses the thread pool to manage
             * resources for you. When you add an item, it is executed when a 
             * thread is available. 
             * 
             * All threads belonging to the thread pool used by QueueUserWorkItem
             * are background threads. This means that you don’t need to clean up
             * those threads before your application exits. 
             * 
             * In the following function call, f2() is preferred.
             */

            f1(); f2(); f3();
            #endregion

            #region Learning-20 Use Event Handler
            /*
             * LEARNING-20
             * 
             * Events are not fields - they are just add/remove method pairs.
             * In this sense, events are something similar to properties, which
             * provide a pair of methods(getters and setters).
             */
            EventTest et = new EventTest(3);
            et.evHandler += Timeout;
            et.Run();
            et.evHandler -= Timeout;
            #endregion

            #region Learning-21 keyword 'this' Usage in C#
            /*
             * LEARNING-21 
             * 
             * 1. Use this to distinguish parameter from member name
             * 2. Use this to pass the object as parameter
             * 3. Use this to declare indexer
             * 4. Use this in class extension (See Learning-3)
             */
            Person2 p2 = new Person2("Tom", Person2.SEX.MALE);
            p2.ShowName();
            p2["Tom"] = "Jerry";
            p2["Jerry"] = "Tom";
            p2.ShowFriend();
            #endregion

            #region Learning-22 Difference between IComparer<T> and IComparable<T>
            /*
             * LEARNING-22 
             * 
             * IComparer<T> is implemented by types that are capable of comparing 
             * 2 different values, whereas an instance of IComparable<T> is capable
             * of comparing _itself_ with another value.
             * 
             */
            Employee_ClassA ea1 = new Employee_ClassA("1st Instance of ClassA");
            Employee_ClassA ea2 = new Employee_ClassA("2nd Instance of ClassA");
            int ea_result = ea1.CompareTo(ea2);
            EmployeeComparer ec = new EmployeeComparer();
            int ec_result = ec.Compare(ea1, ea2);
            #endregion

            #region Learning-23 Nullable<T> and ?/?? Syntactic Suger
            /*
             * LEARNING-22
             * 
             *    first ?? second
             * a) Evaluate first.
             * b) If the result is non-null, that’s the result of the whole expression.
             * c) Otherwise, evaluate second; the result then becomes the result of the 
             *    whole expression.
             */
            int i10 = 10;
            Nullable<int> nullable_i1;
            int? nullable_i2; // Syntactic suger for Nullable<int>
            nullable_i1 = i10;
            nullable_i2 = null;

            var nullable_i3 = nullable_i1 ?? nullable_i2;

            #endregion

            #region Learning-24 Captured variables in anonymous methods
            /*
             * LEARNING-24
             * 
             * The following code shows an example of the captured varaible
             * in an anonymous method. while variable 'outside' is shared betwwen
             * the 2 delegate instances, each delegate instance has it's own 
             * variable 'inside'. 
             * 
             */
            MethodInvoker[] mi = new MethodInvoker[2];
            int outside = 0;
            for (int i = 0; i < 2; i++)
            {
                int inside = 0;
                mi[i] = delegate
                {
                    Console.WriteLine("inside={0}, outside={1}", inside, outside);
                    inside++;
                    outside++;
                };
            }

            MethodInvoker first = mi[0];
            MethodInvoker second = mi[1];

            first(); first(); first();
            second(); second();
            #endregion

            #region Learning-25 Usage of File, Directory, Path etc.
            /*
             * LEARNING-25
             * 
             * Following sample code shows how to use System.IO.File/Directory/Path
             * The FileSystemHelper Class is leveraged from TAP code.
             */
            string sss = FileSystemHelper.CreateTempFile();
            sss = FileSystemHelper.CreateTempDirectory();
            #endregion

            #region Learning-26 The usage of ArraySegment
            /*
             * LEARNING-26
             * 
             * Please note, ArraySegment<T> reference to the original array and 
             * does not make a copy of it, so the changes of segment will affect
             * the array as well.
             */
            var array = new int[] { 5, 8, 9, 20, 70, 44, 2, 4 };

            array.ToList().ForEach(i => Console.Write("{0} ", i));
            Console.WriteLine();

            var segment = new ArraySegment<int>(array, 2, 3);
            segment.ToList().ForEach(i => Console.Write("{0} ", i)); // output: 9, 20, 70
            Console.WriteLine();

            segment.Reverse().ToList().ForEach(i => Console.Write("{0} ", i)); // output 70, 20, 9
            Console.WriteLine();

            array.ToList().ForEach(i => Console.Write("{0} ", i)); // no change
            Console.WriteLine();
            #endregion

            #region Learning-27 The Usage of Interlocked.CompareExchange()
            /*
             * LEARNING-27
             * 
             * Interlocked.CompareExchange(ref int a, int b, int c) do the following
             * if (a == c) then a = b; else do_nothing; regardless of success/fail
             * the Interlocked.CompareExchange() always return the original value of a.
             */
            int aa = 5;
            var bb = Interlocked.CompareExchange(ref aa, 4, 10);
            Console.WriteLine("aa = {0}, bb = {1}", aa, bb);
            bb = Interlocked.CompareExchange(ref aa, 4, 5);
            Console.WriteLine("aa = {0}, bb = {1}", aa, bb);
            #endregion

            #region Learning-28 The usage of Attribute
            /*
             * LEARNING-28
             * 
             * 1. Attribute 一般译作“特性”，Property 仍然译为“属性”
             * 2. Attribute 是一种可由用户自由定义的修饰符（Modifier），可以用来修饰各种需要被修饰的目标
             * 3. Attribute是程序代码的一部分，会被编译器编译进程序集（Assembly）的元数据（Metadata）里，
             *    在程序运行的时候，你随时可以从元数据里提取出这些附加信息来决策程序的运行. Attribute 本质
             *    上就是一个类，它在所附着的目标对象上最终实例化.
             * 4. AttributeUsage 这个专门用来修饰Attribute 的Attribute ，除了可以控制修饰目标外，还能决
             *    定被它修饰的Attribute 是否可以随宿主“遗传”，以及是否可以使用多个实例来修饰同一个目标！
             */
            MemberInfo m_info = typeof(TestAttribute);
            Author authorAttr = (Author)Attribute.GetCustomAttribute(m_info, typeof(Author));
            if (authorAttr != null)
            {
                Console.WriteLine("Class: {0}, Author: {1}, Team: {2}", m_info.Name, authorAttr.Name, authorAttr.Team);
            }
            #endregion

            #region Learning-29 Using delegate recursively
            /*
             * LEARNING-29
             *
             * Following is the way using delegate recursively
             * 
             * 使用“Lambda表达式来构造一个递归函数”的难点是因为“我们正在构造的东西是没有名字的”，
             * 因此“我们无法调用自身”。那么，如果我们换种写法，把我们正在调用的匿名函数作为参数传给
             * 自己，那么不就可以在匿名函数的方法体中，通过调用参数来调用自身了吗？
             * 
             * delegate TResult SelfApplicable<T, TResult>(SelfApplicable<T, TResult> self, T arg);
             * 
             * 在这里声明SelfApplicable是一个delegate，他有2个参数，第一个参数是他自己，第二个参数是
             * 类型T，返回类型是TResult
             *
             */
            SelfApplicable<int, int> selfFac = (f, x) => x <= 1 ? x : f(f, x - 1) + f(f, x - 2);
            Console.WriteLine(selfFac(selfFac, 4));

            var Fac = Fix<int, int>(f => x => x <= 1 ? x : f(x - 1) + f(x - 2));
            Console.WriteLine(Fac(4));
            #endregion

            #region Learning-30 Using special keywords as variables
            /* 
             * LEARNING-30
             */
            var @new = 1;
            var @if = 2;
            Console.WriteLine("{0} {1}", @new, @if);
            #endregion

            #region Learning-31 Use AutoResetEvent for synchronization
            /*
             * LEARNING-31 用AutoResetEvent或ManualResetEvent进行线程同步
             * 
             * 在.Net多线程编程中,AutoResetEvent和ManualResetEvent这两个类经常用到, 
             * 他们的用法很类似，但也有区别。Set方法将信号置为发送状态，Reset方法将信号
             * 置为不发送状态,WaitOne等待信号的发送。可以通过构造函数的参数值来决定其初
             * 始状态，若为true则非阻塞状态，为false为阻塞状态。如果某个线程调用WaitOne
             * 方法,则当信号处于发送状态时,该线程会得到信号, 继续向下执行。其区别就在调用
             * 后,AutoResetEvent.WaitOne()每次只允许一个线程进入,当某个线程得到信号后,
             * AutoResetEvent会自动又将信号置为不发送状态,则其他调用WaitOne的线程只有继
             * 续等待.也就是说,AutoResetEvent一次只唤醒一个线程;而ManualResetEvent则可
             * 以唤醒多个线程,因为当某个线程调用了ManualResetEvent.Set()方法后,其他调用
             * WaitOne的线程获得信号得以继续执行,而ManualResetEvent不会自动将信号置为不
             * 发送。也就是说,除非手工调用了ManualResetEvent.Reset()方法,则ManualResetEvent
             * 将一直保持有信号状态,ManualResetEvent也就可以同时唤醒多个线程继续执行。
             * 
             * - 本质上AutoResetEvent.Set()方法相当于ManualResetEvent.Set()+ManualResetEvent.Reset();
             * - 因此AutoResetEvent一次只能唤醒一个线程，其他线程还是堵塞
             * 
             */
            var processor = new Thread(ProcessingFunc);
            processor.Start();

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
                Interlocked.Increment(ref count);
                ev.Set();
                Console.WriteLine("Set Signal!");
            }

            #endregion

            #region Learning-32 Different ways of using Task
            /*
             * LEARNING-32 Task调用的几种方式
             * 
             */

            // 下面函数调用用来说明创建Task的几种方式
            TaskUsingThreadPool();

            TaskUsingSynchronous();
            TaskLongRunning();

            // 返回值是泛型类型
            TaskWithResult();

            // 使用ContinueWith方法在任务完成时启动一个新任务
            TaskContinueWith();

            #endregion

            #region Learning-33 Using async and await for asynchronous programing
            /*
             * LEARNING-33  Use async and await
             * 
             * AccessTheWebAsync() 控制流程如下：
             * 1. 事件处理程序调用并等待 AccessTheWebAsync 异步方法。
             * 2. AccessTheWebAsync 可创建 HttpClient 实例并调用 GetStringAsync 异步方法以下载网站内容作为字符串。
             * 3. GetStringAsync 中发生了某种情况，该情况挂起了它的进程。 可能必须等待网站下载或一些其他阻止活动。 
             *    为避免阻止资源，GetStringAsync 会将控制权出让给其调用方 AccessTheWebAsync。GetStringAsync 返回
             *    Task<TResult>，其中 TResult 为字符串，并且 AccessTheWebAsync 将任务分配给 getStringTask 变量。
             *    该任务表示调用 GetStringAsync 的正在进行的进程，其中承诺当工作完成时产生实际字符串值。
             * 4. 由于尚未等待 getStringTask，因此，AccessTheWebAsync 可以继续执行不依赖于 GetStringAsync 得出的
             *    最终结果的其他工作。 该任务由对同步方法 DoIndependentWork 的调用表示。
             * 5. DoIndependentWork 是完成其工作并返回其调用方的同步方法。
             * 6. AccessTheWebAsync 已用完工作，可以不受 getStringTask 的结果影响。 接下来，AccessTheWebAsync 需
             *    要计算并返回该下载字符串的长度，但该方法仅在具有字符串时才能计算该值。因此，AccessTheWebAsync 使用
             *    一个 await 运算符来挂起其进度，并把控制权交给调用 AccessTheWebAsync 的方法。 AccessTheWebAsync 
             *    将 Task<int> 返回给调用方。 该任务表示对产生下载字符串长度的整数结果的一个承诺。
             *    [Note] 如果 GetStringAsync（因此 getStringTask）在 AccessTheWebAsync 等待前完成，则控件会保留在
             *           AccessTheWebAsync 中。 如果异步调用过程 (getStringTask) 已完成，并且 AccessTheWebSync 不
             *           必等待最终结果，则挂起然后返回到 AccessTheWebAsync 将造成成本浪费。
             * 7. 在调用方内部（此示例中的事件处理程序），处理模式将继续。 在等待结果前，调用方可以开展不依赖于 
             *    AccessTheWebAsync 结果的其他工作，否则就需等待片刻。 事件处理程序等待 AccessTheWebAsync，而 
             *    AccessTheWebAsync 等待 GetStringAsync。GetStringAsync 完成并生成一个字符串结果。 字符串结果不是
             *    通过按你预期的方式调用 GetStringAsync 所返回的。 （记住，该方法已返回步骤 3 中的一个任务）。相反，
             *    字符串结果存储在表示 getStringTask 方法完成的任务中。 await 运算符从 getStringTask 中检索结果。 
             *    赋值语句将检索到的结果赋给 urlContents。
             * 8. 当 AccessTheWebAsync 具有字符串结果时，该方法可以计算字符串长度。 然后，AccessTheWebAsync 工作也将
             *    完成，并且等待事件处理程序可继续使用。 在此主题结尾处的完整示例中，可确认事件处理程序检索并打印长度结果
             *    的值。如果你不熟悉异步编程，请花 1 分钟时间考虑同步行为和异步行为之间的差异。 当其工作完成时（第 5 步）
             *    会返回一个同步方法，但当其工作挂起时（第 3 步和第 6 步），异步方法会返回一个任务值。 在异步方法最终完成
             *    其工作时，任务会标记为已完成，而结果（如果有）将存储在任务中。
             * 
             */

            Task<int> t_async = AccessTheWebAsync();
            Console.WriteLine("Length = {0}", t_async.Result);
        #endregion

            #region Learning-34 Thread.Yield() vs. Thread.Sleep()
            /*
             * LEARNING-34 Thread.Yield() vs. Thread.Sleep()
             * 
             * Yield 的中文翻译为 “放弃”，这里意思是主动放弃当前线程的时间片，并
             * 让操作系统调度其它就绪态的线程使用一个时间片。但是如果调用 Yield，
             * 只是把当前线程放入到就绪队列中，而不是阻塞队列。如果没有找到其它就
             * 绪态的线程，则当前线程继续运行。Yield 可以让低于当前优先级的线程得
             * 以运行。可以通过返回值判断是否成功调度了其它线程。
             * 
             * Sleep 的意思是告诉操作系统自己要休息 n 毫秒，这段时间就让给另一个就
             * 绪的线程吧。当 n=0 的时候，意思是要放弃自己剩下的时间片，但是仍然是
             * 就绪状态，其实意思和 Yield 有点类似。但是 Sleep(0) 只允许那些优先级
             * 相等或更高的线程使用当前的CPU，其它线程只能等着挨饿了。如果没有合适的
             * 线程，那当前线程会重新使用 CPU 时间片。相比 Yield，可以调度任何处理器
             * 的线程使用时间片。只能调度优先级相等或更高的线程，意味着优先级低的线程
             * 很难获得时间片，很可能永远都调用不到。当没有符合条件的线程，会一直占用
             *  CPU 时间片，造成 CPU 100%占用率。
             * 
             */

            Thread t = new Thread(() =>
            {
                while (true)
                {
                    // Sleep 0ms，如果有其他就绪线程就执行其他线程，如果没有则继续当前线程
                    Thread.Sleep(0);

                    Console.WriteLine("Thread 2 running");
                }
            });
            t.Start();

            while (true)
            {
                // 主动让出CPU，如果有其他就绪线程就执行其他线程，如果没有则继续当前线程
                var result = Thread.Yield();

                // Do Some Work
                Console.WriteLine("Thread 1 running, yield result {0}", result);
            }

            #endregion

            #region Learning-35 Eager Singleton vs. Lazy Singleton
            /*
             * LEARNING-35 Eager Singleton vs. Lazy Singleton
             * 
             * Eager Singleton(饿汉式单例类)，其静态成员在类加载时就被初始化，此时
             * 类的私有构造函数被调用，单例类的唯一实例就被创建。
             * Lazy Singleton（懒汉式单例类），其类的唯一实例在真正调用时才被创建，
             * 而不是类加载时就被创建。所以Lazy Singleton不是多线程安全的。
             * 
             */
            EagerSingleton es = EagerSingleton.GetInstance();
            LazySingleton ls = LazySingleton.GetInstance();
            #endregion

            #region Learning-36 Difference between Where and TakeWhile
            //TakeWhile: stops until the condition is false
            //Where:  continues and find all elements matching the condition
            var intList = new int[] { 1, 2, 3, 4, 5, -1, -2 };
            Console.WriteLine("Where");
            foreach (var i in intList.Where(x => x <= 3))
                Console.WriteLine(i);

            Console.WriteLine("TakeWhile");
            foreach (var i in intList.TakeWhile(x => x <= 3))
                Console.WriteLine(i);
            #endregion

            #region Learning-37 DataTable and CsvHelper/CsvReader/CsvDataReader
            var table = new DataTable();
            var csvFile = Directory.GetCurrentDirectory() + @"\..\..\..\test.csv";
            Dictionary<string, Type> TypeMappings = new Dictionary<string, Type>();

            using (var reader = File.OpenText(csvFile))
            {
                using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
                using (var dataReader = new CsvHelper.CsvDataReader(csv))
                {
                    table.Load(dataReader);
                }
            }

            var boys = table.Rows.Cast<DataRow>().Where(r => r["Sex"].ToString() == "boy");

            // Another way
            using (var reader = File.OpenText(csvFile))
            {
                using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
                using (var dataReader = new CsvHelper.CsvDataReader(csv))
                {
                    var records = csv.GetRecords<Student>().ToList();
                }
            }

        #endregion

        test_item:

            #region Learning-38 LINQ Query - GroupBy Usage
            var table2 = new DataTable();
            var csvFile2 = Directory.GetCurrentDirectory() + @"\..\..\..\test2.csv";
            Dictionary<string, Type> TypeMappings2 = new Dictionary<string, Type>();

            using (var reader = File.OpenText(csvFile2))
            {
                using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
                using (var dataReader = new CsvHelper.CsvDataReader(csv))
                {
                    table2.Load(dataReader);
                }
            }

            var FR1_TCs = table2.Rows.Cast<DataRow>()
                                .Where(r => r["Frequency Range"].ToString() == "FR1")
                                .Select(r => r["Test Case"])
                                .Distinct();

            var FR2_TCs = (from r in table2.Rows.Cast<DataRow>()
                           where r["Frequency Range"].ToString() == "FR2"
                           select r["Test Case"]).Distinct();

            var SA_TCs = table2.Rows.Cast<DataRow>()
                                         .GroupBy(r => r["Mode"], r => r["Test Case"])
                                         .Where(r => r.Key.ToString() == "SA")
                                         .First()
                                         .ToList()
                                         .Distinct();

            var NSA_TCs = (from r in table2.Rows.Cast<DataRow>()
                          where r["Mode"].ToString() == "NSA"
                          group r["Test Case"] by r["Mode"]
                          ).First().ToList().Distinct();
                          
            #endregion

            goto test_end;

        test_end:

            Console.ReadKey();
        }

    }
}
