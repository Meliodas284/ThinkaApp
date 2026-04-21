using Thinka.Domain.Dto;
using Thinka.Domain.Dto.Chat;
using Thinka.Domain.Entities;
using Thinka.Domain.Exceptions;
using Thinka.Domain.Interfaces.Repositories;
using Thinka.Domain.Interfaces.Services;

namespace Thinka.Application.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IConversationRepository _conversationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IChatHubService _chatHubService;
    private readonly ICurrentUserService _currentUserService;

    public MessageService(
        IMessageRepository messageRepository,
        IConversationRepository conversationRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IChatHubService chatHubService, 
        ICurrentUserService currentUserService)
    {
        _messageRepository = messageRepository;
        _conversationRepository = conversationRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _chatHubService = chatHubService;
        _currentUserService = currentUserService;
    }

    public async Task<List<MessageDto>> GetMessages(Guid userId, Guid conversationId, PaginationQuery pagination)
    {
        var conversation = await _conversationRepository.GetById(conversationId);
        if (conversation is null || !conversation.HasParticipant(userId))
            throw new ForbiddenException("You do not have access to this conversation.");

        var messages = await _messageRepository.GetByConversation(conversationId, pagination);
        
        return messages.Select(m => new MessageDto
        {
            Id = m.Id,
            ConversationId = m.ConversationId,
            SenderId = m.SenderId,
            Text = m.Text,
            CreatedAt = m.CreatedAt
        }).ToList();
    }

    public async Task<MessageDto> SendMessage(Guid senderId, SendMessageDto messageDto)
    {
        var recipient = await _userRepository.GetByIdAsync(messageDto.RecipientId);
        if (recipient is null)
            throw new NotFoundException("Recipient not found.");

        if (senderId == messageDto.RecipientId)
            throw new ArgumentException("You cannot send a message to yourself.");

        if (string.IsNullOrWhiteSpace(messageDto.Text))
            throw new ArgumentException("Message text cannot be empty.");

        using var transaction = await _unitOfWork.BeginTransactionAsync();

        var conversation = await GetOrCreateConversation(senderId, messageDto.RecipientId);       
        var message = new Message
        {
            ConversationId = conversation.Id,
            SenderId = senderId,
            Text = messageDto.Text,
        };

        conversation.LastMessageAt = DateTimeOffset.UtcNow;

        await _messageRepository.Add(message);
        await _unitOfWork.SaveChangesAsync();    
        await transaction.CommitAsync();

        var messageResult = new MessageDto
        {
            Id = message.Id,
            ConversationId = message.ConversationId,
            SenderId = message.SenderId,
            Text = message.Text,
            CreatedAt = message.CreatedAt
        };
        
        await _chatHubService.BroadcastMessage(messageResult);

        return messageResult;
    }
    
    private async Task<Conversation> GetOrCreateConversation(Guid user1Id, Guid user2Id)
    {
        var requestConversation = Conversation.Create(user1Id, user2Id);
        var conversation = await _conversationRepository.GetBetweenUsers(requestConversation.User1Id, requestConversation.User2Id);

        if (conversation is not null) 
            return conversation;
        
        await _conversationRepository.Add(requestConversation);

        return requestConversation;
    }
}