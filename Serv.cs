using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;


namespace _15c
{
class Serv
{
    
    TcpListener server;
    int port;
    IPAddress address;
    bool running = false;
    CancellationTokenSource cts = new CancellationTokenSource();
    Task serverTask;

    public Task ServerTask
    {
        get { return serverTask; }
    }
    



    
    public IPAddress Address
    {
        get { return address; }
        set
        {
            if (!running) address = value;
            else ;
        }
    }
    public int Port
    {
        get { return port; }
        set
        {
            if (!running)
                port = value;
            else ;
        }
    }
    
    
    public Serv()
    {
        Address = IPAddress.Any;
        port = 2048;
    }
    public Serv(int port)
    {
        this.port = port;
    }
    public Serv(IPAddress address)
    {
        this.address = address;
    }
    
    

    public async Task RunAsync(CancellationToken ct)
    {

        server = new TcpListener(address, port);

        try
        {
            server.Start();
            running = true;
        }
        catch (SocketException ex)
        {
            throw (ex);
        }
        while (true && !ct.IsCancellationRequested)
        {

            TcpClient client = await server.AcceptTcpClientAsync();
            byte[] buffer = new byte[1024];
            using (ct.Register(() => client.GetStream().Close()))
            {
                client.GetStream().ReadAsync(buffer, 0, buffer.Length, ct).ContinueWith(
                    async (t) =>
                    {
                        int i = t.Result;
                        while (true)
                        {
                            client.GetStream().WriteAsync(buffer, 0, i, ct);
                            try
                            {
                                i = await client.GetStream().ReadAsync(buffer, 0, buffer.Length, ct);
                            }
                            catch
                            {
                                break;
                            }
                        }
                    });
            }
        }

    }
    public void RequestCancellation()
    {
        cts.Cancel();

        server.Stop();
    }
    public void Run()
    {

        serverTask = RunAsync(cts.Token);
    }
    public void StopRunning()
    {
        RequestCancellation();
 
    }
    
}
}