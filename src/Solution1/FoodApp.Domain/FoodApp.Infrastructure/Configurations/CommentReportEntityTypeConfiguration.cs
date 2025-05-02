using FoodApp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodApp.Infrastructure.Configurations
{
    public class CommentReportEntityTypeConfiguration : IEntityTypeConfiguration<CommentReport>
    {
        public void Configure(EntityTypeBuilder<CommentReport> builder)
        {
            builder.HasKey(cr => cr.Id);

            builder.HasOne(cr => cr.Comment)
                   .WithMany()
                   .HasForeignKey(cr => cr.CommentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cr => cr.Reporter)
                   .WithMany()
                   .HasForeignKey(cr => cr.ReporterId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
