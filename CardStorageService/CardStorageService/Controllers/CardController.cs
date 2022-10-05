using CardStorageService.DataBase.Entities;
using CardStorageService.DataBase.Repository;
using CardStorageService.DataBase.Repository.Impl;
using CardStorageService.Core.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CardStorageService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ILogger<CardController> _logger;
        private readonly IRepository<Card, Guid> _cardRepository;

        public CardController(ILogger<CardController> logger, IRepository<Card, Guid> cardRepository)
        {
            _logger = logger;
            _cardRepository = cardRepository;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Create([FromBody] CardDTO card)
        {
            try
            {
                _cardRepository.Create(new Card
                {
                    ClientId = card.ClientId,
                    CardNo = card.CardNo,
                    Name = card.Name,
                    CVV2 = card.CVV2,
                    ExpDate = card.ExpDate
                });
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
                return Ok(response);
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
                return Ok(response);
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
                    _cardRepository.Update(new Card
                    {
                        CardId = card.CardId.Value,
                        ClientId = card.ClientId,
                        CardNo = card.CardNo,
                        Name = card.Name,
                        CVV2 = card.CVV2,
                        ExpDate = card.ExpDate
                    });
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
