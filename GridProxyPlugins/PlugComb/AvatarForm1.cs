using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using OpenMetaverse;

namespace PubCombN
{
    public partial class FormAvatars : Form
    {
        public AvatarTracker a;
        public FormAvatars(AvatarTracker at)
        {
            a = at;
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
        }
        public delegate void AddAvatarCallback(PubComb.Aux_Avatar avatar);
        public void AddAvatar1(PubComb.Aux_Avatar avatar)
        {
            dataGridView1.Rows.Add(new string[] {avatar.Name, avatar.UUID.ToString(), avatar.sim_IP.ToString(), avatar.LocalID.ToString(), avatar.Position.ToString()});
        }
        public void AddAvatar(PubComb.Aux_Avatar avatar)
        {
            this.dataGridView1.Invoke(new AddAvatarCallback(this.AddAvatar1), 
            new object[]{avatar});
        }
        public delegate void RemAvatarCallback(PubComb.Aux_Avatar avatar);
        public void RemoveAvatar(PubComb.Aux_Avatar avatar)
        {
            this.dataGridView1.Invoke(new RemAvatarCallback(this.RemoveAvatar1),
            new object[] { avatar });
        }
        public void RemoveAvatar1(PubComb.Aux_Avatar avatar)
        {
            lock (dataGridView1.Rows)
            {
                int t = dataGridView1.Rows.Count;
                for (int i = 0; i < t; i++)
                {
                    if ((string)dataGridView1.Rows[i].Cells["UUID"].Value == avatar.UUID.ToString())
                    {
                        dataGridView1.Rows.RemoveAt(i);
                        return;
                    }
                }
            }
        }

        public delegate void UpdatevatarCallback(PubComb.Aux_Avatar avatar);
        public void UpdateAvatar(PubComb.Aux_Avatar avatar)
        {
            this.dataGridView1.Invoke(new UpdatevatarCallback(this.UpdateAvatar1),
            new object[] { avatar });
        }
        public void UpdateAvatar1(PubComb.Aux_Avatar avatar)
        {
            lock (dataGridView1.Rows)
            {
                int t = dataGridView1.Rows.Count;
                for (int i = 0; i < t; i++)
                {
                    if ((string)dataGridView1.Rows[i].Cells["UUID"].Value == avatar.UUID.ToString())
                    {
                        dataGridView1.Rows[i].Cells["Position"].Value = avatar.Position.ToString();
                        return;
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
