using AutoMapper;
using Randomizer.Application.DTOs;
using Randomizer.Application.DTOs.FileSystem;
using Randomizer.Domain.Entities;

namespace Randomizer.Application.Mapper;

public class GameConfigMappingProfile : Profile
{
	public GameConfigMappingProfile()
	{
		CreateMap<CreateGameConfigDto, GameConfigEntity>()
			.ForMember(dest => dest.Id, src => src.Ignore())
			.ForMember(dest => dest.DisplayId, src => src.Ignore())
			.ForMember(dest => dest.Rounds, src => src.Ignore());

		CreateMap<GameConfigEntity, GameConfigDto>();

		CreateMap<string, ParticipantEntity>()
			.ForMember(dest => dest.NickName, src => src.MapFrom(x => x))
			.ForMember(dest => dest.Id, src => src.Ignore())
			.ForMember(dest => dest.Position, src => src.Ignore())
			.ForMember(dest => dest.StartGameConfigId, src => src.Ignore())
			.ForMember(dest => dest.StartGameConfig, src => src.Ignore());

		CreateMap<string, MessageEntity>()
            .ForMember(dest => dest.Content, src => src.MapFrom(x => x))
            .ForMember(dest => dest.Id, src => src.Ignore())
            .ForMember(dest => dest.Position, src => src.Ignore())
            .ForMember(dest => dest.StartGameConfigId, src => src.Ignore())
            .ForMember(dest => dest.StartGameConfig, src => src.Ignore());

		CreateMap<ParticipantEntity, ParticipantDto>();

		CreateMap<MessageEntity, MessageDto>();

		CreateMap<RoundEntity, RoundDto>()
			.ForMember(dest => dest.LastRound, src => src.Ignore());

		CreateMap<UpdateRoundResultDto, RoundResultEntity>()
			.ForMember(dest => dest.SequenceNumber, src => src.Ignore())
			.ForMember(dest => dest.Message, src => src.Ignore())
			.ForMember(dest => dest.MessageId, src => src.Ignore())
			.ForMember(dest => dest.WhoPerformAction, src => src.Ignore())
			.ForMember(dest => dest.WhoPerformActionId, src => src.Ignore())
			.ForMember(dest => dest.WhoPerformFeedback, src => src.Ignore())
			.ForMember(dest => dest.WhoPerformFeedbackId, src => src.Ignore())
			.ForMember(dest => dest.Round, src => src.Ignore())
            .ForMember(dest => dest.RoundId, src => src.Ignore());

		CreateMap<RoundResultEntity, RoundResultDto>();

		// Document Dto Mappings
		CreateMap<GameConfigEntity, GameResultsDocumentDto>();

		CreateMap<RoundEntity, RoundDocumentDto>();
    }
}
