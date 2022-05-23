using JudgeApp.Core.Entities;
using JudgeApp.Core.Utils;

namespace JudgeApp.Core.Services.Abstractions;

public interface IStatusRepository
{
    Task<ActionResponse<List<Status>>> GetStatuses();
    Task<ActionResponse<Status>> GetActiveStatus();
    Task<ActionResponse<Status>> CreateStatus(string name);
    Task<ActionResponse<Status>> EditStatus(Status status);
    Task<ActionResponse<Status>> DeleteStatus(Guid id);

    Task<ActionResponse<Status>> GetById(Guid id);
}