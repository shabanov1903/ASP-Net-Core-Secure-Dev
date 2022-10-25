using AutoMapper;
using CardStorageService.DataBase.Entities;
using CardStorageService.DataBase.Repository;
using CardStorageService.Protos;
using Google.Protobuf.Collections;
using Grpc.Core;
using static CardStorageService.Protos.ClientService;

namespace CardStorageService.Controllers.RPC
{
    public class ClientControllerRPC : ClientServiceBase
    {
        private readonly ILogger<CardController> _logger;
        private readonly IRepository<Client, int> _clientRepository;
        private readonly IMapper _mapper;

        public ClientControllerRPC(ILogger<CardController> logger, IRepository<Client, int> clientRepository, IMapper mapper)
        {
            _logger = logger;
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public override Task<IdRPC> Create(ClientRPC request, ServerCallContext context)
        {
            try
            {
                int result = _clientRepository.Create(_mapper.Map<Client>(request));
                return Task.FromResult(new IdRPC { ClientId = result });
            }
            catch (Exception e)
            {
                _logger.LogError($"Create gRPC Error: {e.Message}");
                return Task.FromResult(new IdRPC());
            }
        }

        public override Task<ClientRPC> GetById(IdRPC request, ServerCallContext context)
        {
            try
            {
                var result = _clientRepository.GetById(request.ClientId);
                return Task.FromResult(_mapper.Map<ClientRPC>(result));
            }
            catch (Exception e)
            {
                _logger.LogError($"Get By Id gRPC Error: {e.Message}");
                return Task.FromResult(new ClientRPC());
            }
        }

        public override Task<ClientListRPC> GetAll(GetAllPRC request, ServerCallContext context)
        {
            try
            {
                var result = new ClientListRPC();
                result.Clients.AddRange(_mapper.Map<RepeatedField<ClientRPC>>(_clientRepository.GetAll()));
                return Task.FromResult(result);
            }
            catch (Exception e)
            {
                _logger.LogError($"Get All gRPC Error: {e.Message}");
                return Task.FromResult(new ClientListRPC());
            }
        }
    }
}
