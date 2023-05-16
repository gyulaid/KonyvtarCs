using System.Text.RegularExpressions;
using AutoMapper;
using LibraryApi.Contracts.Member;
using LibraryApi.Database;
using LibraryApi.Exception;

namespace LibraryApi.Member;

public class MemberService
{
    private const string MemberNotFound = "Member was not found with id: ";
    private const string NamePattern = "^[a-zA-Z]+(?:.\\s[a-zA-Z]+)*$";

    private readonly LibraryContext libraryContext;
    private readonly ILogger<MemberService> logger;
    private readonly IMapper mapper;

    public MemberService(LibraryContext libraryContext, ILogger<MemberService> logger, IMapper mapper)
    {
        this.libraryContext = libraryContext;
        this.logger = logger;
        this.mapper = mapper;
    }

    public List<MemberResponseDto> GetAllMember()
    {
        return this.mapper.Map<List<MemberResponseDto>>(this.libraryContext.Members.ToList());
    }

    public MemberResponseDto GetMemberById(int id)
    {
        var member = this.libraryContext.Members.Find(id);
        if (member != null)
        {
            return this.mapper.Map<MemberResponseDto>(member);
        }

        throw new EntityNotFoundException(MemberNotFound + id);
    }

    public MemberResponseDto CreateMember(CreateMemberDto createDto)
    {
        if (IsNameValid(createDto.Name))
        {
            var savedMember = this.libraryContext.Members.Add(this.mapper.Map<Member>(createDto));
            this.libraryContext.SaveChanges();
            this.logger.Log(LogLevel.Information, "A new member was saved");
            return mapper.Map<MemberResponseDto>(savedMember.Entity);
        }
        else
        {
            throw new ArgumentException("Name must contain only alphabetic characters");
        }
    }

    public void DeleteMember(int id)
    {
        var member = this.libraryContext.Members.FindAsync(id);
        if (member.Result != null)
        {
            this.libraryContext.Members.Remove(member.Result);
            this.libraryContext.SaveChanges();
            this.logger.Log(LogLevel.Information, "Member was deleted with id: " + id);
        }
        else
        {
            throw new EntityNotFoundException(MemberNotFound + id);
        }
    }

    private static bool IsNameValid(string name)
    {
        return Regex.IsMatch(name, NamePattern);
    }
}