using CardStorageService.DataBase.Entities;
using CardStorageService.DataBase.Repository;
using CardStorageService.Core.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace CardStorageService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<CardController> _logger;
        private readonly IRepository<Client, int> _clientRepository;
        private readonly IMapper _mapper;

        public ClientController(ILogger<CardController> logger, IRepository<Client, int> clientRepository, IMapper mapper)
        {
            _logger = logger;
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Create([FromBody] ClientDTO client)
        {
            try
            {
                _clientRepository.Create(_mapper.Map<Client>(client));
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
                return Ok(_mapper.Map<IList<ClientDTO>>(response));
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
                return Ok(_mapper.Map<ClientDTO>(response));
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
                    _clientRepository.Update(_mapper.Map<Client>(client));
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
