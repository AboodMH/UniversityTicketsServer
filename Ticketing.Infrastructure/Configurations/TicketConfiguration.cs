using Ticketing.Domain.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Tickets;

namespace Ticketing.Infrastructure.Configurations;

internal sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("tickets");

        builder.HasKey(ticket => ticket.Id);

        builder.Property(ticket => ticket.Title)
            .HasMaxLength(200)
            .HasConversion(title => title.Value, value => new Title(value));

        builder.Property(ticket => ticket.Description)
            .HasMaxLength(200)
            .HasConversion(description => description.Value, value => new Description(value));

        builder.Property(ticket => ticket.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(ticket => ticket.CreatedAt)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(ticket => ticket.StudentId)
            .IsRequired();

        builder.Navigation(ticket => ticket.Replies)
            .HasField("_replies")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}