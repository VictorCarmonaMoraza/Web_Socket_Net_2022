using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimerServidorWebSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            //Nuestra ipv4
            string ipServidorWebSocket = "ws://192.168.0.111:9001";
            //Creamos nuestro WebSocketServer
            WebSocketServer servidorSocket = new WebSocketServer(ipServidorWebSocket);
            //Lista de clientes que se van a conectar a nuestro WebSocket.Alamacena todos los clientes conecatdos
            List<IWebSocketConnection> clientesSockets = new List<IWebSocketConnection>();
            Console.WriteLine("Servidor web sockets iniciado");
            servidorSocket.Start(clienteSocket =>
            {
                //Cada vez que nos conectamos a nuestro socket
                clienteSocket.OnOpen = () =>
                {
                    clientesSockets.Add(clienteSocket);
                    Console.WriteLine("Cliente conectado");
                };
                //Caundo tu envias algo al socket
                clienteSocket.OnMessage = (string texto) =>
                {
                    //Con esta expresion emitesa a todos, emites el texto que hemos enviado
                    clientesSockets.ForEach(p=>p.Send(texto));
                    //Equivalente de lo de arriba
                    //for (int i = 0; i < clientesSockets.Count; i++)
                    //{
                    //    clientesSockets[i].Send(texto);
                    //}

                    //Otra forma de hacerlo
                    //foreach (var cliente in clientesSockets)
                    //{
                    //    cliente.Send(texto);
                    //}
                };
                //Cuando ya no estamos conectados al socket
                clienteSocket.OnClose = () =>
                {
                    clientesSockets.Remove(clienteSocket);
                    Console.WriteLine("Cliente desconectado");
                };
            });

            Console.ReadLine();
        }
    }
}
