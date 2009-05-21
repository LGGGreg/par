using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Net;
using OpenMetaverse;
using OpenMetaverse.Packets;
using OpenMetaverse.StructuredData;
using GridProxy;

public class NeilSay : ProxyPlugin
{
    // Fields
    private ProxyFrame frame;
    private Proxy proxy;

    // Methods
    public NeilSay(ProxyFrame frame)
    {
        this.frame = frame;
        this.proxy = frame.proxy;
    }

    private Packet BlockPacket(Packet packet, IPEndPoint endPoint)
    {
        return null;
    }

    private Packet ChatPacketOut(Packet packet, IPEndPoint endPoint)
    {
        ChatFromViewerPacket packet2 = (ChatFromViewerPacket) packet;
        if (packet2.ChatData.Type == 1)
        {
            packet2.ChatData.Type = 0;
            return packet2;
        }
        return null;
    }

    private void CmdShoutOff(string[] words)
    {
        this.SayToUser("Shout Plugin OFF");
        this.proxy.RemoveDelegate(PacketType.ChatFromViewer, Direction.Outgoing, new PacketDelegate(this.CrapPacketOut));
    }

    private void CmdShoutOn(string[] words)
    {
        this.SayToUser("Shout Plugin ON");
        this.proxy.AddDelegate(PacketType.ChatFromViewer, Direction.Outgoing, new PacketDelegate(this.CrapPacketOut));
    }

    private void CmdWhisperOff(string[] words)
    {
        this.SayToUser("Whisper Plugin OFF");
        this.proxy.RemoveDelegate(PacketType.ChatFromViewer, Direction.Outgoing, new PacketDelegate(this.ChatPacketOut));
    }

    private void CmdWhisperOn(string[] words)
    {
        this.SayToUser("Whisper Plugin ON");
        this.proxy.AddDelegate(PacketType.ChatFromViewer, Direction.Outgoing, new PacketDelegate(this.ChatPacketOut));
    }

    private Packet CrapPacketOut(Packet packet, IPEndPoint endPoint)
    {
        ChatFromViewerPacket packet2 = (ChatFromViewerPacket) packet;
        if (packet2.ChatData.Type == 1)
        {
            packet2.ChatData.Type = 2;
            return packet2;
        }
        return null;
    }

    public override void Init()
    {
        this.frame.AddCommand("/whisper-on", new ProxyFrame.CommandDelegate(this.CmdWhisperOn));
        this.frame.AddCommand("/whisper-off", new ProxyFrame.CommandDelegate(this.CmdWhisperOff));
        this.frame.AddCommand("/shout-on", new ProxyFrame.CommandDelegate(this.CmdShoutOn));
        this.frame.AddCommand("/shout-off", new ProxyFrame.CommandDelegate(this.CmdShoutOff));
        this.frame.AddCommand("/phantom-on", new ProxyFrame.CommandDelegate(this.PhantomOn));
        this.frame.AddCommand("/phantom-off", new ProxyFrame.CommandDelegate(this.PhantomOff));
        this.frame.AddCommand("/sound-on", new ProxyFrame.CommandDelegate(this.SoundOn));
        this.frame.AddCommand("/sound-off", new ProxyFrame.CommandDelegate(this.SoundOff));
    }

    private void PhantomOff(string[] words)
    {
        this.SayToUser("phantom Plugin OFF");
        this.proxy.RemoveDelegate(PacketType.AgentUpdate, Direction.Outgoing, new PacketDelegate(this.BlockPacket));
    }

    private void PhantomOn(string[] words)
    {
        this.SayToUser("phantom Plugin ON");
        this.proxy.AddDelegate(PacketType.AgentUpdate, Direction.Outgoing, new PacketDelegate(this.BlockPacket));
    }

    private void SayToUser(string message)
    {
        ChatFromSimulatorPacket packet = new ChatFromSimulatorPacket();
        packet.ChatData.FromName = Utils.StringToBytes("neilsay Plugin Loaded copyright of neil1234 Mimulus");
        packet.ChatData.SourceID = UUID.Random();
        packet.ChatData.OwnerID = this.frame.AgentID;
        packet.ChatData.SourceType = 2;
        packet.ChatData.ChatType = 1;
        packet.ChatData.Audible = 1;
        packet.ChatData.Position = new Vector3(0f, 0f, 0f);
        packet.ChatData.Message = Utils.StringToBytes(message);
        this.proxy.InjectPacket(packet, Direction.Incoming);
    }

    private void SoundOff(string[] words)
    {
        this.SayToUser("Sound Plugin Off");
        this.proxy.AddDelegate(PacketType.SoundTrigger, Direction.Incoming, new PacketDelegate(this.BlockPacket));
    }

    private void SoundOn(string[] words)
    {
        this.SayToUser("Sound Plugin On");
        this.proxy.RemoveDelegate(PacketType.SoundTrigger, Direction.Incoming, new PacketDelegate(this.BlockPacket));
    }
}

 
