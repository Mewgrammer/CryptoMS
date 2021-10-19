using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Contracts;

public class UtcDateValueGenerator : ValueGenerator
{
    protected override object? NextValue(EntityEntry entry)
    {
        return DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
    }
    public override bool GeneratesTemporaryValues => false;
}