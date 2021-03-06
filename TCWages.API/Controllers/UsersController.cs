using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TCWages.API.Data;
using TCWages.API.Dtos;
using TCWages.API.Helpers;
using TCWages.API.Models;

namespace TCWages.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity)) ]
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser(currentUserId);
            userParams.UserId = currentUserId;

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }
            
            var users = await _repo.GetUsers(userParams);
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name="GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        // Needs always to add [FromBody] to correctly use JSON.
        // TODO: Create repeated methods for JSON and NON JSON
        public async Task<IActionResult> UpdateUser(int id, [FromBody]UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();
            
            if (userForUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(userForUpdateDto));
            }

            var userFromRepo = await _repo.GetUser(id);

            _mapper.Map(userForUpdateDto, userFromRepo);

            if (await _repo.SaveAll())
            return NoContent();
            else
            return BadRequest("No changes were made! Records not Updated.");

            //throw new Exception($" Updating user {id} failed on save");

        }

        [HttpPost("{id}/like/{recipientId}")]

        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();

            var like = await _repo.GetLike(id, recipientId);

            if (like != null)
            return BadRequest("You already like this user");

            if (await _repo.GetUser(recipientId) == null )
            return NotFound();

            like = new Like
            {
                LikerID = id,
                LikeeID = recipientId
                
            };

            _repo.Add<Like>(like);

            if (await _repo.SaveAll())
            return Ok();

            return BadRequest("Failed to like this user");


        }
    }
}