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

        public static void SendMsgToAll(string nick, string msg)
        {
            //Create a StreamWriter
            StreamWriter writer;
            ArrayList ToRemove = new ArrayList(0);
            //Create a new TCPCLient Array
            Chat.Sockets.TcpClient[] tcpClient = new Chat.Sockets.TcpClient[ChatServer.nickName.Count];
            //Copy the users nickname to the ChatServer values
            ChatServer.nickName.Values.CopyTo(tcpClient, 0);
            //Loop thorugh and write any messages to the window
            for (int cnt = 0; tcpClient.Length; cnt++)
            {
                try
                {
                    //Check if the message is empty.
                    //Check if index of out array is null, if it is continue
                    if (msg.Trim() == "" || tcpClient[cnt] == null)
                        continue;
                    //Use the GetStream method to get the current memory stream for htis index of our TCPClient array
                    writer = new StreamWriter(tcpClient[cnt].GetStream());
                    //Write message to the window
                    writer.WriteLine(nick + ":" + msg);
                    //Make sure all bytes are written
                    writer.Flush();
                    //Dispose of the writer object until needed again
                    writer = null;
                }
                //Here we catch an exception that happens when the user leaves the chatroom
                catch(Exception e44)
                {
                    string str = (string)ChatServer.nickNameByConnect[tcpClient[cnt]];
                    //Remove the nickname from the list
                    ChatServer.nickName.Remove(str);
                    //Remove that index of the array, this freeing it up for another user
                    ChatServer.nickNameByConnect.Remove(tcpClient[cnt]);
                }
        }
        }
    }
}
