using System.Text.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseApi.Feature.Job;
public class JobRecord : IJobStorageRecord, IJobResultStorage
{
    public Guid Id { get; set; }

    public string QueueId { get; set; }
    string IJobStorageRecord.QueueID
    {
        get => QueueId;
        set => QueueId = value;
    }

    public Guid TrackingId { get; set; }
    Guid IJobStorageRecord.TrackingID
    {
        get => TrackingId;
        set => TrackingId = value;
    }

    public DateTime ExecuteAfter { get; set; }
    public DateTime ExpireOn { get; set; }
    public bool IsComplete { get; set; }

    [NotMapped]
    public object Command { get; set; }
    public string CommandJson { get; set; }

    TCommand IJobStorageRecord.GetCommand<TCommand>() => JsonSerializer.Deserialize<TCommand>(CommandJson)!;

    void IJobStorageRecord.SetCommand<TCommand>(TCommand command) => CommandJson = JsonSerializer.Serialize(command);

    [NotMapped]
    public object? Result { get; set; }
    public string? ResultJson { get; set; }

    TResult? IJobResultStorage.GetResult<TResult>() where TResult : default
        => ResultJson is not null ? JsonSerializer.Deserialize<TResult>(ResultJson) : default;

    void IJobResultStorage.SetResult<TResult>(TResult result) => ResultJson = JsonSerializer.Serialize(result);

    public int FailureCount { get; set; }
    public DateTime? LastFailureOn { get; set; }
    public string? LastError { get; set; }
}

