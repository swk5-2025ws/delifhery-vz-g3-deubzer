using DeliFHery.API.Dto;
using DeliFHery.API.Models;
using Riok.Mapperly.Abstractions;

namespace DeliFHery.API.DtoMappers
{
    [Mapper]
    public static partial class CustomerDtoMapper
    {
        public static partial CustomerDto ToDto(Customer customer);
        public static partial Customer FromDto(CustomerDto dto);
    }
}
