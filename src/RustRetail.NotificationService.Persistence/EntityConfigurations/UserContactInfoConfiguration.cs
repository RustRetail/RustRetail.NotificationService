using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RustRetail.NotificationService.Domain.Entities;

namespace RustRetail.NotificationService.Persistence.EntityConfigurations
{
    internal class UserContactInfoConfiguration : IEntityTypeConfiguration<UserContactInfo>
    {
        const string TableName = "UserContactInfos";

        public void Configure(EntityTypeBuilder<UserContactInfo> builder)
        {
            builder.ToTable(TableName);
        }
    }
}
