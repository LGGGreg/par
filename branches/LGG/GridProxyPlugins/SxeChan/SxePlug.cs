using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using OpenMetaverse;
using OpenMetaverse.Packets;
using OpenMetaverse.StructuredData;
using GridProxy;
using System.Threading;
using System.Windows.Forms;
using Meebey.SmartIrc4net;

namespace SxeChan
{
    public class GregIRC
    {
        public Thread lis;
        int i;
        public IrcClient irc;
        private SxePlugin sxe;
        private Form1 form;
        private  string myName;
        public GregIRC(SxePlugin sex, Form1 f)
        {
            sxe = sex;
            form = f;
            irc = new IrcClient();
            irc.AutoReconnect = false;
            irc.AutoRetry = true;
            irc.AutoRetryDelay = 2;

            irc.Encoding = System.Text.Encoding.UTF8;
            irc.SendDelay = 200;
            irc.ActiveChannelSyncing = true;
            irc.OnError += new Meebey.SmartIrc4net.ErrorEventHandler(OnError);
            irc.OnConnected += new EventHandler(OnConnected);
            irc.OnConnecting += new EventHandler(OnConnecting);
            irc.OnDisconnected += new EventHandler(OnDisconnected);
            //irc.OnReadLine += new ReadLineEventHandler(OnReadLine);
            irc.OnChannelMessage += new IrcEventHandler(OnChannelMessage);
            irc.OnQueryMessage += new IrcEventHandler(OnQueryMessage);
            irc.OnPing += new PingEventHandler(OnPing);

            sex.readData();

        }
        void OnConnected(object sender, EventArgs e)
        {
            
            sxe.sendGroupIm("SxeChat", myName + " has Connected to server " + sxe.server + " on port " + sxe.port + " to the room " + sxe.chanel, 0);
            sxe.saveData();

            lis = new Thread(new ThreadStart(ReadCommands));
            lis.Start();

            
        }
        public void ReadCommands()
        {

            irc.Listen();
            
        }
        public static void OnError(object sender, Meebey.SmartIrc4net.ErrorEventArgs e)
        {
            System.Console.WriteLine("Error: " + e.ErrorMessage);
            
        }
        public void setserver(string s)
        {
            sxe.server = s;
        }
        public void setchanel(string c)
        {
            sxe.chanel = c;
        }
        public void setport(int p)
        {
            sxe.port = p;
        }
        public void setname(string n)
        {
            myName = n;
        }
        void OnChannelMessage(object sender, IrcEventArgs e)
        {
            if (sxe.getForm() == false) return;
            Console.WriteLine(e.Data.Type + ":");
            Console.WriteLine("(" + e.Data.Channel + ") <" +
    e.Data.Nick + "> " + e.Data.Message);
            sxe.sendGroupIm(e.Data.Nick, e.Data.Message, 0);
        }
        void OnQueryMessage(object sender, IrcEventArgs e)
        {
            if (sxe.getForm() == false) return;

            Console.WriteLine(e.Data.Type + ":");

            sxe.sendGroupIm(e.Data.Nick, "(Private)" + e.Data.Message, 0);
            Console.WriteLine("(private) <" + e.Data.Nick + "> " +
    e.Data.Message);
        }
        void OnPing(object sender, PingEventArgs e)
        {
            Console.WriteLine("Responded to ping at {0}.",
    DateTime.Now.ToShortTimeString());
        }
        void OnConnecting(object sender, EventArgs e)
        {
            Console.WriteLine("Connecting to {0}.", sxe.server);
        }
        void OnDisconnected(object sender, EventArgs e)
        {
            Console.WriteLine("Disconnected.");
        }
        public void pleaseConnect(string server, int port)
        {

            try
            {
                // here we try to connect to the server and exceptions get handled
                irc.Connect(server, port);
            }
            catch (ConnectionException e)
            {
                // something went wrong, the reason will be shown
                System.Console.WriteLine("couldn't connect! Reason: " + e.Message);
                
            }

            try
            {
                // here we logon and register our nickname and so on 

                irc.Login(myName, myName);
                // join the channel
                irc.RfcJoin(sxe.chanel);

                    irc.SendMessage(SendType.Action, sxe.chanel, "joined this channel from sl");



            }
            catch (ConnectionException e)
            {
                // this exception is handled because Disconnect() can throw a not
                // connected exception
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                // this should not happen by just in case we handle it nicely
                System.Console.WriteLine("Error occurred! Message: " + e.Message);
                System.Console.WriteLine("Exception: " + e.StackTrace);
                
            }
        }
    }
    public class SxePlugin : ProxyPlugin
    {
        private GregIRC girc;
        private ProxyFrame frame;
        private Proxy proxy;
        private Thread formthread;
        private Form1 form;
        private string brand;
        private bool pass = true;
        public string server;
        public string chanel;
        private int chan;
        public int port;
        private string myName;
        private UUID grop;
        private bool lsn;
        private Thread ircthred;

        private PacketDelegate _packetDelegate;
        private PacketDelegate packetDelegate
        {
            get
            {
                if (_packetDelegate == null)
                {
                    _packetDelegate = new PacketDelegate(NameHandler);
                }
                return _packetDelegate;
            }
        }

        public SxePlugin(ProxyFrame frame)
        {
            
            formthread = new Thread(new ThreadStart(delegate()
            {
                form = new Form1(this);
                Application.Run(form);
            }));

            formthread.SetApartmentState(ApartmentState.STA);
            formthread.Start();


            ircthred = new Thread(new ThreadStart(delegate()
            {
                girc = new GregIRC(this,form);
            }));

            ircthred.SetApartmentState(ApartmentState.MTA);
            ircthred.Start();

            
                       

            this.frame = frame;
            this.proxy = frame.proxy;
            this.brand = "SxeChan";
            proxy.AddDelegate(PacketType.ChatFromViewer, Direction.Outgoing, new PacketDelegate(SimChat));
            proxy.AddDelegate(PacketType.ImprovedInstantMessage, Direction.Outgoing, new PacketDelegate(IMs));
            grop = UUID.Random();
            writethis("inited worked");

            
        }
        public void writethis(string th)
        {
             ConsoleColor oldback = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.DarkRed ;
            Console.WriteLine(th);
            Console.BackgroundColor = oldback;
        }
        private void gotName()
        {
            Console.WriteLine( myName+" is trying to connect to server "+server+" on port "+port+" to the room "+chanel+" listening to chat on channel "+chan.ToString());
            
            proxy.RemoveDelegate(PacketType.UUIDNameReply, Direction.Incoming, this.packetDelegate);
            girc.pleaseConnect(server, port);

                
            
        }
        private void getName()
        {

            proxy.AddDelegate(PacketType.UUIDNameReply, Direction.Incoming, this.packetDelegate);
            UUIDNameRequestPacket p = new UUIDNameRequestPacket();
            p.UUIDNameBlock = new UUIDNameRequestPacket.UUIDNameBlockBlock[1];
            p.UUIDNameBlock[0] = new UUIDNameRequestPacket.UUIDNameBlockBlock();
            p.UUIDNameBlock[0].ID = frame.AgentID;
            p.Header.Reliable = true;
            
            proxy.InjectPacket(p, Direction.Outgoing);


        }
        public bool getForm()
        {
            return form.getCheck();
        }
        private Packet NameHandler(Packet p, IPEndPoint sim)
        {
            UUIDNameReplyPacket nrp = (UUIDNameReplyPacket)p;
            for (int i = 0; i < nrp.UUIDNameBlock.Length; i++)
            {
                if (nrp.UUIDNameBlock[i].ID == frame.AgentID)
                {
                    myName=Utils.BytesToString(nrp.UUIDNameBlock[i].FirstName)+"-"+Utils.BytesToString(nrp.UUIDNameBlock[i].LastName);
                    if (myName.Length > 32)
                        myName = myName.Substring(0, 32);
                    girc.setname(myName);
                    gotName();
                    return p;
                }
            }
            proxy.RemoveDelegate(PacketType.UUIDNameReply, Direction.Incoming, this.packetDelegate);
            getName();
            return p;

            
        }
        public void openwindow()
        {
            sendGroupIm("SxeChat", "Window Opened", 0);
        }
        public void ircRefresh()
        {
            grop = UUID.Random();
            if (girc == null) return;
            if (girc.irc.IsConnected)
            {
                try
                {
                    girc.irc.Disconnect();
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            if (myName.Length > 3)
                gotName();


        }
        public void setInfo(string achan, string aser, int ach, int aport)
        {
            this.chan = ach;
            this.chanel = achan;
            this.port = aport;
            this.server = aser;
            this.saveData();
            this.ircRefresh();
        }
        private Packet IMs(Packet p, IPEndPoint sim)
        {
            ImprovedInstantMessagePacket im = (ImprovedInstantMessagePacket)p;
            if (im.MessageBlock.ToAgentID == grop)
            {
                if (Utils.BytesToString(im.MessageBlock.Message).ToLower().Trim().Equals("typing")) return null;
                girc.irc.SendMessage(SendType.Message, chanel, Utils.BytesToString(im.MessageBlock.Message), Priority.High);
                return null;
            }
            return p;
        }
       
        public void loadDefaults()
        {
            this.chan = 1337;
            this.chanel = "#IRCPLUG";
            this.server = "us.undernet.org";
            this.port = 6667;
            this.lsn = true;
        }
        public void readData()
        {
            loadDefaults();
            if (File.Exists("SxeChan.settings"))
            {
                StreamReader re = File.OpenText("SxeChan.settings");
                lsn = false;
                chan = int.Parse(re.ReadLine().Trim());
                chanel = re.ReadLine();
                server = re.ReadLine();
                port = int.Parse(re.ReadLine().Trim());
                try
                {
                    if (re.ReadLine() == "on")
                        lsn = true;
                }
                catch
                {
                }
                re.Close();
            }
            //girc.setport(port);
            //girc.setserver(server);
            //girc.setchanel(chanel);
            
        }

        public void loadme()
        {

            form.setForm( server, chan.ToString(),chanel, port.ToString(),lsn);
        }
        public void saveData()
        {
            TextWriter tw = new StreamWriter("SxeChan.settings");
            
            tw.WriteLine(chan);
            tw.WriteLine(chanel);
            tw.WriteLine(server);
            tw.WriteLine(port);
            string s = "off";
            if (form.getCheck()) s = "on";
            tw.WriteLine(s);
            tw.Close();
        }
        public Packet SimChat(Packet p, IPEndPoint s)
        {
            ChatFromViewerPacket ch = (ChatFromViewerPacket)p;
            if (ch.ChatData.Channel == this.chan)
            {

                girc.irc.SendMessage(SendType.Message, chanel, Utils.BytesToString(ch.ChatData.Message), Priority.High);
                sendGroupIm(myName, Utils.BytesToString(ch.ChatData.Message), 0);
                return null;
            }
            
            return p;
        }
        public override void Init()
        {
            SayToUser("/me loaded");
            getName();
        }
        private void SayToUser(string message)
        {

            ChatFromSimulatorPacket packet = new ChatFromSimulatorPacket();
            packet.ChatData.FromName = Utils.StringToBytes(this.brand);
            packet.ChatData.SourceID = UUID.Random();
            packet.ChatData.OwnerID = frame.AgentID;
            packet.ChatData.SourceType = (byte)2;
            packet.ChatData.ChatType = (byte)1;
            packet.ChatData.Audible = (byte)1;
            packet.ChatData.Position = new Vector3(0, 0, 0);
            packet.ChatData.Message = Utils.StringToBytes(message);
            proxy.InjectPacket(packet, Direction.Incoming);
        }
        public void sendGroupIm(string ircname, string ircmsg, int t)
        {
            
            //SayToUser(ircname + ": " + ircmsg);
            ImprovedInstantMessagePacket im = new ImprovedInstantMessagePacket();
            im.AgentData.AgentID = grop ;
            im.AgentData.SessionID = UUID.Random();
            im.MessageBlock.FromGroup = false;
            im.MessageBlock.ToAgentID = frame.AgentID;
            im.MessageBlock.ParentEstateID = 0;
            im.MessageBlock.RegionID = UUID.Zero;
            im.MessageBlock.Position = Vector3.Zero;
            im.MessageBlock.Offline = 0;
            im.MessageBlock.Dialog = (byte)t;
            im.MessageBlock.ID = this.grop;
            im.MessageBlock.FromAgentName = Utils.StringToBytes(ircname);
            im.MessageBlock.Message = Utils.StringToBytes(ircmsg);
            im.MessageBlock.Timestamp = 0;
            im.MessageBlock.BinaryBucket = Utils.StringToBytes(chanel);

            proxy.InjectPacket(im, Direction.Incoming);

            /*
             * --- ImprovedInstantMessage ---
            -- AgentData --
            AgentID: 160013f7-9095-4547-a93d-8c54210de18c
            SessionID: 00000000-0000-0000-0000-000000000000
            -- MessageBlock --
            FromGroup: False
            ToAgentID: f0334909-28bc-4717-ae0e-1893c2881e3d
            ParentEstateID: 0
            RegionID: 00000000-0000-0000-0000-000000000000
            Position: <0, 0, 0>
            Offline: 0
            Dialog: 17
            ID: 9c196cd7-04ce-0d29-ef03-625e3da11aef
            Timestamp: 0
            FromAgentName: ciganito111 Babii
            Message: hi
            BinaryBucket: SpacePirates*/

        }
        public void SendUserAlert(string message)
        {
            AlertMessagePacket packet = new AlertMessagePacket();
            packet.AlertData.Message = Utils.StringToBytes(message);

            proxy.InjectPacket(packet, Direction.Incoming);

        }
        private void SendUserDialog(string first, string last, string objectName, string message, string[] buttons)
        {
            Random rand = new Random();
            ScriptDialogPacket packet = new ScriptDialogPacket();
            packet.Data.ObjectID = UUID.Random();
            packet.Data.FirstName = Utils.StringToBytes(first);
            packet.Data.LastName = Utils.StringToBytes(last);
            packet.Data.ObjectName = Utils.StringToBytes(objectName);
            packet.Data.Message = Utils.StringToBytes(message);
            packet.Data.ChatChannel = (byte)rand.Next(1000, 10000);
            packet.Data.ImageID = UUID.Zero;

            ScriptDialogPacket.ButtonsBlock[] temp = new ScriptDialogPacket.ButtonsBlock[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                temp[i] = new ScriptDialogPacket.ButtonsBlock();
                temp[i].ButtonLabel = Utils.StringToBytes(buttons[i]);
            }
            packet.Buttons = temp;
            proxy.InjectPacket(packet, Direction.Incoming);
        }
    }
}