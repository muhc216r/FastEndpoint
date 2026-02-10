//using System.Text.Json;
//using Microsoft.EntityFrameworkCore.Infrastructure;

//namespace FastEndPoint.Feature.Job;

//class JobStorageProvider : IJobStorageProvider<JobRecord>, IJobResultProvider
//{
//    readonly PooledDbContextFactory<JobDbContext> _dbPool;
//    public JobStorageProvider()
//    {
//        var options = new DbContextOptionsBuilder<JobDbContext>()
//            .UseSqlServer(AppConfig.Connection)
//            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
//            .Options;
//        _dbPool = new(options);

//        using var db = _dbPool.CreateDbContext();
//        db.Database.EnsureCreated();
//    }

//    public async Task StoreJobAsync(JobRecord job, CancellationToken cancellation)
//    {
//        await using var db = await _dbPool.CreateDbContextAsync(cancellation);
//        await db.AddAsync(job, cancellation);
//        await db.SaveChangesAsync(cancellation);
//    }

//    public async Task<IEnumerable<JobRecord>> GetNextBatchAsync(PendingJobSearchParams<JobRecord> pendingJob)
//    {
//        await using var db = await _dbPool.CreateDbContextAsync(pendingJob.CancellationToken);

//        return await db.Jobs
//            .Where(pendingJob.Match)
//            .OrderBy(x => x.ExecuteAfter)
//            .Take(pendingJob.Limit)
//            .ToListAsync(pendingJob.CancellationToken);
//    }

//    public async Task MarkJobAsCompleteAsync(JobRecord job, CancellationToken cancellation)
//    {
//        await using var db = await _dbPool.CreateDbContextAsync(cancellation);

//        await db.Jobs.Where(x => x.Id == job.Id)
//            .ExecuteUpdateAsync(x => x.SetProperty(x => x.IsComplete, true), cancellation);
//    }

//    public async Task CancelJobAsync(Guid trackingId, CancellationToken cancellation)
//    {
//        await using var db = await _dbPool.CreateDbContextAsync(cancellation);

//        await db.Jobs.Where(x => x.TrackingId == trackingId)
//            .ExecuteUpdateAsync(x => x.SetProperty(x => x.IsComplete, true), cancellation);
//    }

//    public async Task OnHandlerExecutionFailureAsync(JobRecord job, Exception exception, CancellationToken cancellation)
//    {
//        await using var db = await _dbPool.CreateDbContextAsync(cancellation);

//        var next = DateTime.UtcNow.AddMinutes(1);
//        var now = DateTime.UtcNow;

//        var error = exception.ToString(); if (error.Length > 20000) error = error[..20000];

//        await db.Jobs
//            .Where(x => x.Id == job.Id)
//            .ExecuteUpdateAsync(x => x
//                .SetProperty(y => y.ExecuteAfter, next)
//                .SetProperty(y => y.LastFailureOn, now)
//                .SetProperty(y => y.LastError, error)
//                .SetProperty(y => y.FailureCount, z => z.FailureCount + 1)
//                , cancellation);
//    }

//    public async Task PurgeStaleJobsAsync(StaleJobSearchParams<JobRecord> p)
//    {
//        await using var db = await _dbPool.CreateDbContextAsync(p.CancellationToken);
//        await db.Jobs.Where(p.Match).ExecuteDeleteAsync(p.CancellationToken);
//    }

//    public async Task StoreJobResultAsync<TResult>(Guid trackingId, TResult result, CancellationToken cancellation)
//    {
//        await using var db = await _dbPool.CreateDbContextAsync(cancellation);
//        var resultJson = JsonSerializer.Serialize(result);
//        await db.Jobs.Where(x => x.TrackingId == trackingId)
//            .ExecuteUpdateAsync(x => x.SetProperty(y => y.ResultJson, resultJson), cancellation);
//    }

//    public async Task<TResult?> GetJobResultAsync<TResult>(Guid trackingId, CancellationToken cancellation)
//    {
//        await using var db = await _dbPool.CreateDbContextAsync(cancellation);

//        var resultJson = await db.Jobs.Where(x => x.TrackingId == trackingId)
//            .Select(x => x.ResultJson).FirstOrDefaultAsync(cancellation);

//        return resultJson is null ? default : JsonSerializer.Deserialize<TResult>(resultJson);
//    }
//}
