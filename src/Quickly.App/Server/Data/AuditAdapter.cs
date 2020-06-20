﻿using Microsoft.EntityFrameworkCore;
using Quickly.App.Server.Models;
using System.Collections.Generic;

namespace Quickly.App.Server.Data
{
    public class AuditAdapter
    {
        public void Snap(IdentityAuditDbContext context)
        {
            var audits = new List<UserAudit>();
            var tracker = context.ChangeTracker;
            foreach (var item in tracker.Entries<ApplicationUser>())
            {
                if (item.State == EntityState.Added ||
                    item.State == EntityState.Deleted ||
                    item.State == EntityState.Modified)
                {
                    var audit = new UserAudit(item.State.ToString(), item.Entity);
                    if (item.State == EntityState.Modified)
                    {
                        var wasConfirmed =
                            (bool)item.OriginalValues[nameof(ApplicationUser.EmailConfirmed)];
                        if (wasConfirmed == false && item.Entity.EmailConfirmed == true)
                        {
                            audit.Action = "Email Confirmed";
                        }
                    }
                    audits.Add(audit);
                }
            }
            if (audits.Count > 0)
            {
                context.Audits.AddRange(audits);
            }
        }
    }
}
