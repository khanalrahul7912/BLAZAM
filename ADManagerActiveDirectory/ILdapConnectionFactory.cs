using ADManager.ActiveDirectory.Data;
using ADManager.Database.Models;
using System.DirectoryServices.Protocols;

namespace ADManager.ActiveDirectory
{
    public interface ILdapConnectionFactory:IDisposable
    {
        int Count { get; }
        AppEvent? OnCountChanged { get; set; }

        void ClearPool();
        AppLdapConnection? Connect(ADSettings settings);
        bool ConnectWithLdaps(ADSettings settings, out LdapConnection? connection);
        bool ConnectWithStartTls(ADSettings settings, out LdapConnection? connection);
        bool StopTls(LdapConnection currentConnection);
    }
}