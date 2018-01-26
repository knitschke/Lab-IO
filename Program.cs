using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication8
{

    class Program
    {


        //*z1
    static void ThreadProc(Object stateInfo)
    {
        Thread.Sleep((int)stateInfo);
        System.Console.WriteLine("Waited for "+stateInfo);
    }

        //*/

        //*z2
        static void ServerThread(Object stateInfo)
            {
                TcpListener server = new TcpListener(IPAddress.Any, 2048);
                server.Start();
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    byte[] buffer = new byte[1024];
                    client.GetStream().Read(buffer, 0, 1024);
                    client.GetStream().Write(buffer, 0, buffer.Length);
                    client.Close();
                }
            }

            static void Client1Thread(Object stateInfo)
            {
                TcpClient client = new TcpClient();
                client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
                byte[] message = new ASCIIEncoding().GetBytes("wiadomosc");
                client.GetStream().Write(message, 0, message.Length);

            }
        //*/


        /*z34
         private static Object thisLock = new Object(); 
     
         static void ServerThread(Object stateInfo)
            {
                TcpListener server = new TcpListener(IPAddress.Any, 2048);
                server.Start();
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(ClientServingThread, client);
                                }
            }

            static void ClientThread(Object stateInfo)
            {
                TcpClient client = new TcpClient();
                client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
                byte[] message = new ASCIIEncoding().GetBytes("wiadomosc");
                client.GetStream().Write(message, 0, message.Length);
                byte[] buffer = new byte[1024];
                NetworkStream stream = client.GetStream();
                stream.Read(buffer, 0, message.Length);
                lock (thisLock)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine("Klient odebral: " + System.Text.Encoding.Default.GetString(buffer));
                    Console.ForegroundColor = ConsoleColor.White;
                }
            


            }

            static void ClientServingThread(Object stateInfo)
            {
                TcpClient client = (TcpClient)stateInfo;
                byte[] buffer = new byte[1024];
                client.GetStream().Read(buffer, 0, 1024);
                lock (thisLock)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.WriteLine("Serwer odebral: " + System.Text.Encoding.Default.GetString(buffer));
                    Console.ForegroundColor = ConsoleColor.White;
                }
           
                client.GetStream().Write(buffer, 0, buffer.Length);
                client.Close();

            }
     
     
        */

        //*z5
            private static Object thisLock = new Object();

            private static int result = 0;

            static List<WaitHandle> waitHandles = new List<WaitHandle>();
     
            private static void RunThrFrgment(List<int[]> fragmenty)
            {
                int j = 0;
                foreach (int[] fragment in fragmenty)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(CountingThread), new HandleAndNumber(fragment, waitHandles[j]));
                    j++;
                }
            }

            private static List<int[]> crFragm(ref int TabLen, int FragmentL)
            {
                List<int[]> fragmenty = new List<int[]>();
                while (TabLen > FragmentL - 1)
                {
                    int[] fragment = new int[FragmentL];
                    randomfill(fragment);
                    fragmenty.Add(fragment);
                    TabLen -= FragmentL;
                }

                int[] lastFrag = new int[TabLen];
                randomfill(lastFrag);
                fragmenty.Add(lastFrag);
                return fragmenty;
            }

            private static void CreateHandlerForEachFragment(int TabLen, int FragmentL)
            {
                for (int i = 0; i < TabLen / FragmentL + 1; i++)
                {
                    waitHandles.Add(new AutoResetEvent(false));
                }
            }

            static void CountingThread(Object stateInfo)
            {
                HandleAndNumber handleAndNumber = (HandleAndNumber)stateInfo;
                AutoResetEvent are = (AutoResetEvent)handleAndNumber.waitHandle;
                lock (thisLock)
                {
                    for(int i = 0; i < handleAndNumber.fragment.Length; i++)
                    {
                        result += handleAndNumber.fragment[i];
                    }
                }
                are.Set();
            }

        static void randomfill(int [] table)
            {
                Random rnd = new Random();
                for (int i = 0; i < table.Length; i++)
                {
                    table[i]= rnd.Next(0, 101);
                }
            }



        public class HandleAndNumber
        {
            public int [] fragment;
            public WaitHandle waitHandle;

            public HandleAndNumber(int [] fragment, WaitHandle waitHandle)
            {
                this.fragment = fragment;
                this.waitHandle = waitHandle;
            }
        }
     
         //*/

        //*z6
        static void myAsyncCallback (IAsyncResult state) {
                object[] fsAndBuffer = (object[]) state.AsyncState;
                FileStream fs = (FileStream)fsAndBuffer[0];
                byte[] buffer = (byte[])fsAndBuffer[1];
                int l = fs.EndRead(state);
                string tresc = Encoding.UTF8.GetString(buffer, 0, l);
                Console.Out.WriteLine(tresc);
            }
         //*/




        delegate int DelegateType(int n);
        static DelegateType f1;
        static DelegateType f2;
        static DelegateType f3;
        static DelegateType f4;

        static int FibR(int n)
        {
            if (n == 0) return 0;
            if (n == 1) return 1;
            else return FibR(n - 1) + FibR(n - 2);
            
        }
        static int FibI(int n)
        {
            int f1=1; 
            int f2=1; 
            int temp;

            if (n == 0) return 0;
            if (n == 1) return 1;
            else
            {
                for (int i = 3; i <= n; i++)
                {
                    temp = f1 + f2;
                    f1 = f2;
                    f2 = temp;
                }
                return f2;
            }
        }
        static int SilniaR(int n)
        {
            return 0;
        }
        static int SilniaI(int n)
        {
            return 0;
        }
     






        static void Main(string[] args)
        {
            /*z1
            ThreadPool.QueueUserWorkItem(ThreadProc, 100);
            ThreadPool.QueueUserWorkItem(ThreadProc, 1000);
            Thread.Sleep(1500);
            */

            /*z2
            ThreadPool.QueueUserWorkItem(ServerThread);
            ThreadPool.QueueUserWorkItem(Client1Thread);
            ThreadPool.QueueUserWorkItem(Client1Thread);
            Thread.Sleep(1000);
            
            */

            /*z34

            ThreadPool.QueueUserWorkItem(ServerThread);
            ThreadPool.QueueUserWorkItem(ClientThread);
            ThreadPool.QueueUserWorkItem(ClientThread);
            Console.ReadKey();
             
             */


            /*z5
            int TabLen = 250;
            int FragmentL = 15;

            CreateHandlerForEachFragment(TabLen, FragmentL);
            List<int[]> fragmenty = crFragm(ref TabLen, FragmentL);

            Stopwatch stopwatch = Stopwatch.StartNew();
            RunThrFrgment(fragmenty);
            System.Threading.Thread.Sleep(500);
            stopwatch.Stop();
            Console.WriteLine("Czas w ms: "+stopwatch.ElapsedMilliseconds);

            WaitHandle.WaitAll(waitHandles.ToArray());
            Console.Out.WriteLine("Wynik: "+result);
            Console.ReadKey(); 
             */

            /*z6
            FileStream fs = File.OpenRead("plik.txt");
            byte[] buffer = new byte[1000];
            fs.BeginRead(buffer, 0, buffer.Length, myAsyncCallback, new object[]{ fs, buffer});
            Console.ReadKey();
            */

            //*z7
            FileStream fs = File.OpenRead("file.txt");
            byte [] buffer = new byte[1000];
            var IAsync = fs.BeginRead(buffer, 0, buffer.Length, null, "arg nie uzywany");

            for (int i = 0; i < 1000000; i++)
                i = i + 1;
            int l = fs.EndRead(IAsync);
            string tresc = Encoding.UTF8.GetString(buffer, 0, l);
            fs.Close();
            Console.WriteLine(tresc);
            Console.ReadKey();
            //*/

            //8
            f1 = new DelegateType(FibI);
            f2 = new DelegateType(FibR);
            f3 = new DelegateType(SilniaI);
            f4 = new DelegateType(SilniaR);

            IAsyncResult ar1 = f1.BeginInvoke(5, null, null);
            IAsyncResult ar2 = f2.BeginInvoke(5, null, null);
            IAsyncResult ar3 = f3.BeginInvoke(5, null, null);
            IAsyncResult ar4 = f4.BeginInvoke(5, null, null);

            int result1 = f1.EndInvoke(ar1);
            int result2 = f2.EndInvoke(ar2);
            int result3 = f3.EndInvoke(ar3);
            int result4 = f4.EndInvoke(ar4);

            Console.WriteLine(result1 + " " + result2);

            Console.ReadKey();
        }
    }
}
