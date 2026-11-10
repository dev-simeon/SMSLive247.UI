namespace SMSLive247.UI.Services;

public sealed class MaintenanceNoticeClient
{
    private static readonly MaintenanceNotice Notice = new()
    {
        Id = "maintenance-2026-11-07",
        Title = "Scheduled maintenance",
        Message = "Scheduled downtime: SMSLive247 will be unavailable on November 7 from 1:00 AM to 4:00 AM (WAT).",
        PublishedAtUtc = new DateTimeOffset(2026, 11, 6, 0, 0, 0, TimeSpan.Zero),
        StartsAtUtc = new DateTimeOffset(2026, 11, 7, 0, 0, 0, TimeSpan.Zero),
        EndsAtUtc = new DateTimeOffset(2026, 11, 7, 3, 0, 0, TimeSpan.Zero)
    };

    // Mock implementation shaped like the planned active-maintenance API response.
    public Task<MaintenanceNotice?> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var now = DateTimeOffset.UtcNow;
        MaintenanceNotice? active = now >= Notice.PublishedAtUtc && now < Notice.EndsAtUtc
            ? Notice
            : null;

        return Task.FromResult(active);
    }
}

public sealed class MaintenanceNotice
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTimeOffset PublishedAtUtc { get; set; }
    public DateTimeOffset StartsAtUtc { get; set; }
    public DateTimeOffset EndsAtUtc { get; set; }
}
