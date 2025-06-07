using finrv.ApiService.Application;
using finrv.Application.Interfaces;
using finrv.Application.Services.UserService.Dtos;
using finrv.Domain.Entities;
using finrv.Infra;
using finrv.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace finrv.Application.Services.UserService;

public class UserService(
    InvestimentDbContext context,
    ILogger<UserService> logger,
    RequestInfo requestInfo) : IUserService
{
    private const string CLASS_NAME = nameof(UserService);
    
    public async Task<Pagination<object, AllUsersResponseDto>> AllUsers(AllUsersRequestDto request)
    {
        logger.LogInformation("Starting | Class: {Class} | Method: {Method} | CorrelationId: {CorrelationId} | ClientType {Clienttype}.",
            CLASS_NAME, nameof(AllUsers), requestInfo.CorrelationId, requestInfo.ClientType);
        
        IQueryable<UserEntity> query = context.User.AsQueryable();

        var totalItems = await query.CountAsync();
        
        var users = await query
            .Skip((int)((request.Page - 1) * request.PageSize))
            .Take((int)request.PageSize)
            .Select(u => new AllUsersResponseDto(u.Id.ToString(), u.Name, u.Email))
            .ToListAsync();
        
        logger.LogInformation("Finished | Class: {Class} | Method: {Method} | CorrelationId: {CorrelationId} | ClientType {ClientType}.",
            CLASS_NAME, nameof(AllUsers), requestInfo.CorrelationId, requestInfo.ClientType);

        return new Pagination<object, AllUsersResponseDto>(users, request.Page, request.PageSize, (uint)totalItems, null);
    }
}