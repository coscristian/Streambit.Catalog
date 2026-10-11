using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Streambit.Application.Sync;
using Streambit.Contracts.Sync;

namespace Streambit.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/sync-logs")]
[ApiController]
public class SyncLogsController : ControllerBase
{
    private readonly ISyncLogService _logs;

    public SyncLogsController(ISyncLogService logs)
    {
        _logs = logs;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SyncLogResponse>>> GetAll(
        [FromQuery] string? entity,
        [FromQuery] bool? succeeded,
        [FromQuery] Guid? runId,
        [FromQuery] int limit = SyncLogService.DefaultLimit,
        CancellationToken cancellationToken = default)
    {
        var logs = await _logs.ListAsync(entity, succeeded, runId, limit, cancellationToken);
        return Ok(logs);
    }
}
