using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;







namespace _15c
{
    class Program
    {




        static void Main(string[] args)
        {


            Serv sv = new Serv();
            sv.Run();
            Klient a1 = new Klient();
            Klient a2 = new Klient();
            Klient a3 = new Klient();

            a1.Connect();
            a2.Connect();
            a3.Connect();

            CancellationTokenSource tok1 = new CancellationTokenSource();
            CancellationTokenSource tok2 = new CancellationTokenSource();
            CancellationTokenSource tok3 = new CancellationTokenSource();

            var client1 = a1.keepPinging("message", tok1.Token);
            var client2 = a2.keepPinging("message", tok2.Token);
            var client3 = a3.keepPinging("message", tok3.Token);

            tok1.CancelAfter(2000);
            tok2.CancelAfter(3000);
            tok3.CancelAfter(4000);

            Task.WaitAll(new Task[] { client1, client2, client3 });
            sv.StopRunning();

            Console.WriteLine(client1.Result);
            Console.WriteLine(client2.Result);
            Console.WriteLine(client3.Result);




        }
    }
}