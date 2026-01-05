namespace FastEndpoint;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }
    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Properties<string>().HaveMaxLength(500);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // logging
        //var userType = modelBuilder.Model.FindEntityType(typeof(User));
        //var nameProp = userType?.FindProperty(nameof(Feature.User.Name));
        //var finalMax = nameProp?.GetMaxLength();
        //Console.WriteLine($"User.Name MaxLength = {finalMax}");
    }
}
