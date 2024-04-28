using Rebex.Net;

namespace BGM_Express.Core.Utils
{
    public static class RebexUtils
    {
        public static Sftp CreateRebexConnection()
        {
            var sftp = new Sftp();
            sftp.Connect("192.168.56.1");
            sftp.Login("tester", "password");

            return sftp;
        }
    }
}
