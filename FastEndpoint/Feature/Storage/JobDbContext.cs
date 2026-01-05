namespace BaseApi.Feature.Job;
public class JobDbContext(DbContextOptions<JobDbContext> options) : DbContext(options)
{
    public DbSet<JobRecord> Jobs => Set<JobRecord>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        var entity = builder.Entity<JobRecord>();

        entity.ToTable("JobRecords");
        entity.HasKey(x => x.Id);

        entity.Property(x => x.QueueId).IsRequired().HasMaxLength(128);
        entity.Property(x => x.CommandJson).IsRequired().HasColumnType("nvarchar(max)");
        entity.Property(x => x.ResultJson).HasColumnType("nvarchar(max)");

        entity.HasIndex(x => new { x.IsComplete, x.ExecuteAfter });
        entity.HasIndex(x => x.QueueId);
        entity.HasIndex(x => x.TrackingId);

        entity.Property(x => x.LastError).HasColumnType("nvarchar(max)");
        entity.HasIndex(x => x.IsComplete);
        entity.HasIndex(x => x.ExecuteAfter);
    }
}