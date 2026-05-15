using Ticketing.Domain.Tickets;
using Ticketing.Domain.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.TicketReplies;

namespace Ticketing.Infrastructure.Configurations;

internal sealed class TicketReplyConfiguration : IEntityTypeConfiguration<TicketReply>
{
    public void Configure(EntityTypeBuilder<TicketReply> builder)
    {
        builder.ToTable("ticket_replies");

        builder.HasKey(reply => reply.Id);

        builder.Property(reply => reply.Id)
               .ValueGeneratedNever();

        builder.Property(reply => reply.Message)
            .HasMaxLength(200)
            .HasConversion(message => message.Value, value => new Message(value));

        builder.Property(reply => reply.CreatedAt)
            .IsRequired();

        builder.Property(reply => reply.TicketId)
            .IsRequired();

        builder.HasOne<Ticket>()
            .WithMany(t => t.Replies)
            .HasForeignKey(reply => reply.TicketId)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(reply => reply.InstructorId)
            .IsRequired();
    }
}