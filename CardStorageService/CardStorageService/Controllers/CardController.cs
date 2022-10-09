using CardStorageService.DataBase.Entities;
using CardStorageService.DataBase.Repository;
using CardStorageService.DataBase.Repository.Impl;
using CardStorageService.Core.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Azure.Core;
using Azure;
using FluentValidation;
using CardStorageService.Services;
using FluentValidation.Results;

namespace CardStorageService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ILogger<CardController> _logger;
        private readonly IRepository<Card, Guid> _cardRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CardDTO> _cardValidator;

        public CardController(ILogger<CardController> logger, IRepository<Card, Guid> cardRepository, IMapper mapper, IValidator<CardDTO> cardValidator)
        {
            _logger = logger;
            _cardRepository = cardRepository;
            _mapper = mapper;
            _cardValidator = cardValidator;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Create([FromBody] CardDTO card)
        {
            ValidationResult validationResult = _cardValidator.Validate(card);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ToDictionary());

            try
            {
                _cardRepository.Create(_mapper.Map<Card>(card));
            }
            catch (Exception e)
            {
                string message = "Create card error";
                _logger.LogError(e, message);
                return Ok(new CardDTO
                {
                    Code = 500,
                    Message = message
                });
            }
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Delete([FromRoute] Guid id)
        {
            try
            {
                _cardRepository.Delete(id);
            }
            catch (Exception e)
            {
                string message = "Delete card error";
                _logger.LogError(e, "Delete card error");
                return Ok(new CardDTO
                {
                    Code = 500,
                    Message = message
                });
            }
            return Ok();
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            try
            {
                var response = _cardRepository.GetAll();
                return Ok(_mapper.Map<IList<CardDTO>>(response));
            }
            catch (Exception e)
            {
                return Ok(Error(e, "Get all card error"));
            }
            return Ok();
        }

        [HttpGet("get-by-id/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult GetById([FromRoute] Guid id)
        {
            try
            {
                var response = _cardRepository.GetById(id);
                return Ok(_mapper.Map<CardDTO>(response));
            }
            catch (Exception e)
            {
                return Ok(Error(e, "Get card error"));
            }
            return Ok();
        }

        [HttpPost("update")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Update([FromBody] CardDTO card)
        {
            try
            {
                if (card.CardId != null)
                {
                    _cardRepository.Update(_mapper.Map<Card>(card));
                }
            }
            catch (Exception e)
            {
                return Ok(Error(e, "Update card error"));
            }
            return Ok();
        }

        private CardDTO Error(Exception e, string message)
        {
            _logger.LogError(e, message);
            return new CardDTO
            {
                Code = 500,
                Message = message
            };
        }
    }
}
