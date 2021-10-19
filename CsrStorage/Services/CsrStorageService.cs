using CsrStorage.Data;
using CsrStorage.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CsrStorage.Services;

public class CsrStorageService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<CsrStorageService> _logger;
    public event Action<CertificateRequestEntity>? CsrAdded;
    public event Action<CertificateRequestEntity>? CsrRemoved ;
    
    public CsrStorageService(IServiceScopeFactory scopeFactory, ILogger<CsrStorageService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task<List<CertificateRequestEntity>> Find()
    {
        using var scope = _scopeFactory.CreateScope() ;
        await using var dbContext = scope.ServiceProvider.GetRequiredService<CsrDbContext>();
        return await dbContext.CertificateRequests.ToListAsync();
    }

    public async Task<CertificateRequestEntity> FindOne(Guid id)
    {
        using var scope = _scopeFactory.CreateScope() ;
        await using var dbContext = scope.ServiceProvider.GetRequiredService<CsrDbContext>();
        return await dbContext.CertificateRequests.SingleAsync(x => x.Id == id);
    }

    public async Task<CertificateRequestEntity> AddCsr(string csr)
    {
        using var scope = _scopeFactory.CreateScope() ;
        await using var dbContext = scope.ServiceProvider.GetRequiredService<CsrDbContext>();
        var csrEntity = new CertificateRequestEntity {CertificateRequest = csr};
        var createdEntity = await dbContext.AddAsync(csrEntity);
        await dbContext.SaveChangesAsync();
        CsrAdded?.Invoke(createdEntity.Entity);
        return createdEntity.Entity;
    }

    public async Task DeleteCsr(Guid id)
    {
        using var scope = _scopeFactory.CreateScope() ;
        await using var dbContext = scope.ServiceProvider.GetRequiredService<CsrDbContext>();
        var entity = await dbContext.CertificateRequests.SingleAsync(x => x.Id == id);
        dbContext.CertificateRequests.Remove(entity);
        await dbContext.SaveChangesAsync();
        CsrRemoved?.Invoke(entity);
    }
}