﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tourism_Guidance_And_Networking.Core.Consts;
using Tourism_Guidance_And_Networking.Core.DTOs.AI_Integration;
using Tourism_Guidance_And_Networking.DataAccess;
using Tourism_Guidance_And_Networking.Web.Services.AI;

namespace Tourism_Guidance_And_Networking.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AIInegrationController : ControllerBase
    {
        private readonly IExternalService _externalService;
        private readonly IUnitOfWork _unitOfWork;

        public AIInegrationController(IExternalService externalService, IUnitOfWork unitOfWork)
        {
            _externalService = externalService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("SeintmentAnalysis")]
        public async Task<IActionResult> MakeSeintmentAnalysis([FromBody] List<CommentsDTO> comments)
        {
            var response = await _externalService.PostDataToBackendAsync(comments);
            return Ok(response);
        }

        [HttpGet("GetUserInteractions")]
        public async Task<IActionResult> GetUserInteractions()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationUser = await _unitOfWork.ApplicationUsers.GetApplicationUserByUserName(userName);

            var userMatrices = await _unitOfWork.UserMatrix.FindAllAsync(um=>um.UserID== applicationUser.Id);

            var itemsIds = userMatrices.Select(x=>x.ItemID).ToList();

            for (int i = 0; i < itemsIds.Count; i++)
                itemsIds[i] = itemsIds[i] - 10000;

            List<int> roomsIds = new List<int>();
            List<int> accomdationsIds = new List<int>();

            InteractionResult interactionResult = new();
            interactionResult.AccomdationsIneractions = new();
            interactionResult.RoomsInteractions = new();

            foreach(var userMatrix in userMatrices)
            {
                var roomMapping = await _unitOfWork.RoomMappings.FindAsync(rm => rm.Item == (userMatrix.ItemID-10000));

                if(roomMapping is not null)
                {
                    var room = await _unitOfWork.Rooms.FindAsync(r => r.Id == roomMapping.Room);
                    // roomsIds.Add(roomMapping.Room);
                    RoomsInteractions roomsInteractions = new()
                    {
                        Action = userMatrix.Action,
                        Room = room,
                        Hotel = await _unitOfWork.Hotels.FindAsync(h => h.Id == room.HotelId)
                    };
                    interactionResult.RoomsInteractions.Add(roomsInteractions);

                }
                else
                {
                    var accommdationMapping = await _unitOfWork.AccomdationMappings.FindAsync(am => am.Item == (userMatrix.ItemID - 10000));

                    if (accommdationMapping is null)
                        return NotFound();

                    //accomdationsIds.Add(accommdationMapping.Accomdation);

                    AccomdationsIneractions accomdationsIneractions = new()
                    {
                        Action = userMatrix.Action,
                        Accommodation = await _unitOfWork.Accommodations.FindAsync(r => r.Id == accommdationMapping.Accomdation)
                    };

                    interactionResult.AccomdationsIneractions.Add(accomdationsIneractions);

                }
            }
                return Ok(interactionResult);
        }

        [HttpGet("RecommendationSystem")]
        public async Task<IActionResult> RecomemndItems(string userId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationUser = await _unitOfWork.ApplicationUsers.GetApplicationUserByUserName(userName);








            var userMatrices = await _unitOfWork.UserMatrix.FindAllAsync(um => um.UserID == applicationUser.Id);

            var itemsIds = userMatrices.Select(x => x.ItemID).ToList();

            for (int i = 0; i < itemsIds.Count; i++)
                itemsIds[i] = itemsIds[i] - 10000;

            List<int> roomsIds = new List<int>();
            List<int> accomdationsIds = new List<int>();

            InteractionResult interactionResult = new();
            interactionResult.AccomdationsIneractions = new();
            interactionResult.RoomsInteractions = new();

            foreach (var userMatrix in userMatrices)
            {
                var roomMapping = await _unitOfWork.RoomMappings.FindAsync(rm => rm.Item == (userMatrix.ItemID - 10000));

                if (roomMapping is not null)
                {
                    var room = await _unitOfWork.Rooms.FindAsync(r => r.Id == roomMapping.Room);
                    // roomsIds.Add(roomMapping.Room);
                    RoomsInteractions roomsInteractions = new()
                    {
                        Action = userMatrix.Action,
                        Room = room,
                        Hotel = await _unitOfWork.Hotels.FindAsync(h => h.Id == room.HotelId)
                    };
                    interactionResult.RoomsInteractions.Add(roomsInteractions);

                }
                else
                {
                    var accommdationMapping = await _unitOfWork.AccomdationMappings.FindAsync(am => am.Item == (userMatrix.ItemID - 10000));

                    if (accommdationMapping is null)
                        return NotFound();

                    //accomdationsIds.Add(accommdationMapping.Accomdation);

                    AccomdationsIneractions accomdationsIneractions = new()
                    {
                        Action = userMatrix.Action,
                        Accommodation = await _unitOfWork.Accommodations.FindAsync(r => r.Id == accommdationMapping.Accomdation)
                    };

                    interactionResult.AccomdationsIneractions.Add(accomdationsIneractions);

                }
            }
            return Ok(interactionResult);

        }



    }
}
