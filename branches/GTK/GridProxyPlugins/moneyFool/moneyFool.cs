using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using OpenMetaverse;
using OpenMetaverse.Packets;
using GridProxy;

public class moneyFool : ProxyPlugin
{
    private ProxyFrame frame;
    private Proxy proxy;
    public static bool Enabled = false;

    public moneyFool(ProxyFrame frame)
    {
        this.frame = frame;
        this.proxy = frame.proxy;
        this.proxy.AddDelegate(PacketType.ScriptDialogReply, Direction.Outgoing, new PacketDelegate(OutDialogFromViewer));
        this.proxy.AddDelegate(PacketType.ChatFromViewer, Direction.Outgoing, new PacketDelegate(OutChatFromViewerHandler));
        this.proxy.AddDelegate(PacketType.MoneyBalanceReply,Direction.Incoming,new PacketDelegate(InMoneyReply));
       
    }

    public override void Init()
    {
        SayToUser("/me loaded, say /money-menu for menu");
    }
    private void SayToUser(string message)
    {

        ChatFromSimulatorPacket packet = new ChatFromSimulatorPacket();
        packet.ChatData.FromName = Utils.StringToBytes("Money Fool plugin");
        packet.ChatData.SourceID = UUID.Random();
        packet.ChatData.OwnerID = frame.AgentID;
        packet.ChatData.SourceType = (byte)2;
        packet.ChatData.ChatType = (byte)1;
        packet.ChatData.Audible = (byte)1;
        packet.ChatData.Position = new Vector3(0, 0, 0);
        packet.ChatData.Message = Utils.StringToBytes(message);
        proxy.InjectPacket(packet, Direction.Incoming);
    }
    private void FoolsMoney(int amount)
    {
        MoneyBalanceReplyPacket packet = new MoneyBalanceReplyPacket();
        packet.MoneyData.MoneyBalance = amount;
        packet.MoneyData.Description = Utils.StringToBytes("Can has cheezeburger?");
        packet.MoneyData.TransactionID = UUID.Zero;
        packet.MoneyData.TransactionSuccess = true;
        packet.MoneyData.SquareMetersCredit = 0;
        packet.MoneyData.SquareMetersCommitted = 0;
        packet.MoneyData.AgentID = frame.AgentID;
        //SayToUser(frame.AgentID.ToString());
        proxy.InjectPacket(packet, Direction.Incoming);
    }
    private void SendUserAlert(string message)
    {
        AlertMessagePacket packet = new AlertMessagePacket();
        packet.AlertData.Message = Utils.StringToBytes(message);

        proxy.InjectPacket(packet, Direction.Incoming);

    }
    private void RequestObjects(byte[] objects)
    {
        RequestMultipleObjectsPacket packet = new RequestMultipleObjectsPacket();
        packet.AgentData.AgentID = frame.AgentID;
        RequestMultipleObjectsPacket.ObjectDataBlock[] temp = new RequestMultipleObjectsPacket.ObjectDataBlock[4];
        for (int i = 0; i < objects.Length; i++)
        {
            temp[i] = new RequestMultipleObjectsPacket.ObjectDataBlock();
            temp[i].ID = objects[i];
            temp[i].CacheMissType = (byte)1;
        }
        packet.ObjectData = temp;
        proxy.InjectPacket(packet, Direction.Outgoing);
    }
    private void RequestMoney()
    {
        MoneyBalanceRequestPacket packet = new MoneyBalanceRequestPacket();
        packet.AgentData.AgentID = frame.AgentID;
        packet.AgentData.SessionID = frame.SessionID;
        packet.MoneyData.TransactionID = UUID.Zero;
        proxy.InjectPacket(packet, Direction.Outgoing);
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
    private Packet OutDialogFromViewer(Packet packet, IPEndPoint sim)
    {
        ScriptDialogReplyPacket DialogFromViewer = (ScriptDialogReplyPacket)packet;
        string message =Utils.BytesToString(DialogFromViewer.Data.ButtonLabel).Trim().ToLower();
        if (message.Contains("m-"))
        {
            if (message.Contains("fake"))
            {
                Enabled = true;
                FoolsMoney(1000000);
                SendUserAlert("You now have 1000000 linden.");
            }
            else
            {
                Enabled = false;
                RequestMoney();
                SendUserAlert("linden set to actual ammount");
            }
            return null;
        }
        else if (message.Contains("money-"))
        {
            SendUserDialog("Money", "Emulation", "Menu", "What would you like to do?", new String[2] { "M-Fake Money", "M-Real Money" });
            return null;
        }
        return packet;
    }
    private Packet OutChatFromViewerHandler(Packet packet, IPEndPoint sim)
    {
        ChatFromViewerPacket ChatFromViewer = (ChatFromViewerPacket)packet;
        string message = Utils.BytesToString(ChatFromViewer.ChatData.Message).ToLower();
        if (message.Contains("money-"))
        {
            SendUserDialog("Money", "Emulation", "Menu", "What would you like to do?", new String[2] { "M-Fake Money", "M-Real Money" });
            return null;
        }
        return packet;
    }
    private Packet InMoneyReply(Packet packet, IPEndPoint sim)
    {
        //MoneyBalanceReplyPacket MoneyReply = (MoneyBalanceReplyPacket)packet;
        if (Enabled)
            return null;
        else
            return packet;
    }
    
}