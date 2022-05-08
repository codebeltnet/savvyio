using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Savvyio.Assets.Domain;

namespace Savvyio.Assets
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder AddAccount(this ModelBuilder mb)
        {
            return mb.Entity<Account>(entity =>
            {
                entity.ToTable(nameof(Account));
                entity.HasKey(x => new { x.Id });
                entity.Property(x => x.Id)
                    .HasColumnName("id")
                    .HasValueGenerator<LongValueGenerator>()
                    .ValueGeneratedOnAdd();
                entity.Property(x => x.PlatformProviderId)
                    .HasColumnName("platformProviderId")
                    .HasColumnType("uniqueidentifier");
                entity.Property(x => x.EmailAddress)
                    .HasColumnName("emailAddress")
                    .HasColumnType("varchar(256)");
                entity.Property(x => x.FullName)
                    .HasColumnName("fullName")
                    .HasColumnType("varchar(256)");
                entity.Property(x => x.Metadata)
                    .HasConversion(ap => JsonConvert.SerializeObject(ap), ap => JsonConvert.DeserializeObject<MetadataDictionary>(ap));
            });
        }

        public static ModelBuilder AddPlatformProvider(this ModelBuilder mb)
        {
            return mb.Entity<PlatformProvider>(entity =>
            {
                entity.ToTable(nameof(PlatformProvider));
                entity.HasKey(x => new { x.Id });
                entity.Property(x => x.Id)
                    .HasColumnName("id");
                entity.Property(x => x.Policy)
                    .HasConversion(ap => JsonConvert.SerializeObject(ap), ap => JsonConvert.DeserializeObject<PlatformProviderAccountPolicy>(ap));
                entity.Property(x => x.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(256)");
                entity.Property(x => x.ThirdLevelDomainName)
                    .HasColumnName("thirdLevelDomainName")
                    .HasColumnType("varchar(256)");
                entity.Property(x => x.Description)
                    .HasColumnName("description")
                    .HasColumnType("varchar(256)");
                entity.Property(x => x.Metadata)
                    .HasConversion(ap => JsonConvert.SerializeObject(ap), ap => JsonConvert.DeserializeObject<MetadataDictionary>(ap));
            });
        }
    }
}
