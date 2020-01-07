
using System;
using System.Collections.Generic;
using System.Text;

// A C# program for Client
using System.Net;
using System.Net.Sockets;

namespace ClientAppDynamics
{
    class Client
    {
        private static string command = "";

        // Main Method 
        static void Main(string[] args)
        {
            ExecuteClient(args);
        }

        // Check if input arguments were supplied and valid.
        private static bool areValidArgs(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a command.");
                return false;
            }

            if (args.Length > 2)
            {
                Console.WriteLine("Limit of commands over. Try with HELLO, TIME or DIR /path.");
                return false;
            }

            command = string.Join(" ", args); //It sets the command if they were provided
            Console.WriteLine("\nCommand introduced: " + command);

            return true;

        }

        // ExecuteClient() Method. Runs the client to establish a TCP/IP connection with a server.
        static void ExecuteClient(string[] args)
        {
            if (areValidArgs(args))
            {
                try
                {

                    // Establish the remote endpoint for the socket. 
                    //This example uses port 10000. 
                    IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                    IPAddress ipAddr = ipHost.AddressList[0]; //IPAddress.Parse("51.140.222.237") for introducing a specific IP
                    IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 10000);

                    // Creation TCP/IP Socket using Socket Class Costructor 
                    Socket sender = new Socket(ipAddr.AddressFamily,
                            SocketType.Stream, ProtocolType.Tcp);

                    try
                    {

                        // Connect Socket to the remote endpoint using method Connect() 
                        sender.Connect(localEndPoint);

                        // We print EndPoint information that we are connected 
                        Console.WriteLine("Socket connected to {0} ",
                                    sender.RemoteEndPoint.ToString());

                        // Creation of messagge that we will send to Server
                        byte[] messageSent = Encoding.ASCII.GetBytes(command); 
                        int byteSent = sender.Send(messageSent);

                        // Data buffer 
                        byte[] messageReceived = new byte[1024];

                        // We receive the messagge using the method Receive(). 
                        // This method returns number of bytes received, that we'll use to convert them to string 
                        int byteRecv = sender.Receive(messageReceived);
                        Console.WriteLine("\nMessage from Server -> \n{0}",
                            Encoding.ASCII.GetString(messageReceived,
                                                        0, byteRecv));

                        // Close Socket using the method Close() 
                        sender.Shutdown(SocketShutdown.Both);
                        sender.Close();
                    }

                    // Manage of Socket's Exceptions 
                    catch (ArgumentNullException ane)
                    {

                        Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                    }

                    catch (SocketException se)
                    {

                        Console.WriteLine("SocketException " + se.ErrorCode + " : {0}", "Connection cant be established, the server denied the connection");
                    }

                    catch (Exception e)
                    {
                        Console.WriteLine("Unexpected exception : {0}", e.ToString());
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

            }
            else
            {
                return;
            }


        }
    }
}
