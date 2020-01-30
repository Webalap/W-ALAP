using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace QuartzScheduler.QuartzJobs
{
    public class PackageUpdateSyncJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            // For installing packages
            string InstallPaclageCommand = @"""C:\Program Files\MiKTeX 2.9\miktex\bin\x64\mpm"" --admin --verbose --package-level=complete --upgrade";
            // For Updating packages
            string UpdatePaclageCommand = @"""C:\Program Files\MiKTeX 2.9\miktex\bin\x64\mpm"" --admin --verbose --package-level=complete --upgrade";

            //// Execute the command synchronously.
            ExecuteCmd exe = new ExecuteCmd();
            //exe.ExecuteCommandSync(command);

            // Execute the command asynchronously.
            exe.ExecuteCommandAsync(InstallPaclageCommand);
            exe.ExecuteCommandAsync(UpdatePaclageCommand);

        }
    }
}