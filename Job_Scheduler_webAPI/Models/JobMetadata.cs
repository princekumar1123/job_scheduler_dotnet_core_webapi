namespace Job_Scheduler_webAPI.Models
{
    public class JobMetadata
    {
        public Guid JobId { get; set; }

        public Type JobType { get; }
        public string JobName { get; }

        public string CronExpression { get; }

        public JobMetadata(Guid id, Type jobType, string jobName, string cronExpression) 
        {
            JobId = id;

            JobType = jobType;
                
            JobName = jobName;
                
            CronExpression = cronExpression;
        }


    }
}
