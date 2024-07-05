
using Job_Scheduler_webAPI.Models;
using Quartz;
using Quartz.Spi;

namespace Job_Scheduler_webAPI.Schedular
{
    public class MySchedular : IHostedService
    {

        public IScheduler Scheduler { get; set; }

        public readonly IJobFactory jobFactory;

        public readonly JobMetadata jobMetadata;

        public readonly ISchedulerFactory schedulerFactory;


        public MySchedular(IJobFactory jobFactory, JobMetadata jobMetadata, ISchedulerFactory schedulerFactory)
        {
            this.jobFactory = jobFactory;
            this.jobMetadata = jobMetadata;
            this.schedulerFactory = schedulerFactory;
        }

       public async Task StartAsync(CancellationToken cancellationToken)
        {

            //create Scheduler
            Scheduler = await schedulerFactory.GetScheduler();
            Scheduler.JobFactory = jobFactory;

            //create Job
            IJobDetail jobDetail = CreateJob(jobMetadata);

            //create trigger
            ITrigger trigger=CreateTrigger(jobMetadata);

            //Shedule job
            await Scheduler.ScheduleJob(jobDetail, trigger,cancellationToken);

            //start the scheduler
            await Scheduler.Start(cancellationToken);
        }

        private ITrigger CreateTrigger(JobMetadata jobMetadata)
        {
           return TriggerBuilder.Create()
                .WithIdentity(jobMetadata.JobId.ToString())
                .WithCronSchedule(jobMetadata.CronExpression)
                .WithDescription(jobMetadata.JobName)
                .Build();
        }

        private IJobDetail CreateJob(JobMetadata jobMetadata)
        {
            return JobBuilder.Create(jobMetadata.JobType)
                .WithIdentity(jobMetadata.JobId.ToString())
                .WithDescription(jobMetadata.JobName)
                .Build();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler.Shutdown();
        }
    }
}
