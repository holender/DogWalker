using GraphQL.Common;
using GraphQL.DataLoader;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types.Sessions
{
    public class ScheduleSessionPayload : SessionPayloadBase
    {
        public ScheduleSessionPayload(Session session)
            : base(session)
        {
        }

        public ScheduleSessionPayload(UserError error)
            : base(new[] { error })
        {
        }

        public ScheduleSessionPayload(IReadOnlyList<UserError> errors)
            : base(errors)
        {
        }
    }
}