﻿using BookStoreTask.Utli;
using Microsoft.EntityFrameworkCore;

namespace BookStoreTask.Data;

public class ProjectContext: DbContext
{
    
    public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
    {
    }

    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
    
    
    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity<Guid> &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            var entity = (BaseEntity<Guid>)entityEntry.Entity;
            entity.UpdatedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
        }
    }
    
}