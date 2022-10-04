﻿using CardStorageService.DataBase.Entities;
using CardStorageService.DataBase.Repository;
using CardStorageService.Core.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CardStorageService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<CardController> _logger;
        private readonly IRepository<Client, int> _clientRepository;

        public ClientController(ILogger<CardController> logger, IRepository<Client, int> clientRepository)
        {
            _logger = logger;
            _clientRepository = clientRepository;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Create([FromBody] ClientDTO client)
        {
            try
            {
                _clientRepository.Create(new Client
                {
                    Surname = client.Surname,
                    FirstName = client.FirstName,
                    Patronymic = client.Patronymic
                });
            }
            catch (Exception e)
            {
                return Ok(Error(e, "Create client error"));
            }
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                _clientRepository.Delete(id);
            }
            catch (Exception e)
            {
                return Ok(Error(e, "Delete client error"));
            }
            return Ok();
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            try
            {
                var response = _clientRepository.GetAll();
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(Error(e, "Get all clients error"));
            }
            return Ok();
        }

        [HttpGet("get-by-id/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult GetById([FromRoute] int id)
        {
            try
            {
                var response = _clientRepository.GetById(id);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(Error(e, "Get client error"));
            }
            return Ok();
        }

        [HttpPost("update")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Update([FromBody] ClientDTO client)
        {
            try
            {
                if (client.ClientId != null)
                {
                    _clientRepository.Update(new Client
                    {
                        ClientId = client.ClientId.Value,
                        Surname = client.Surname,
                        FirstName = client.FirstName,
                        Patronymic = client.Patronymic
                    });
                }
            }
            catch (Exception e)
            {
                return Ok(Error(e, "Update client error"));
            }
            return Ok();
        }

        private ClientDTO Error(Exception e, string message)
        {
            _logger.LogError(e, message);
            return new ClientDTO
            {
                Code = 500,
                Message = message
            };
        }
    }
}
