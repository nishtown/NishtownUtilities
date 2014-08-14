﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Reflection;
using System.ComponentModel;

namespace Nishtown.Utilities
{
    public class NetworkImpersonationContext : IDisposable
    {
        private readonly WindowsIdentity _identity;
        private readonly WindowsImpersonationContext _impersonationContext;
        private readonly IntPtr _token;
        private bool _disposed;

        public NetworkImpersonationContext(string domain, string userName, string password)
        {
            if (!LogonUser(userName, domain, password, 9, 0, out _token))
                throw new Win32Exception();
            _identity = new WindowsIdentity(_token);
            try
            {
                _impersonationContext = _identity.Impersonate();
            }
            catch
            {
                _identity.Dispose();
                throw;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        #endregion

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser(
            string lpszUsername,
            string lpszDomain,
            string lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            out IntPtr phToken
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hHandle);

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            _disposed = true;

            if (disposing)
            {
                _impersonationContext.Dispose();
                _identity.Dispose();
            }

            if (!CloseHandle(_token))
                throw new Win32Exception();
        }

        ~NetworkImpersonationContext()
        {
            Dispose(false);
        }
    }
}
