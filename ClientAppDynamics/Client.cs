
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

        // Test if input arguments were supplied.
        private static bool isValidArgs(string[] args)
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

            return true;

        }

        // ExecuteClient() Method 
        static void ExecuteClient(string[] args)
        {
            if (isValidArgs(args))
            {
                try
                {

                    // Establish the remote endpoint 
                    // for the socket. This example 
                    // uses port 11111 on the local 
                    // computer. 
                    IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                    IPAddress ipAddr = ipHost.AddressList[0];
                    IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

                    // Creation TCP/IP Socket using 
                    // Socket Class Costructor 
                    Socket sender = new Socket(ipAddr.AddressFamily,
                            SocketType.Stream, ProtocolType.Tcp);

                    try
                    {

                        // Connect Socket to the remote 
                        // endpoint using method Connect() 
                        sender.Connect(localEndPoint);

                        // We print EndPoint information 
                        // that we are connected 
                        Console.WriteLine("Socket connected to -> {0} ",
                                    sender.RemoteEndPoint.ToString());

                        // Creation of messagge that 
                        // we will send to Server 
                        byte[] messageSent = Encoding.ASCII.GetBytes(command + "<EOF>");
                        int byteSent = sender.Send(messageSent);

                        // Data buffer 
                        byte[] messageReceived = new byte[1024];

                        // We receive the messagge using 
                        // the method Receive(). This 
                        // method returns number of bytes 
                        // received, that we'll use to 
                        // convert them to string 
                        int byteRecv = sender.Receive(messageReceived);
                        Console.WriteLine("Message from Server -> {0}",
                            Encoding.ASCII.GetString(messageReceived,
                                                        0, byteRecv));

                        // Close Socket using 
                        // the method Close() 
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

                        Console.WriteLine("SocketException : {0}", se.ToString());
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
