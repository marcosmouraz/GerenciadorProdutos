namespace GerenciadorProdutos.Profile;
using AutoMapper;
using GerenciadorProdutos.Data.Dtos.Produto;
using GerenciadorProdutos.Models;

public class ProdutoProfile : Profile
{
    public ProdutoProfile()
    {
        CreateMap<CreateProdutoDto, Produto>();
        CreateMap<Produto, ReadProdutoDto>()
             .ForMember(produtoDto => produtoDto.Categoria,
                    opt => opt.MapFrom(produtoDto => produtoDto.Categoria));
        CreateMap<UpdateProdutoDto, Produto>();
    }
}

