using System.IO;
using System.Net;
using System;
using System.Threading;
using Chat = System.Net;
using System.Collections;

namespace ChatServer
{
    class ChatServer
    {
        System.Net.Sockets.TcpListener chatServer;
        public static Hashtable nickName;
        public static Hashtable nickNameByConnect;
        public static IPAddress LocalAddress;

        public ChatServer()
        {

            // -Initialise the IPAddress.
            LocalAddress = Dns.GetHostEntry("localhost").AddressList[0];

            //create nickname and nikcnmae by connection varriables
            // - Size of the HASHTABLE here is to prevent large names causing problems.
            nickName = new Hashtable(100);
            nickNameByConnect = new Hashtable(100);

            //Create TCPListner Object
            // - Nothing I know of uses this port.
            chatServer = new System.Net.Sockets.TcpListener(4296);
            
            //Check to see if the server is running
            //While (true) do commands

            while (true)
            {
                chatServer.Start();
                if (chatServer.Pending())
                {
                    //If there are pending requests create a new connection
                    Chat.Sockets.TcpClient chatConnection = chatServer.AcceptTcpClient();
                    //Display a warning letting the user know they're connected

                    //TODO: DISPLAY THE NAME OF THE ROOM
                    Console.WriteLine("You are no connected to: ");
                    //Create a new DoCommunicate Object
                    DoCommunicate comm = new DoCommunicate(chatConnection);
                }
            }

        }
    }
}
