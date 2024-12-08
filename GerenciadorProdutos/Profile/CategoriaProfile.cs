namespace GerenciadorProdutos.Profile;
using AutoMapper;
using GerenciadorProdutos.Data.Dtos.Categoria;
using GerenciadorProdutos.Models;

public class CategoriaProfile : Profile
{
    public CategoriaProfile()
    {
        CreateMap<CreateCategoriaDto, Categoria>();
        CreateMap<Categoria, ReadCategoriaDto>();
        CreateMap<UpdateCategoriaDto, Categoria>();
    }
}

