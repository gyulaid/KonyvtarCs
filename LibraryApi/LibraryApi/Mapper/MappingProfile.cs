using AutoMapper;
using LibraryApi.Book;
using LibraryApi.Lending.Dto;
using LibraryApi.Member.Dto;

namespace LibraryApi.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book.Book, BookResponseDto>();
        CreateMap<CreateBookDto, Book.Book>();

        CreateMap<Member.Member, MemberResponseDto>();
        CreateMap<CreateMemberDto, Member.Member>();

        CreateMap<Lending.Lending, LendingResponseDto>();
        CreateMap<CreateLendingDto, Lending.Lending>();
    }
}