using AutoMapper;
using CardStorageService.Core.Models.DTO;
using CardStorageService.DataBase.Entities;

namespace CardStorageService.Services
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            CreateMap<Card, CardDTO>();
            CreateMap<CardDTO, Card>();

            CreateMap<Client, ClientDTO>();
            CreateMap<ClientDTO, Client>();
        }
    }
}
