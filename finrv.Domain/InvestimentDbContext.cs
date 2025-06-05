using Microsoft.EntityFrameworkCore;

namespace finrv.Domain;

public class InvestimentDbContext : DbContext
{

    public InvestimentDbContext(DbContextOptions<InvestimentDbContext> options) : base(options) {
        Console.WriteLine("Hellow");
    }
}
