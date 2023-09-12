using GraphQL.Common;
using GraphQL.DataLoader;
using Infrastructure.Data;

namespace GraphQL.Types.Dogs
{
    public class CheckInDogPayload : DogPayloadBase
    {
        private readonly int? _sessionId;

        public CheckInDogPayload(Dog Dog, int sessionId)
            : base(Dog)
        {
            _sessionId = sessionId;
        }

        public CheckInDogPayload(UserError error)
            : base(new[] { error })
        {
        }

        public async Task<Session?> GetSessionAsync(
            SessionByIdDataLoader sessionById,
            CancellationToken cancellationToken)
        {
            if (_sessionId.HasValue)
            {
                return await sessionById.LoadAsync(_sessionId.Value, cancellationToken);
            }

            return null;
        }
    }
}