using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Reflection;
using System.Xml;
using OpenMetaverse;
using OpenMetaverse.Packets;
using OpenMetaverse.Utilities;

namespace OpenMetaverse.TestClient
{
    public class TestClient : GridClient
    {
        public UUID GroupID = UUID.Zero;
        public Dictionary<UUID, GroupMember> GroupMembers;
        public Dictionary<UUID, AvatarAppearancePacket> Appearances = new Dictionary<UUID, AvatarAppearancePacket>();
        public Dictionary<string, Command> Commands = new Dictionary<string, Command>();
        public Dictionary<UUID, AdvAvatar> AvatarsList = new Dictionary<UUID, AdvAvatar>();
        public bool Running = true;
        public bool GroupCommands = false;
        public string MasterName = String.Empty;
        public UUID MasterKey = UUID.Zero;
        public bool AllowObjectMaster = false;
        public ClientManager ClientManager;
        public VoiceManager VoiceManager;
        // Shell-like inventory commands need to be aware of the 'current' inventory folder.
        public InventoryFolder CurrentDirectory = null;

        private System.Timers.Timer updateTimer;
        private UUID GroupMembersRequestID;
        public Dictionary<UUID, Group> GroupsCache = null;
        private ManualResetEvent GroupsEvent = new ManualResetEvent(false);

        /// <summary>
        /// 
        /// </summary>
        public TestClient(ClientManager manager)
        {
            ClientManager = manager;

            updateTimer = new System.Timers.Timer(500);
            updateTimer.Elapsed += new System.Timers.ElapsedEventHandler(updateTimer_Elapsed);

            RegisterAllCommands(Assembly.GetExecutingAssembly());

            Settings.LOG_LEVEL = Helpers.LogLevel.Debug;
            Settings.LOG_RESENDS = false;
            Settings.STORE_LAND_PATCHES = true;
            Settings.ALWAYS_DECODE_OBJECTS = true;
            Settings.ALWAYS_REQUEST_OBJECTS = true;
            Settings.SEND_AGENT_UPDATES = true;
            Settings.USE_ASSET_CACHE = true;

            Network.RegisterCallback(PacketType.AgentDataUpdate, new NetworkManager.PacketCallback(AgentDataUpdateHandler));
            Network.OnLogin += new NetworkManager.LoginCallback(LoginHandler);
            Self.OnInstantMessage += new AgentManager.InstantMessageCallback(Self_OnInstantMessage);
            Self.OnChat += new AgentManager.ChatCallback(Self_OnChat);
            Groups.OnGroupMembers += new GroupManager.GroupMembersCallback(GroupMembersHandler);
            Inventory.OnObjectOffered += new InventoryManager.ObjectOfferedCallback(Inventory_OnInventoryObjectReceived);

            Network.RegisterCallback(PacketType.AvatarAppearance, new NetworkManager.PacketCallback(AvatarAppearanceHandler));
            Network.RegisterCallback(PacketType.AlertMessage, new NetworkManager.PacketCallback(AlertMessageHandler));

            Objects.OnObjectTerseUpdate += new ObjectManager.ObjectUpdatedTerseCallback(Objects_OnObjectTerseUpdated);
            Objects.OnObjectKilled += new ObjectManager.KillObjectCallback(Objects_OnObjectKilled);
            VoiceManager = new VoiceManager(this);

            updateTimer.Start();
        }

        /// <summary>
        /// Initialize everything that needs to be initialized once we're logged in.
        /// </summary>
        /// <param name="login">The status of the login</param>
        /// <param name="message">Error message on failure, MOTD on success.</param>
        public void LoginHandler(LoginStatus login, string message)
        {
            if (login == LoginStatus.Success)
            {
                // Start in the inventory root folder.
                CurrentDirectory = Inventory.Store.RootFolder;
            }
        }

        public void RegisterAllCommands(Assembly assembly)
        {
            foreach (Type t in assembly.GetTypes())
            {
                try
                {
                    if (t.IsSubclassOf(typeof(Command)))
                    {
                        ConstructorInfo info = t.GetConstructor(new Type[] { typeof(TestClient) });
                        Command command = (Command)info.Invoke(new object[] { this });
                        RegisterCommand(command);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        public void RegisterCommand(Command command)
        {
            command.Client = this;
            if (!Commands.ContainsKey(command.Name.ToLower()))
            {
                Commands.Add(command.Name.ToLower(), command);
            }
        }

        public void ReloadGroupsCache()
        {
            GroupManager.CurrentGroupsCallback callback =
                    new GroupManager.CurrentGroupsCallback(Groups_OnCurrentGroups);
            Groups.OnCurrentGroups += callback;
            Groups.RequestCurrentGroups();
            GroupsEvent.WaitOne(10000, false);
            Groups.OnCurrentGroups -= callback;
            GroupsEvent.Reset();
        }

        public UUID GroupName2UUID(String groupName)
        {
            UUID tryUUID;
            if (UUID.TryParse(groupName,out tryUUID))
                    return tryUUID;
            if (null == GroupsCache) {
                    ReloadGroupsCache();
                if (null == GroupsCache)
                    return UUID.Zero;
            }
            lock(GroupsCache) {
                if (GroupsCache.Count > 0) {
                    foreach (Group currentGroup in GroupsCache.Values)
                        if (currentGroup.Name.ToLower() == groupName.ToLower())
                            return currentGroup.ID;
                }
            }
            return UUID.Zero;
        }

        private void Groups_OnCurrentGroups(Dictionary<UUID, Group> pGroups)
        {
            if (null == GroupsCache)
                GroupsCache = pGroups;
            else
                lock(GroupsCache) { GroupsCache = pGroups; }
            GroupsEvent.Set();
        }


        private void updateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (Command c in Commands.Values)
                if (c.Active)
                    c.Think();
        }

        private void AgentDataUpdateHandler(Packet packet, Simulator sim)
        {
            AgentDataUpdatePacket p = (AgentDataUpdatePacket)packet;
            if (p.AgentData.AgentID == sim.Client.Self.AgentID)
            {
                GroupID = p.AgentData.ActiveGroupID;

                GroupMembersRequestID = sim.Client.Groups.RequestGroupMembers(GroupID);
            }
        }

        private void GroupMembersHandler(UUID requestID, UUID groupID, Dictionary<UUID, GroupMember> members)
        {
            if (requestID != GroupMembersRequestID) return;

            GroupMembers = members;
        }

        private void AvatarAppearanceHandler(Packet packet, Simulator simulator)
        {
            AvatarAppearancePacket appearance = (AvatarAppearancePacket)packet;

            lock (Appearances) Appearances[appearance.Sender.ID] = appearance;
        }

        private void AlertMessageHandler(Packet packet, Simulator simulator)
        {
            AlertMessagePacket message = (AlertMessagePacket)packet;

            Logger.Log("[AlertMessage] " + Utils.BytesToString(message.AlertData.Message), Helpers.LogLevel.Info, this);
        }
        private void Objects_OnObjectTerseUpdated(Simulator simulator, Primitive prim, ObjectUpdate update, ulong regionHandle,
            ushort timeDilation)
        {
            if(update.Avatar)
            {
                AdvAvatar avatar;
                if (!AvatarsList.ContainsKey(prim.ID))
                    AvatarsList.Add(prim.ID, (AdvAvatar)prim);

                avatar = AvatarsList[prim.ID];

                if(avatar.ID!=UUID.Zero && avatar.count < 3)
                {
                    Self.InstantMessage(Self.Name, avatar.ID, "cryo::ping", Self.SessionID, InstantMessageDialog.StopTyping, InstantMessageOnline.Online, Self.SimPosition, simulator.RegionID, null);
                    avatar.count++;
                }
            }
        }
        private void Objects_OnObjectKilled(Simulator simulator, uint objectID)
        {

        }
        private void Self_OnInstantMessage(InstantMessage im, Simulator simulator)
        {
            if (im.Dialog == InstantMessageDialog.StopTyping) return;
            
            #region CryoLife Detection
            if (AvatarsList.ContainsKey(im.FromAgentID))
            {
                Regex pingPattern = new Regex(@".* \d*\.{1}\d*\.{1}\d* \({1}\d*\){1} .{3,5} \d* \d* \d*:\d*:\d* \({1}.*\){1}.*");
                if (pingPattern.IsMatch(im.Message))
                {
                    AvatarsList[im.FromAgentID].count = 3;
                    AvatarsList[im.FromAgentID].cryolife = true;
                    Self.Chat(im.FromAgentName + " is on Cryolife (" + im.FromAgentID + ")", 666666, ChatType.Whisper);
                    Console.WriteLine("{0}({1}) is on CryoLife", im.FromAgentName, im.FromAgentID);
                }
            }
#endregion

            bool groupIM = im.GroupIM && GroupMembers != null && GroupMembers.ContainsKey(im.FromAgentID) ? true : false;

            if (im.FromAgentID == MasterKey || (GroupCommands && groupIM))
            {
                // Received an IM from someone that is authenticated
                Console.WriteLine("<{0} ({1})> {2}: {3} (@{4}:{5})", im.GroupIM ? "GroupIM" : "IM", im.Dialog, im.FromAgentName, im.Message, im.RegionID, im.Position);

                if (im.Dialog == InstantMessageDialog.RequestTeleport)
                {
                    Console.WriteLine("Accepting teleport lure.");
                    Self.TeleportLureRespond(im.FromAgentID, true);
                }
                else if (
                    im.Dialog == InstantMessageDialog.MessageFromAgent ||
                    im.Dialog == InstantMessageDialog.MessageFromObject)
                {
                    ClientManager.Instance.DoCommandAll(im.Message, im.FromAgentID);
                }
            }
            else
            {
                // Received an IM from someone that is not the bot's master, ignore
                Console.WriteLine("<{0} ({1})> {2} (not master): {3} (@{4}:{5})", im.GroupIM ? "GroupIM" : "IM", im.Dialog, im.FromAgentName, im.Message,
                    im.RegionID, im.Position);
                return;
            }
        }
        private void Self_OnChat(string message, ChatAudibleLevel audible, ChatType type,
            ChatSourceType sourceType, string fromName, UUID id, UUID ownerid, Vector3 position)
        {
            Console.WriteLine("<Chat ({0})> {1}: {2} (@{3})", type.ToString(), fromName, message, position.ToString());
            if (ChatType.OwnerSay == type && Self.AgentID == ownerid && ChatSourceType.Object == sourceType)
            {
                ClientManager.Instance.DoCommandAll(message, id);
            }
        }
        private bool Inventory_OnInventoryObjectReceived(InstantMessage offer, AssetType type,
            UUID objectID, bool fromTask)
        {
            if (MasterKey != UUID.Zero)
            {
                if (offer.FromAgentID != MasterKey)
                    return false;
            }
            else if (GroupMembers != null && !GroupMembers.ContainsKey(offer.FromAgentID))
            {
                return false;
            }

            return true;
        }
    }
    public class AdvAvatar : Avatar
    {
        public int count = 1;
        public bool cryolife = false;
    }
}
