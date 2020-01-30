using Quartz;
using Quartz.Impl;
using QuartzScheduler.QuartzJobs;

namespace QuartzScheduler.QuartzSniffers
{

    public static class DailySniffer
    {

        public static void Activate()
        {
            ISchedulerFactory factory = new StdSchedulerFactory();

            // get and start scheduler
            IScheduler scheduler = factory.GetScheduler();
            scheduler.Start();
            IJobDetail PackageUpdateSyncJob = JobBuilder.Create<PackageUpdateSyncJob>()
                         .WithIdentity("PackageUpdateSyncJob", "Daily")
                         .Build();

            // Trigger the job to run now, and then every 24 hours
            ITrigger PackageUpdateSyncTrigger = TriggerBuilder.Create()
                                             .WithIdentity("PackageUpdateSyncTrigger", "Daily")
                                            //.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(23, 55)) // Everyday at 5 min befor midnight
                                            //.ForJob(transferPendingCreditsToProductCreditsJob)
                                            //.Build();
                                            .WithSimpleSchedule(x => x
                                            .WithIntervalInMinutes(15) // Everyday , every 15 minutes 
                                            .RepeatForever())
                                            .Build();
            scheduler.ScheduleJob(PackageUpdateSyncJob, PackageUpdateSyncTrigger);
        }
    }
}
