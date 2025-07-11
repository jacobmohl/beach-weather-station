using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace BeachWeatherStation.Infrastructure.Data
{
    public class YearMonthValueGenerator : ValueGenerator<string>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
        {
            // Try to get the CreatedAt property from the entity
            var createdAtProp = entry.Property("CreatedAt");
            DateTime createdAt;
            if (createdAtProp != null && createdAtProp.CurrentValue is DateTime dt)
            {
                createdAt = dt;
            }
            else
            {
                createdAt = DateTime.UtcNow;
            }
            return createdAt.ToString("yyyy-MM");
        }
    }
}
