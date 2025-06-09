using System.Collections.Concurrent;
using finrv.Domain.Entities;
using finrv.Infra;
using finrv.QuotationWorkerService.Abstraction;
using finrv.QuotationWorkerService.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace finrv.QuotationWorkerService.Listeners;

public class QuotationUpdateListener : IListener<QuotationUpdateEvent>
{
    private readonly string? _kafkaTopic;
    private const string CLASS_NAME = nameof(QuotationUpdateListener);
    
    private readonly ILogger<QuotationUpdateListener> _logger;
    private readonly InvestimentDbContext _dbContext;
    private static readonly ConcurrentDictionary<Guid, bool> _processedEvents = new ConcurrentDictionary<Guid, bool>();

    public QuotationUpdateListener(
        ILogger<QuotationUpdateListener> logger,
        InvestimentDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }
    
    public async Task ProcessMessageAsync(QuotationUpdateEvent message)
    {
        _logger.LogInformation("Starting | Processing event | Class: {ClassName} | Method: {Method} | Event ID: {EventId} | CorrelationId {CorrelationId}",
            CLASS_NAME, nameof(ProcessMessageAsync), message?.Id, message?.CorrelationId);
        
        if (message == null || _processedEvents.ContainsKey(message.Id))
        {
            _logger.LogInformation("Event with Id '{EventId}' already processed or is null. Skipping.", message?.Id);
            return;
        }
        
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var hasTicker = await _dbContext.Asset.FirstOrDefaultAsync(a => a.Ticker == message.Ticker);
            if (hasTicker == null)
            {
                var asset = await _dbContext.Asset.AddAsync(new AssetEntity(message.Ticker, message.Name));
                _dbContext.Quotation.Add(new QuotationEntity(asset.Entity, message.Price, message.LatestUpdate));
                await FinishTransaction(transaction, message);
                return;
            }
            
            var query = _dbContext.Quotation
                .Include(q => q.Asset)
                .AsQueryable();
        
            query = query
                .Where(q => q.Asset.Ticker == message.Ticker)
                .OrderByDescending(q => q.UpdatedAt ?? q.CreatedAt);
        
            var quotation = await query.FirstOrDefaultAsync();

            if (quotation is not null && quotation.Price != message.Price)
            {
                _dbContext.Quotation.Add(new QuotationEntity(quotation.Asset, message.Price, message.LatestUpdate));
                await FinishTransaction(transaction, message);
                return;
            }
        
            quotation?.UpdateQuotation(message.LatestUpdate);
            _dbContext.Quotation.Update(quotation);
            await FinishTransaction(transaction, message);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error processing event | Class: {ClassName} | Method: {Method} | Event ID: {EventId} | CorrelationId {CorrelationId}",
                CLASS_NAME, nameof(ProcessMessageAsync), message?.Id, message?.CorrelationId);
            throw;
        }
    }
    
    public void ProcessErrorAsync(QuotationUpdateEvent rawMessage, Exception exception)
    {
        _logger.LogError(exception, "Error handling message. Message: {Message}", rawMessage);
    }
    
    public async Task FinishTransaction(IDbContextTransaction transaction, QuotationUpdateEvent message)
    {
        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
        if (!_processedEvents.TryAdd(message.Id, true))
        {
            _logger.LogWarning("Concurrent add detected for Event ID '{EventId}'. Already processed.", message.Id);
            return;
        }
            
        _logger.LogInformation("Finished | Processing event | Class: {ClassName} | Method: {Method} | Event ID: {EventId} | CorrelationId {CorrelationId}",
            CLASS_NAME, nameof(ProcessMessageAsync), message?.Id, message?.CorrelationId);
    }
}