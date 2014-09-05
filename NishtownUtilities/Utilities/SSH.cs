using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;

namespace Nishtown.Utilities
{
    public class SSH : IDisposable
    {
        private SshClient sshcon;
        public string server { private get; set; }
        public string username { private get; set; }
        public string password { private get; set; }
        private bool bConnected = false;

        bool disposed = false;

        public SSH()
        {

        }

        public SSH(string user, string pass)
        {
            username = user;
            password = pass;
        }

        public SSH(string host, string user, string pass)
        {
            server = host;
            username = user;
            password = pass;

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                this.Close();
                if (IsConnected())
                    sshcon.Disconnect();
                username = null;
                password = null;
                server = null;
                sshcon.Dispose();
            }

            disposed = true;
        }

        public string Command(string cmd)
        {
            if (IsConnected())
            {
                SshCommand sshcmd = sshcon.RunCommand(cmd);
                return sshcmd.Result;
            }
            else
            {
                return null;
            }
        }

        public void Connect()
        {
            if (server == null)
            {
                throw new Exception("No server supplied");
            }
            if (username == null)
            {
                throw new Exception("No username supplied");
            }
            if (password == null)
            {
                throw new Exception("No password supplied");
            }

            sshcon = new SshClient(server, username, password);
            try
            {
                sshcon.Connect();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                bConnected = true;
            }
        }

        public bool IsConnected()
        {
            return bConnected;
        }

        public void Connect(string user, string pass)
        {
            username = user;
            password = pass;
            Connect();
        }

        public void Connect(string host, string user, string pass)
        {
            server = host;
            username = user;
            password = pass;

            Connect();
        }

        public void Close()
        {
            if (IsConnected())
            {
                sshcon.Disconnect();
            }
        }

    }
}
