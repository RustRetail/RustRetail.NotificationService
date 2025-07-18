﻿using RustRetail.NotificationService.Domain.Entities;
using RustRetail.SharedKernel.Domain.Abstractions;

namespace RustRetail.NotificationService.Domain.Repositories
{
    public interface IUserContactInfoRepository : IRepository<UserContactInfo, Guid>
    {
    }
}
