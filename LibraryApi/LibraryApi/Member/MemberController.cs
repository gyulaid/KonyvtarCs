using LibraryApi.Member.Dto;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Member;

[ApiController]
[Route("/members")]
public class MemberController : ControllerBase
{
    private readonly MemberService memberService;

    public MemberController(MemberService memberService)
    {
        this.memberService = memberService;
    }

    [HttpGet]
    public List<MemberResponseDto> GetAllMembers()
    {
        return this.memberService.GetAllMember();
    }

    [HttpGet("{id}")]
    public MemberResponseDto GetMember(int id)
    {
        return this.memberService.GetMemberById(id);
    }

    [HttpPost]
    public MemberResponseDto CreateMember([FromBody] CreateMemberDto createDto)
    {
        return this.memberService.CreateMember(createDto);
    }

    [HttpDelete("{id}")]
    public void DeleteMember(int id)
    {
        this.memberService.DeleteMember(id);
    }
}