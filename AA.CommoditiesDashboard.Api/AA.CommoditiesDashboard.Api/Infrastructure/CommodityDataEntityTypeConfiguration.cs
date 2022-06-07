using AA.CommoditiesDashboard.Api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AA.CommoditiesDashboard.Api.Infrastructure
{
    public class CommodityDataEntityTypeConfiguration : IEntityTypeConfiguration<CommodityData>
    {
        public void Configure(EntityTypeBuilder<CommodityData> builder)
        {
            builder.ToTable("CommodityData");

            builder.HasKey(cd => cd.Id);

            builder.Property(cd => cd.Contract)
                .IsRequired()
                .HasMaxLength(6);
            
            builder.Property(cd => cd.Date)
                .IsRequired();
            
            builder.Property(cd => cd.Position)
                .IsRequired();
            
            builder.Property(cd => cd.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            
            builder.Property(cd => cd.PnlDaily)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(cd => cd.Commodity)
                .WithMany()
                .HasForeignKey(cd => cd.CommodityId);

        }
    }
}