namespace Wejo.Game.Application.Interfaces;

using Common.Domain.Dtos;
using Request;

public interface IGameChatService
{
    Task<GameChatMessageDto> SendMessageAsync(Guid gameId, string userId, GameChatSendMessageR request, CancellationToken cancellationToken);
}
