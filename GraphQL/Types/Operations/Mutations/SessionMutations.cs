using GraphQL.Common;
using GraphQL.Extensions;
using GraphQL.Types.Operations.Subscriptions;
using GraphQL.Types.Sessions;
using HotChocolate.Subscriptions;
using Infrastructure;
using Infrastructure.Data;

namespace GraphQL.Types.Operations.Mutations
{
    [ExtendObjectType(Name = OperationTypeNames.Mutation)]
    public class SessionMutations
    {

        public async Task<AddSessionPayload> AddSessionAsync(
            AddSessionInput input,
            WalkerPlanerDbContext context,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(input.Title))
            {
                return new AddSessionPayload(
                    new UserError("The title cannot be empty.", "TITLE_EMPTY"));
            }

            if (input.WalkerIds.Count == 0)
            {
                return new AddSessionPayload(
                    new UserError("No Walker assigned.", "NO_WALKER"));
            }

            var session = new Session
            {
                Title = input.Title,
                Description = input.Description,
            };

            foreach (var walkerId in input.WalkerIds)
            {
                session.SessionWalkers.Add(new SessionWalker
                {
                    WalkerId = walkerId
                });
            }

            context.Session.Add(session);
            await context.SaveChangesAsync(cancellationToken);

            return new AddSessionPayload(session);
        }

        [UseWalkerPlanerDbContext]
        public async Task<ScheduleSessionPayload> ScheduleSessionAsync(
            ScheduleSessionInput input,
            WalkerPlanerDbContext context,
            [Service] ITopicEventSender eventSender)
        {


            var errors = new List<UserError>();
            if (input.EndTime < input.StartTime)
            {
                errors.Add(
                    new UserError("endTime has to be larger than startTime.", "END_TIME_INVALID"));
            }

            var session = await context.Session.FindAsync(input.SessionId);
            var initialTrackId = session?.TrackId;

            if (session is null)
            {
                errors.Add(
                    new UserError("Session not found.", "SESSION_NOT_FOUND"));
            }

            if (errors.Count > 0)
            {
                return new ScheduleSessionPayload(errors);
            }

            session.TrackId = input.TrackId;
            session.StartTime = input.StartTime;
            session.EndTime = input.EndTime;

            await context.SaveChangesAsync();

            await eventSender.SendAsync(
                nameof(SessionSubscriptions.OnSessionScheduledAsync),
                session.Id);

            return new ScheduleSessionPayload(session);
        }
    }
}