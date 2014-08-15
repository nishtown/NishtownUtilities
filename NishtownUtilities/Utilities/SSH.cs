using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tamir.SharpSsh;

namespace Nishtown.Utilities
{
    class SSH : IDisposable
    {
        private SshExec sshcon;
        public string server { get; set; }
        public string username { get; set; }
        public string password { get; set; }
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
                this.close();
                username = null;
                password = null;
                server = null;
                sshcon = null;
            }

            disposed = true;
        }


        public void connect()
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

            sshcon = new SshExec(server, username, password);
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

        public bool isConnected()
        {
            return bConnected;
        }

        public void connect(string user, string pass)
        {
            username = user;
            password = pass;
            connect();
        }

        public void connect(string host, string user, string pass)
        {
            server = host;
            username = user;
            password = pass;

            connect();
        }

        public void close()
        {
            if (isConnected())
            {
                sshcon.Close();
            }
        }

    }
}
