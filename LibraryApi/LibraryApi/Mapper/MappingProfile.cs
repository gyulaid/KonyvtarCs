using AutoMapper;
using LibraryApi.Contracts.Book;
using LibraryApi.Contracts.Lending;
using LibraryApi.Contracts.Member;

namespace LibraryApi.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book.Book, BookResponseDto>();
        CreateMap<CreateBookDto, Book.Book>()
            .ForMember(
                dest => dest.IsAvailable,
                opt => opt.MapFrom(src => true));

        CreateMap<Member.Member, MemberResponseDto>();
        CreateMap<CreateMemberDto, Member.Member>();

        CreateMap<Lending.Lending, LendingResponseDto>();
        CreateMap<CreateLendingDto, Lending.Lending>();
    }
}