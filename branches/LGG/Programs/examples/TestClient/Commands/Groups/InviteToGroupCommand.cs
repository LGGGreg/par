/*
 * Copyright (c) 2009, Gregory Hendrickson (LordGregGreg Back)
 *All rights reserved.
 *
 *Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 *
    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
    * Neither the name of the  Gregory Hendrickson nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

 *THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

//First revision by LordGregGreg Back woot it works ish
//doesnt let you choose a role to send them to
//doesnt find hidden groups
using System;
using System.Collections.Generic;
using System.Threading;
using OpenMetaverse;
using OpenMetaverse.Packets;
using System.Text;

namespace OpenMetaverse.TestClient
{
    public class InviteToGroupCommand : Command
    {
        ManualResetEvent GetGroupsSearchEvent = new ManualResetEvent(false);
        private UUID avatarid;
        private UUID groupid = UUID.Zero;
        private UUID GqueryID = UUID.Zero;//for calbacks and stuff
        private UUID AqueryID = UUID.Zero;
        private string targetGroupName;
        private string targetAvatarName;
        ManualResetEvent NameSearchEvent = new ManualResetEvent(false);
        Dictionary<string, UUID> Name2Key = new Dictionary<string, UUID>();
        private TestClient testC;

        public InviteToGroupCommand(TestClient testClient)
        {
            testC = testClient;
            Name = "invitetogroup";
            Description = "invites and avatar to a group the bot is in. Usage: invitetogroup AvatarName|AvatarUUID , GroupName|UUIDGroupId";
            Category = CommandCategory.Groups;
        }

        public override string Execute(string[] args, UUID fromAgentID)
        {
            if (args.Length < 1)
                return Description;
            int place = 0;//for knowing what arg to grab


            if (!UUID.TryParse(args[place].Trim(), out avatarid))
            {
                //not a uuid, check if its a name
                testC.Avatars.OnAvatarNameSearch += new AvatarManager.AvatarNameSearchCallback(Avatars_OnAvatarNameSearch);

                targetAvatarName = args[place] + " " + args[++place];

                if (!Name2Key.ContainsKey(targetAvatarName.ToLower()))
                {
                    // Send the Query
                    Client.Avatars.RequestAvatarNameSearch(targetAvatarName, UUID.Random());

                    NameSearchEvent.WaitOne(20000, false);
                }

                if (Name2Key.ContainsKey(targetAvatarName.ToLower()))
                {
                    avatarid = Name2Key[targetAvatarName.ToLower()];
                }
                else
                {
                    return "Name lookup for " + targetAvatarName + " failed";
                }
            }

            if (!UUID.TryParse(args[++place].Trim(), out groupid))
            {
                //not a uuid, try group name
                targetGroupName = args[place++];
                for (; place < args.Length; place++)
                    targetGroupName += " " + args[place];

                DirectoryManager.DirGroupsReplyCallback callback = new DirectoryManager.DirGroupsReplyCallback(Directory_OnDirGroupsReply);
                Client.Directory.OnDirGroupsReply += callback;
                GqueryID = Client.Directory.StartGroupSearch(DirectoryManager.DirFindFlags.Groups, targetGroupName, 0);

                GetGroupsSearchEvent.WaitOne(60000, false);

                Client.Directory.OnDirGroupsReply -= callback;
                GetGroupsSearchEvent.Reset();


                if (groupid == UUID.Zero)
                {
                    return "Unable to obtain UUID for group ";
                }
            }
            //we now got everything in uuid form woot
            //TODO check if this failed, like if avatar isnt in the group or has permissions
            //TODO get better roles o.0
            Client.Groups.Invite(groupid, new List<UUID>(new UUID[] { UUID.Zero }), avatarid);
            
            return "Invited to the group "+targetGroupName;
        }

        void Avatars_OnAvatarNameSearch(UUID queryID, Dictionary<UUID, string> avatars)
        {
            foreach (KeyValuePair<UUID, string> kvp in avatars)
            {
                if (kvp.Value.ToLower() == this.targetAvatarName.ToLower())
                {
                    Name2Key[this.targetAvatarName.ToLower()] = kvp.Key;
                    NameSearchEvent.Set();
                    return;
                }
            }
        }
        void Directory_OnDirGroupsReply(UUID queryid, List<DirectoryManager.GroupSearchData> matchedGroups)
        {
            if (GqueryID == queryid)
            {
                GqueryID = UUID.Zero;
                if (matchedGroups.Count < 1)
                {
                    Console.WriteLine("ERROR: Got an empty reply");
                }
                else
                {
                    if (matchedGroups.Count > 1)
                    {
                        /* A.Biondi 
                         * The Group search doesn't work as someone could expect...
                         * It'll give back to you a long list of groups even if the 
                         * searchText (groupName) matches esactly one of the groups 
                         * names present on the server, so we need to check each result.
                         * UUIDs of the matching groups are written on the console.
                         */
                        Console.WriteLine("Matching groups are:\n");
                        foreach (DirectoryManager.GroupSearchData groupRetrieved in matchedGroups)
                        {
                            Console.WriteLine(groupRetrieved.GroupName + "\t\t\t(" +
                                Name + " UUID " + groupRetrieved.GroupID.ToString() + ")");

                            if (groupRetrieved.GroupName.ToLower() == targetGroupName.ToLower())
                            {
                                groupid = groupRetrieved.GroupID;
                                targetGroupName = groupRetrieved.GroupName;
                                break;
                            }
                        }
                        //if (string.IsNullOrEmpty(resolvedGroupName))
                          //  resolvedGroupName = "Ambiguous name. Found " + matchedGroups.Count.ToString() + " groups (UUIDs on console)";
                    }

                }
                GetGroupsSearchEvent.Set();
            }
        }
    }
}
